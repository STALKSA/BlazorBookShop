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
            if(request is null) { throw new ArgumentNullException(nameof(request)); }
            try
            {
                await _accountService.Register(request.Name, request.Email, request.Password, cancellationToken);
                return Ok();
            }
            catch(EmailAlreadyExistsException ex)
            {
                return Conflict(new ErrorResponse("Пользователь с таким Email'ом уже зарегистрирован."));
            }
            catch (Exception ex)
            {
                return BadRequest("Argument null exception caught:\n" + ex.Message);
            }
          
        }


    }
}
