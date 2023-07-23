using Microsoft.AspNetCore.Mvc;
using ShopBackend.Data;
using ShopBackend.Data.Repositories;

namespace ShopBackend.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("register")]
        public async Task<IActionResult> Register(
            Account account, 
            IAccountRepository accountRepository, 
            CancellationToken cancellationToken)
        {

            ArgumentNullException.ThrowIfNull(accountRepository);
            await accountRepository.Add(account, cancellationToken);
            return Ok();
        }

    }
}
