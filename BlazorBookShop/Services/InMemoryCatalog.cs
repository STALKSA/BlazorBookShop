using BlazorBookShop.Interfaces;
using BlazorBookShop.Models;
using System;
using System.Collections.Concurrent;

namespace BlazorBookShop.Services
{
    public class InMemoryCatalog : ICatalog
    {
        private ConcurrentDictionary<Guid, Product> _products = GenerateProducts(10);

        public async Task<List<Product>> GetProductsAsync()
        {
            return _products.Values.ToList();
        }


        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            if (!_products.TryGetValue(productId, out var product))
            {
                throw new KeyNotFoundException($"Id {productId} не найден.");
            }
            else
            {
                return product;
            }
        }


        public Task AddProduct(Product product)
        {
            if (!_products.TryAdd(product.Id, product))
            {
                throw new ArgumentException($"Товар {product.Id} уже существует.");
            };
            return Task.CompletedTask;

        }


        public Task DeleteProductById(Guid productId)
        {
            if (!_products.TryRemove(productId, out _))
            {
                throw new InvalidOperationException($"Товар {productId} не найден");
            }
            return Task.CompletedTask;

        }


        public Task ClearProducts()
        {
            _products.Clear();
            return Task.CompletedTask;

        }


        public Task UpdateProductById(Guid productId, Product newProduct)
        {
            if (!_products.TryGetValue(productId, out var oldProductValue))
            {
                throw new KeyNotFoundException($" ID {newProduct.Id} не найден.");
            }
            if (!_products.TryUpdate(productId, newProduct, oldProductValue))
            {
                throw new InvalidCastException($"Операция прервана...");
            }

            return Task.CompletedTask;
        }


        static ConcurrentDictionary<Guid, Product> GenerateProducts(int count)
        {
            var random = new Random();
            var products = new ConcurrentDictionary<Guid, Product>();

            var productImages = new ProductsImages[]
            {
                new ProductsImages("Паттерны проектирования", "book1.jpg"),
                new ProductsImages("Чистый код", "book2.jpg"),
                new ProductsImages("Совершенный код. Мастер-класс", "book3.jpg"),
                new ProductsImages("Алгоритмы: построение и анализ", "book4.jpeg"),
                new ProductsImages("Фундаментальные основы хакерства", "book5.jpg"),
                new ProductsImages("Человеческий фактор: успешные проекты и команды", "book6.jpg"),
                new ProductsImages("Грокаем алгоритмы. Иллюстрированное пособие для программистов и любопытствующих", "book7.jpeg"),
                new ProductsImages("Программист-фанатик", "book8.jpg"),
                new ProductsImages("Карьера программиста", "book9.jpeg"),
                new ProductsImages("Рефакторинг: улучшение дизайна существующего кода", "book10.jpg"),
            };

            var productNames = new string[]
            {
            "Паттерны проектирования",
            "Чистый код",
            "Совершенный код. Мастер-класс",
            "Алгоритмы: построение и анализ",
            "Фундаментальные основы хакерства",
            "Человеческий фактор: успешные проекты и команды",
            "Грокаем алгоритмы. Иллюстрированное пособие для программистов и любопытствующих",
            "Программист-фанатик",
            "Карьера программиста",
            "Рефакторинг: улучшение дизайна существующего кода"
            };



            for (int i = 0; i < count; i++)
            {
                var name = productImages[i].name;
                var price = random.Next(50, 2000);
                var stock = Math.Round(random.NextDouble() * 100);

                var product = new Product(name, price);
                product.Id = i == 0 ? Guid.Empty : Guid.NewGuid();
                product.Img = productImages[i].img;
                product.Description = name;
                product.Stock = stock;
                products.TryAdd(product.Id, product);
            }

            return products;
        }

    }

    struct ProductsImages
    {
        public string name;
        public string img;
        public ProductsImages(string name, string img)
        {
            this.name = name;
            this.img = img;
        }

    };
}

