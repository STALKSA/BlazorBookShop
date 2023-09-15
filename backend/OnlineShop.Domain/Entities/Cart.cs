namespace OnlineShop.Domain.Entities
{
    public class Cart : IEntity
    {
        public Cart(Guid id, Guid accountId)
        {
            Id = id;
            AccountId = accountId;
            Items = new List<CartItem>();
        }

     

        public Guid Id { get; init; }
        public Guid AccountId { get; set; }

        public List<CartItem>? Items { get; set; }


        public void AddItem(Guid productId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            if (Items == null) throw new InvalidOperationException("Items корзины - null");

            var existedItem = Items!.SingleOrDefault(item => item.ProductId == productId);
            if (existedItem is null)
            {
                Items.Add(new CartItem(Guid.Empty, productId, quantity));
            }
            else
            {
                existedItem.Quantity += quantity;
            }
        }

    }

    public record CartItem : IEntity
    {
        protected CartItem() { }

        public CartItem(Guid id, Guid productId, int quantity)
        {
            Id = id;
            ProductId = productId;
            Quantity = quantity;
        }

        public Guid Id { get; init; }

        public Guid ProductId { get; init; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; } = null!;
    }
}
