using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace ShopBackend.Data.Repositories
{
    public class ProductRepositoryEf : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepositoryEf(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteProduct(Product product, CancellationToken cancellationToken)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _dbContext.Remove(product);
            return _dbContext.SaveChangesAsync(cancellationToken);

        }

        public Task<Product> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Products.FirstAsync(product => product.Id == id, cancellationToken: cancellationToken);
        }

        public Task UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _dbContext.Update(product);
            return _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Product[]> GetProducts(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.ToArrayAsync(cancellationToken);
        }
    }
}
