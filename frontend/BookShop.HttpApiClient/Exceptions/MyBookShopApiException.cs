using OnlineShop.HttpModals.Responses;
using System.Net;

namespace BookShop.HttpApiClient.Exceptions
{
    public class MyBookShopApiException : Exception
    {
        public ErrorResponse? Error { get; }
        public ValidationProblemDetails? Details { get; }
        public int? StatusCode { get; }

        public MyBookShopApiException()
        {
        }

        public MyBookShopApiException(int statusCode, ValidationProblemDetails details) : base(details.Title)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public MyBookShopApiException(ErrorResponse error) : base(error.Message)
        {
            Error = error;
            StatusCode = error.StatusCode;
        }

        public MyBookShopApiException(int statusCode, string message)
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
