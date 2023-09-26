using System.Net;

namespace OnlineShop.HttpModals.Responses
{
    public record ErrorResponse(string Message, int? StatusCode = null);
}
