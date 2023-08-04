using Microsoft.AspNetCore.Identity;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.IdentityPasswordHasherLib
{
    public class IdentityPasswordHasher : IApplicationPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        private readonly object _fake = new();

        public string HashPassword(string password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            return _passwordHasher.HashPassword(_fake, password);
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword, out bool rehashNeeded)
        {
            if (hashedPassword is null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            if (providedPassword is null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }

            var result = _passwordHasher.VerifyHashedPassword(_fake, hashedPassword, providedPassword);
            rehashNeeded = result == PasswordVerificationResult.SuccessRehashNeeded;
            return result != PasswordVerificationResult.Failed;
        }

    }
}