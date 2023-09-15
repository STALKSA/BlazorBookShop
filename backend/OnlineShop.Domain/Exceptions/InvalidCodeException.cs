namespace OnlineShop.Domain.Exceptions
{
    public class InvalidCodeException : DomainException
    {
        public InvalidCodeException(string message) : base(message)
        {

        }
    }
}
