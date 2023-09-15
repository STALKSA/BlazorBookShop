using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Services
{
    public class CartService
    {
        private readonly IUnitOfWork _uow;


        public CartService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));

        }

        public virtual async Task AddProduct(Guid accountId, Guid productId, int quantity, CancellationToken cancellationToken)
        {
            var cart = await _uow.CartRepository.GetCartByAccountId(accountId, cancellationToken);
            cart.AddItem(productId, quantity);
            await _uow.CartRepository.Update(cart, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        public virtual Task<Cart> GetAccountCart(Guid accountId, CancellationToken cancellationToken)
        {
            return _uow.CartRepository.GetCartByAccountId(accountId, cancellationToken);
        }
    }
}
