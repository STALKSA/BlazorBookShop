using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;

namespace OnlineShop.Domain.Services
{
    public class AccountService

    {
        
        private readonly IApplicationPasswordHasher _hasher;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, 
            IApplicationPasswordHasher hasher,
            IUnitOfWork uow,
            ILogger<AccountService> logger)
        {  
            
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public virtual async Task Register(string name, string email, string password, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            var existedAccount = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Аккаунт с данным Email уже существует");
               
            }
            var account = new Account(Guid.NewGuid(), name, email, EncryptPassword(password));
            Cart cart = new(Guid.NewGuid(), account.Id);
            await _uow.AccountRepository.Add(account, cancellationToken);
            await _uow.CartRepository.Add(cart, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

        }
   

        public virtual async Task<(Account account, Guid codeId)> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            var account = await LoginByPassword(email, password, cancellationToken);

            var code = await CreateAndSendConfirmationCode(account, cancellationToken);

            return (account, code.Id);

        }

        public virtual async Task<Account> LoginByPassword(string email, string password, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var account = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
            {
                throw new AccountNotFoundException("Аккаунт не найден");
            }

            var isPasswordValid = _hasher.VerifyHashedPassword(account.HashedPassword, password, out var rehashNeeded);
            if (!isPasswordValid)
            {
                throw new InvalidPasswordException("Не валидный пароль");
            }

            if (rehashNeeded)
            {
                await RehashPassword(password, account, cancellationToken);
            }

            return account;
        }

        public async Task<Account> LoginByCode(string email, Guid codeId, string code, CancellationToken cancellationToken)
        {

            var codeObject = await _uow.ConfirmationCodeRepository.GetById(codeId, cancellationToken);
            if (codeObject is null)
                throw new CodeNotFoundException("There is no Code for this CodeId");
            if (codeObject.Code != code)
                throw new InvalidCodeException("Code not confirmed!");
            var account = await _uow.AccountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is null)
                throw new AccountNotFoundException("Account not found");
            return account;
        }

        private async Task<ConfirmationCode> CreateAndSendConfirmationCode(Account account, CancellationToken cancellationToken)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (account.Email == null) throw new ArgumentNullException(nameof(account));
            var code = GeneraNewConfirmationCode(account);
            await _uow.ConfirmationCodeRepository.Add(code, cancellationToken);

            _logger.LogInformation($"Email sent from with password: {code.Code}");
            // await _emailSender.SendEmailAsync(
            //     account.Email,
            //     "Подтверждение входа",
            //     code.Code, cancellationToken);

            await _uow.SaveChangesAsync(cancellationToken);
            return code;
        }

        private ConfirmationCode GeneraNewConfirmationCode(Account account)
        {
            return new ConfirmationCode(Guid.NewGuid(), account.Id, DateTimeOffset.Now,
                DateTimeOffset.Now);
        }

        private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            _logger.LogDebug($"Password hashed: {hashedPassword}", hashedPassword);
            return hashedPassword;
        }

        private async Task RehashPassword(string password, Account? account, CancellationToken cancellationToken)
        {
            account.HashedPassword = EncryptPassword(password);
            await _uow.AccountRepository.Update(account, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        public async Task<Account> GetAccountById(Guid id, CancellationToken cancellationToken)
        {
            return await _uow.AccountRepository.GetById(id, cancellationToken);
        }
    }
}
