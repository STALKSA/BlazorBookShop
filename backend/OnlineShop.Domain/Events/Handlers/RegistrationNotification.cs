using MediatR;
using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Events.Handlers
{
    public class RegistrationNotification : INotificationHandler<AccountRegistered>
    {
        //private readonly IEmailSender _emailSender;
        private readonly ILogger<RegistrationNotification> _logger;

        public RegistrationNotification(
       IEmailSender emailSender,
       ILogger<RegistrationNotification> _logger)
        {
            //_emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this._logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task Handle(AccountRegistered notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Отправка email оповещения о успешной регистрации");
            //await _emailSender.SendEmailAsync(
            //    notification.Account.Email,
            //    "Сообщение о процессе регистрации",
            //    "Вы зарегистрированы",
            //    cancellationToken);
        }
    }
}
