using MediatR;
using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Events.Handlers
{
    public class LoginNotificationHandler : INotificationHandler<LoginCodeCreated>
    {
        //private readonly IEmailSender _emailSender;
        private readonly ILogger<LoginNotificationHandler> _logger;

        public LoginNotificationHandler(
            IEmailSender emailSender,
            ILogger<LoginNotificationHandler> logger)
        {
            //_emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(LoginCodeCreated notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Код подтверждения отправлен на почту пользователя для проверки аутентификации: {CodeCode}", notification.Code.Code);
            //await _emailSender.SendEmailAsync(
            //    notification.Account.Email!,
            //    "Код подтверждения",
            //    notification.Code.Code,
            //    cancellationToken);
        }
    }
}
