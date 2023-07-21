namespace ShopBackend.Data
{
    public class Account : IEntity
    {
        public Guid Id { get; init; }

        public string? Name { get; init; }
        public string? Email { get; init; }

    }
}
