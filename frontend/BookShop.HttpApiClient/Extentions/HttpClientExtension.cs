using OnlineShop.HttpModals.Responses;
using System.Net.Http.Json;
using System.Net;
using BookShop.HttpApiClient.Exceptions;

namespace BookShop.HttpApiClient.Extentions
{
    public static class HttpClientExtension
    {
        public static async Task<TResponse> PostAsJsonAnsDeserializeAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        TRequest request,
        string uri,
        CancellationToken cancellationToken = default)
        {

            using var response = await httpClient.PostAsJsonAsync(uri, request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        {
                            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
                            throw new MyBookShopApiException(error!);
                        }
                    case HttpStatusCode.BadRequest:
                        {
                            var details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
                            throw new MyBookShopApiException((int)response.StatusCode, details!);
                        }
                    default:
                        throw new MyBookShopApiException($"Неизвестная ошибка {response.StatusCode}");
                }
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
            return loginResponse!;
        }
    }
}
