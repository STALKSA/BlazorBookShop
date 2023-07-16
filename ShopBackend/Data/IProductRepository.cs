namespace ShopBackend.Data
{
	public interface IProductRepository
	{
		Task Add(Product product, CancellationToken cancellationToken);
		Task Delete(Product product, CancellationToken cancellationToken);
		Task Update(Product product, CancellationToken cancellationToken);
		Task<Product> GetProductById(Guid id, CancellationToken cancellationToken);
	}
}
