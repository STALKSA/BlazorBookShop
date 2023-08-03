using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModals.Requests;
using OnlineShop.HttpModals.Responses;

namespace OnlineShop.WebApi.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }


        [HttpGet("register")]
        public async Task<IActionResult> Register(
            RegisterRequest request,
            CancellationToken cancellationToken)
        {
            if (request is null) { throw new ArgumentNullException(nameof(request)); }
            try
            {
                await _accountService.Register(request.Name, request.Email, request.Password, cancellationToken);
            }
            catch (EmailAlreadyExistsException)
            {
                return Conflict(new ErrorResponse("Пользователь с таким Email уже зарегистрирован"));
            }

            return Ok();
        }



        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(
            LoginRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
              var account = await _accountService.Login(request.Email, request.Password, cancellationToken);
                return new LoginResponse(account.Id, account.Name);

            } catch(AccountNotFoundException)
            {
                return Conflict(new ErrorResponse("Аккаунт с таким Email не найден"));
            }
            catch(InvalidPasswordException)
            {
                return Conflict(new ErrorResponse("Неверный пароль"));
            }
            
        }

    }

}
