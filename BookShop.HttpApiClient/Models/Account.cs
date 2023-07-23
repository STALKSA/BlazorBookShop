namespace BookShop.HttpApiClient.Models;

public class Account
{
    public Guid Id { get; init; }

    public string? Name { get; set; }
    public string? Email { get; set; }

    public string? Password { get; set; }
}
