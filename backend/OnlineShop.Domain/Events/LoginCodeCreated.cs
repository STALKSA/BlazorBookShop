using MediatR;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Events
{
    public class LoginCodeCreated : INotification
    {
        public Account Account { get; }
        public ConfirmationCode Code { get; }

        public LoginCodeCreated(Account account, ConfirmationCode code)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }
    }
}
