﻿using BlazorBookShop.Models;
using BookShop.HttpApiClient.Exceptions;
//using BookShop.HttpApiClient.Models;
using OnlineShop.HttpModals.Requests;
using OnlineShop.HttpModals.Responses;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
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

        public async Task Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            using var response = await _httpClient.PostAsJsonAsync("account/register", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
                    throw new MyBookShopApiException(error!);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
                    throw new MyBookShopApiException(response.StatusCode, details!);
                }
                else
                {
                    throw new MyBookShopApiException("Неизвестная ошибка");
                }
            }

        }

        public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            using var response = await _httpClient.PostAsJsonAsync("account/login", request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken:cancellationToken);
                    throw new MyBookShopApiException(error!);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken:cancellationToken);
                    throw new MyBookShopApiException(response.StatusCode, details!);
                }
                else
                {
                    throw new MyBookShopApiException($"Неизвестная ошибка: {response.StatusCode}");
                }
            }
            
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken:cancellationToken);
            return loginResponse!;
        }
    }
}
