using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;

namespace OnlineShop.Domain.Services
{
    public class AccountService

    {
        private readonly IAccountRepository _accountRepository;
        private readonly IApplicationPasswordHasher _hasher;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, 
            IApplicationPasswordHasher hasher,
            ILogger<AccountService> logger)
        {  
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task Register(string name, string email, string password, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            var existedAccount = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Аккаунт с данным Email уже существует");
               
            }
            var account = new Account(name, email, EncryptPassword(password));
            await _accountRepository.Add(account, cancellationToken);

        }
   

        public async Task<Account> Login(string email, string password, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            var account = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (account is not null)
            {
                throw new AccountNotFoundException("Аккаунт с данным Email не найден");
            }

            var isPasswordValid = _hasher.VerifyHashedPassword(account.HashedPassword, password, out var rehashNeeded);
            if(!isPasswordValid)
            {
                throw new InvalidPasswordException("Не валидный пароль");
            }
            if(rehashNeeded)
            {
                await RehashPassword(password, account, cancellationToken);
            }

            return account;

        }

        private string EncryptPassword(string password)
        {
            var hashedPassword = _hasher.HashPassword(password);
            _logger.LogDebug($"Password hashed: {hashedPassword}", hashedPassword);
            return password;
        }

        private Task RehashPassword(string password, Account? account, CancellationToken cancellationToken)
        {
            account.HashedPassword = EncryptPassword(password);
            return _accountRepository.Update(account, cancellationToken);
        }
    }
}
