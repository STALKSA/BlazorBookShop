using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Domain.Services;
using OnlineShop.HttpModals.Requests;
using OnlineShop.HttpModals.Responses;
using OnlineShop.WebApi.Filters;
using System.Security.Claims;

namespace OnlineShop.WebApi.Controllers
{
    [GlobalExceptionFilter]
    [Route("cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;
        private readonly IRepository<Product> _repository;

        public CartController(CartService cartService, IRepository<Product> repository)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<CartResponse>> GetCurrentCart(CancellationToken cancellationToken)
        {
            try
            {
                var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var guid = Guid.Parse(strId!);
                var cart = await _cartService.GetAccountCart(guid, cancellationToken);
                var response = await MakeCartResponse(cart, _repository);
                return response;
            }
            catch (Exception)
            {
                return Conflict(new ErrorResponse("Корзина не найдена"));
            }
        }

        //    [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult> AddCartItem(
            [FromBody] AddCartItemRequest reqest,
            CancellationToken cancellationToken)
        {
            try
            {
                await _cartService.AddProduct(reqest.AccountId, reqest.ProductId, reqest.Quantity, cancellationToken);
                return Ok();
            }
            catch (CartNotFoundException)
            {
                return Conflict(new ErrorResponse("Корзина не найдена"));
            }
        }


        private async Task<CartResponse> MakeCartResponse(Cart cart, IRepository<Product> repository)
        {
            if (cart == null) throw new ArgumentNullException(nameof(cart));

            if (cart.Items.IsNullOrEmpty())
                return new CartResponse();

            var response = new CartResponse();
            foreach (var it in cart.Items!)
            {
                var product = await repository.GetById(it.ProductId,default);
                response.Items.Add(new ItemResponse(it.Id, product.Name, it.Quantity));
            }
            return response;
        }
    }
}
