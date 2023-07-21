namespace ShopBackend.Data
{
	public interface IProductRepository
	{
		Task AddProduct(Product product, CancellationToken cancellationToken);
		Task DeleteProduct(Product product, CancellationToken cancellationToken);
		Task UpdateProduct(Product product, CancellationToken cancellationToken);
		Task<Product> GetProductById(Guid id, CancellationToken cancellationToken);
		Task<Product[]> GetProducts(CancellationToken cancellationToken);
	}
}
