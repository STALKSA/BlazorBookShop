using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModals.Requests;
using ShopBackend.Data;
using ShopBackend.Data.Repositories;

namespace ShopBackend.Controllers
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

            await _accountService.Register(request.Name, request.Email, request.Password, cancellationToken);
            return Ok();
        }

   
    }
}
