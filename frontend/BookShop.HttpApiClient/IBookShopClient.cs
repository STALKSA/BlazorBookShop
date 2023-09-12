using BlazorBookShop.Models;
//using BookShop.HttpApiClient.Models;
using OnlineShop.HttpModals.Requests;
using OnlineShop.HttpModals.Responses;

namespace BookShop.HttpApiClient
{
    public interface IBookShopClient
    {
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<Product[]> GetProducts(CancellationToken cancellationToken);
        Task UpdateProduct(Guid id, Product product, CancellationToken cancellationToken);
        Task DeleteProduct(Product product, CancellationToken cancellationToken);
        Task<RegisterResponse> Register(RegisterRequest account, CancellationToken cancellationToken);
        Task<LoginResponse> Login(LoginRequest account, CancellationToken cancellationToken);
        Task<AccountResponse> GetCurrentAccount(CancellationToken cancellationToken);
        void SetAuthorizationToken(string token);
        Task AddCartItemToCart(AddCartItemRequest request, CancellationToken cancellationToken);
        Task<CartResponse> GetCart(CancellationToken cancellationToken);
    }
}