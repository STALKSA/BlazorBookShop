using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace ShopBackend.Data.Repositories
{
    public class AccountRepositoryEf : EfRepository<Account>, IAccountRepository
    {
        public AccountRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {        
        }

        public Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return Entities.SingleAsync(x => x.Email == email, cancellationToken: cancellationToken);
        }

        public Task<Account?> FindAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if(email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return Entities.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
        }


    }
}
