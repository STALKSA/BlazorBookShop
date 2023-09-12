using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
 

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));

        }

        public virtual async Task AddProduct(Guid accountId, Guid productId, int quantity, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.GetCartByAccountId(accountId, cancellationToken);

            var existedItem = cart.Items!.FirstOrDefault(item => item.ProductId == productId);
            if (existedItem is null)
            {
                cart.Items!.Add(new CartItem(Guid.Empty, productId, quantity));
            }
            else
            {
                existedItem.Quantity += quantity;
            }

            await _cartRepository.Update(cart, cancellationToken);
        }

        public virtual Task<Cart> GetAccountCart(Guid accountId, CancellationToken cancellationToken)
        {
            return _cartRepository.GetCartByAccountId(accountId, cancellationToken);
        }
    }
}
