using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using OnlineShop.Domain.Interfaces;


namespace MailKitSmtpWmailSender
{
    internal class EmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly SmtpConfig _smtpConfig;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptionsSnapshot<SmtpConfig> snapshotOptionsAccessor, ILogger<EmailSender> logger)
        {
            ArgumentNullException.ThrowIfNull(snapshotOptionsAccessor);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _smtpConfig = snapshotOptionsAccessor.Value;
        }

        private readonly System.Net.Mail.SmtpClient _smtpClient = new();

        public async ValueTask DisposeAsync()
        {
            await _smtpClient.DisconnectAsync(true);
            _smtpClient.Dispose();
        }


        public async Task SendEmailAsync(string recipientEmail, string subject, string body, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(recipientEmail);
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(body);

            var emailMessage = new MimeMessage
            {
                Subject = subject,
                Body = new TextPart(TextFormat.Plain)
                {
                    Text = body,
                },
                From = { MailboxAddress.Parse(_smtpConfig.UserName) },
                To = { MailboxAddress.Parse(recipientEmail) },
            };

            _logger.LogInformation($"Email sent from with body: {body}");
            await EnsureConnectedAndAuthentificatedAsync();
            await _smtpClient.SendAsync(emailMessage, cancellationToken);

        }

        private async Task EnsureConnectedAndAuthentificatedAsync()
        {
            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false);
            }

            if (!_smtpClient.IsAuthenticated)
                await _smtpClient.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
        }
    }
}
