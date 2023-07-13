namespace ShopBackend.Data
{
	public class Product
	{
		public Guid Id { get; init; }
		public string? Name { get; set; }
		public decimal Price { get; set; }
	}
}