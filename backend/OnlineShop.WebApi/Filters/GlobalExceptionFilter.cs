using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShop.Domain.Exceptions;
using OnlineShop.HttpModals.Responses;

namespace OnlineShop.WebApi.Filters
{
    public class GlobalExceptionFilter : Attribute, IExceptionFilter
    {


        public GlobalExceptionFilter()
        {

        }

        public void OnException(ExceptionContext context)
        {


            var message = TryGetUserMessageFromException(context);
            var statusCode = StatusCodes.Status409Conflict;

            if (message != null)
            {
                context.Result = new ObjectResult(new ErrorResponse(message, statusCode))
                {
                    StatusCode = statusCode
                };
                context.ExceptionHandled = true;
            }
        }

        private string? TryGetUserMessageFromException(ExceptionContext context)
        {
            return context.Exception switch
            {
                EmailAlreadyExistsException => "Аккаунт с таким логином уже зарегистрирован",
                AccountNotFoundException => "Аккаунт с таким логином не найден!",
                InvalidPasswordException => "Неверный пароль!",
                ProductNotFoundException => "Продукт с данным id не найден ",
                CartNotFoundException => "Данная корзина не найден",
                DomainException => "Неизвестная ошибка",
                _ => null
            };
        }
    }
}
