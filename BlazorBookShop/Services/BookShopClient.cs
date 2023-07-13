using BlazorBookShop.Interfaces;
using BlazorBookShop.Models;
using System;
using System.Net.Http.Json;

namespace BlazorBookShop.Services
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

			if (Uri.TryCreate(String.Format("https://{0}", host), UriKind.Absolute, out var hostUri))
            {
                throw new ArgumentException("Адрес хоста должен быть валидного значения", nameof(host));
            }

            _host = host;
            _httpClient = httpClient ?? new HttpClient();
            if (httpClient!.BaseAddress is null)
            {
                _httpClient.BaseAddress = hostUri;
            }
        }

        public void Dispose()
        {
            ((IDisposable)_httpClient).Dispose();
        }

        public async Task<Product[]> GetProducts()
        {
            var products = await _httpClient.GetFromJsonAsync<Product[]>($"get_products");
            if (products is null)
            {
                throw new InvalidOperationException("Сервер вернул продукт со значением null");
            }
            return products;
        }

        public async Task<Product> GetProduct(Guid id)
        {
			ArgumentNullException.ThrowIfNull(id);
			var product = await _httpClient.GetFromJsonAsync<Product>($"get_product?id={id}");
            if (product is null)
            {
                throw new InvalidOperationException("Сервер вернул продукт со значением null");
            }
            return product;

        }

        public async Task AddProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            using var response = await _httpClient.PostAsJsonAsync("add_product", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            using var response = await _httpClient.PostAsJsonAsync($"update_product", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);
            using var response = await _httpClient.PostAsync($"delete_product", null);
            response.EnsureSuccessStatusCode();
        }

    }
}
