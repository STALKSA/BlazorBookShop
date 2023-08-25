using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModals.Requests;
using OnlineShop.HttpModals.Responses;
using OnlineShop.WebApi.Services;
using System.Security.Claims;

namespace OnlineShop.WebApi.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(AccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
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
              var token = _tokenService.GenerateToken(account);
              return new LoginResponse(account.Id, account.Name, token);

            } catch(AccountNotFoundException)
            {
               return Conflict(new ErrorResponse("Аккаунт с таким Email не найден"));
            }
            catch(InvalidPasswordException)
            {
               return Conflict(new ErrorResponse("Неверный пароль"));
            }
            
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<AccountResponse>> GetCurrentAccount(CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guid = Guid.Parse(strId);
            var account = await _accountService.GetAccountById(guid, cancellationToken);
            return new AccountResponse(account.Id, account.Name, account.Email);
        }
    }

}
