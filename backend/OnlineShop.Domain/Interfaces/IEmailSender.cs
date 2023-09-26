namespace OnlineShop.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipientEmail, string subject, string? body, CancellationToken cancellationToken);
    }
}
