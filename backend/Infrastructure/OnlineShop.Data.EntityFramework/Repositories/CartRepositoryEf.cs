using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.WebApi.Data.Repositories;

namespace OnlineShop.Data.EntityFramework.Repositories
{
    public class CartRepositoryEf : EfRepository<Cart>, ICartRepository
    {
        public CartRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Cart> GetCartByAccountId(Guid id, CancellationToken cancellationToken)
        {
            var ent = await Entities.FirstOrDefaultAsync(it => it.AccountId == id, cancellationToken);
            if (ent is null)
            {
                throw new AccountNotFoundException("Аккаунт не найден");
            }
            var cart = await Entities
                .Include(it => it.Items)
                .Where(it => it.AccountId == id)
                .FirstAsync(cancellationToken);
            return cart;
        }
    }
}
