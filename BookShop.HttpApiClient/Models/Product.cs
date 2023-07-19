namespace BlazorBookShop.Models
{
    public class Product
    {
        public Product(string name, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($" {nameof(name)} не может быть пустым или состоять из пробелов");
            }

            if (price <= 0)
            {
                throw new ArgumentException($" {nameof(price)} не может быть меньше или равно 0");
            }

            Name = name;
            Price = price;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Img { get; set; } = "";
        public decimal Price { get; set; }
        public double? Stock { get; set; } = 0;


        public override string ToString()
        {
            return $"Id: {Id},\nName: {Name},\nPrice: {Price},\nStock: {Stock},\nImg: {Img}.";
        }
    }
}

