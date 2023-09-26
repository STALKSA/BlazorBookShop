using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineShop.WebApi.Filters
{
    public class AuthentificationFilter : Attribute, IAuthorizationFilter
    {
        private readonly ILogger<AuthentificationFilter> _logger;
        public AuthentificationFilter(ILogger<AuthentificationFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Headers["Api-Key"].ToString();

            if (string.IsNullOrEmpty(token) || token != "superToken")
            {
                _logger.LogInformation("Не верный api ключ авторизации");
                context.Result =
                    new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }

            _logger.LogInformation("Успешное подтверждение api ключа");
        }
    }
}
