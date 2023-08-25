using OnlineShop.Domain.Entities;

namespace OnlineShop.WebApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(Account account);
    }
}
