namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class ProductNotFoundException : DomainException
    {
        public ProductNotFoundException()
        {
        }

        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}
