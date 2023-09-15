namespace OnlineShop.Domain.Entities
{
    public class ConfirmationCode : IEntity
    {
        protected ConfirmationCode() { }

        public Guid Id { get; init; }
        public Guid AccountId { get; set; }
        public string Code { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }

        public ConfirmationCode(Guid id, Guid accountId, DateTimeOffset createdAt, DateTimeOffset expiresAt)
        {
            Id = id;
            AccountId = accountId;
            Code = GenerateCode();
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
        }

        private string GenerateCode()
        {
            return Random.Shared.Next(100_000, 999_999).ToString();
            return Guid.NewGuid().ToString().Replace("-", "")[..6];
        }
    }
}
