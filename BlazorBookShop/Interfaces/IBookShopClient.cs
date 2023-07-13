using BlazorBookShop.Models;

namespace BlazorBookShop.Interfaces
{
    public interface IBookShopClient
    {
		Task AddProduct(Product product);
		Task<Product> GetProduct(Guid id);
		Task<Product[]> GetProducts();
		Task UpdateProduct(Product product);
		Task DeleteProduct(Product product);

	}
}