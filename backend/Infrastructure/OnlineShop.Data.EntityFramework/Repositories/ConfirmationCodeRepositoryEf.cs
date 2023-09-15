using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.WebApi.Data.Repositories;

namespace OnlineShop.Data.EntityFramework.Repositories
{
    public class ConfirmationCodeRepositoryEf : EfRepository<ConfirmationCode>, IConfirmationCodeRepository
    {
        public ConfirmationCodeRepositoryEf(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
