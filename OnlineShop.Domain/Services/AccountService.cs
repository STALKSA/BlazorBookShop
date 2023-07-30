

using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class AccountService

    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            
              _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            
        }
        public async Task Register(string name, string email, string password, CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(email);
            ArgumentNullException.ThrowIfNull(password);

            var existedAccount = await _accountRepository.FindAccountByEmail(email, cancellationToken);
            if (existedAccount is not null)
            {
                throw new EmailAlreadyExistsException("Аккаунт с данным Email'ом уже существует");
               
            }
            var account = new Account(name, email, EncryptPassword(password));
            await _accountRepository.Add(account, cancellationToken);

        }

        private static string EncryptPassword(string password)
        {
            return password;
        }
    }
}
