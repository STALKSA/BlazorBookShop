namespace OnlineShop.Domain.Interfaces
{
    public interface IApplicationPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string hashedPassword, string providedPassword, out bool rehashNeeded);
    }
}
