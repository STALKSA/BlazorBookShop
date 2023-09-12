using BlazorBookShop.Models;
using BookShop.HttpApiClient.Extentions;
//using BookShop.HttpApiClient.Models;
using OnlineShop.HttpModals.Requests;
using OnlineShop.HttpModals.Responses;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BookShop.HttpApiClient
{
    public class BookShopClient : IDisposable, IBookShopClient
    {
        private readonly string _host;
        private readonly HttpClient _httpClient;
        private bool _httpClientInjected = false;

        public BookShopClient(string host = "https://bookshop.com/", HttpClient? httpClient = null)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException(nameof(host));


            var hostUri = new Uri(host, UriKind.Absolute);

            _host = host;
            _httpClient = httpClient ?? new HttpClient();
            if (_httpClient!.BaseAddress is null)
            {
                _httpClient.BaseAddress = hostUri;
            }
        }

        public void Dispose()
        {
            ((IDisposable)_httpClient).Dispose();
        }

        public async Task<Product[]> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _httpClient.GetFromJsonAsync<Product[]>($"get_products", cancellationToken);
            if (products is null)
            {
                throw new InvalidOperationException("Сервер вернул продукт со значением null");
            }
            return products;
        }

        public async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id);
            var product = await _httpClient.GetFromJsonAsync<Product>($"get_product?id={id}", cancellationToken);
            if (product is null)
            {
                throw new InvalidOperationException("Сервер вернул продукт со значением null");
            }
            return product;

        }

        public async Task AddProduct(Product? product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product);
            using var response = await _httpClient.PostAsJsonAsync("add_product", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProduct(Guid id, Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product);
            using var response = await _httpClient.PostAsJsonAsync($"update_product?id={id}", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Product? product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product);
            using var response = await _httpClient.PostAsJsonAsync("delete_product", product, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            const string uri = "account/register";
            return await _httpClient.PostAsJsonAnsDeserializeAsync<RegisterRequest, RegisterResponse>(request, uri, cancellationToken);

        }

        public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            const string uri = "account/login";
            var response = await _httpClient.PostAsJsonAnsDeserializeAsync<LoginRequest, LoginResponse>(request, uri, cancellationToken);
            SetAuthorizationToken(response.Token);
            return response;
        }

        public void SetAuthorizationToken(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            var header = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Authorization = header;
            IsAuthorizationTokenSet = true;
        }
        public bool IsAuthorizationTokenSet { get; private set; }

        public async Task<AccountResponse> GetCurrentAccount(CancellationToken cancellationToken)
        {
            var accountResponse = await _httpClient
                .GetFromJsonAsync<AccountResponse>($"account/current", cancellationToken);
            if (accountResponse is null)
            {
                throw new InvalidOperationException("The server returned null");
            }
            return accountResponse;
        }

        public async Task AddCartItemToCart(AddCartItemRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            using var response = await _httpClient.PostAsJsonAsync("cart/add", request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<CartResponse> GetCart(CancellationToken cancellationToken)
        {
            var cartItems = await _httpClient
                .GetFromJsonAsync<CartResponse>($"cart", cancellationToken);
            if (cartItems is null)
            {
                throw new InvalidOperationException("Сервер вернул null");
            }
            return cartItems;
        }
    }

}
