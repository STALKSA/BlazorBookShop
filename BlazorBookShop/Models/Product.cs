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
        public string? Img { get; set; }
	    public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ProducedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public double Stock { get; set; }
       

        public override string ToString()
        {
            return $"Id: {Id},\nName: {Name},\nDescription: {Description},\nPrice: {Price},\nProducedAt: {ProducedAt},\nExpiredAt: {ExpiredAt},\nStock: {Stock},\nImg: {Img}.";
        }
    }
}

