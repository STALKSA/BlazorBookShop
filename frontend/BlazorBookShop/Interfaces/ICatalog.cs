using BlazorBookShop.Models;

namespace BlazorBookShop.Interfaces
{
    public interface ICatalog
    {
        Task AddProduct(Product product);
        Task ClearProducts();
        Task DeleteProductById(Guid productId);
        Task<Product> GetProductByIdAsync(Guid productId);
        Task<List<Product>> GetProductsAsync();
        Task UpdateProductById(Guid productId, Product newProduct);
    }
}