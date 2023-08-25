using Microsoft.IdentityModel.Tokens;
using OnlineShop.Domain.Entities;
using OnlineShop.WebApi.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineShop.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;

        public TokenService(JwtConfig jwtConfig)
        {
            if (jwtConfig == null) throw new ArgumentNullException(nameof(jwtConfig));
            _jwtConfig = jwtConfig;
        }

        public string GenerateToken(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.Add(_jwtConfig.LifeTime),
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_jwtConfig.SigningKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
