
using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken);
        Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken);
    }
}
