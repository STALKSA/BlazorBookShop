namespace OnlineShop.HttpModals.Requests
{
    public record AddCartItemRequest(Guid AccountId, Guid ProductId, int Quantity);
}
