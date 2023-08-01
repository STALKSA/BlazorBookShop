using BlazorBookShop.Models;
//using BookShop.HttpApiClient.Models;
using OnlineShop.HttpModals.Requests;

namespace BlazorBookShop.Interfaces
{
    public interface IBookShopClient
    {
		Task AddProduct(Product product, CancellationToken cancellationToken);
		Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
		Task<Product[]> GetProducts(CancellationToken cancellationToken);
		Task UpdateProduct(Guid id, Product product, CancellationToken cancellationToken);
		Task DeleteProduct(Product product, CancellationToken cancellationToken);

		Task Register(RegisterRequest account, CancellationToken cancellationToken);

	}
}