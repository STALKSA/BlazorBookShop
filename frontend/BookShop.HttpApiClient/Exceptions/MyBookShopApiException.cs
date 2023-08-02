using OnlineShop.HttpModals.Responses;
using System.Net;

namespace BookShop.HttpApiClient.Exceptions
{
    public class MyBookShopApiException : Exception
    {
        public ErrorResponse? Error { get; }
        public ValidationProblemDetails? Details { get; }
        public HttpStatusCode? StatusCode { get; }

        public MyBookShopApiException()
        {
        }

        public MyBookShopApiException(HttpStatusCode statusCode, ValidationProblemDetails details) : base(details.Title)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public MyBookShopApiException(ErrorResponse error) : base(error.Message)
        {
            Error = error;
            StatusCode = error.StatusCode;
        }

        public MyBookShopApiException(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
        }

        public MyBookShopApiException(string? message) : base(message)
        {
        }

        public MyBookShopApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
