using OnlineShop.Domain.Entities;

namespace BookShop.Domain.Test
{
    public class CartTests
    {
        [Fact]
        public void Item_is_added_to_cart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();
            var quantity = 1;

            // Act
            cart.AddItem(productId, quantity);

            // Assert
            Assert.Single(cart.Items!);

            var cartItem = cart.Items!.FirstOrDefault(it => it.ProductId == productId);
            Assert.NotNull(cartItem);

            Assert.Equal(quantity, cartItem.Quantity);
        }

        [Fact]
        public void Adding_existing_item_to_cart_increases_its_quantity()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            // Act
            cart.AddItem(productId, 1);
            cart.AddItem(productId, 1);

            // Assert
            Assert.Single(cart.Items!);
            Assert.Equal(2, cart.Items!.First().Quantity);
        }


        [Fact]
        public void Multiple_products_are_correctly_added_to_cart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();

            // Act
            cart.AddItem(productId1, 2);
            cart.AddItem(productId2, 3);

            // Assert
            Assert.Equal(2, cart.Items!.Count);
        }

        [Fact]
        public void Adding_negative_quantity_does_not_change_cart()
        {
            // Arrange
            var cart = new Cart(Guid.NewGuid(), Guid.NewGuid());
            var productId = Guid.NewGuid();

            // Act
            cart.AddItem(productId, -1);

            // Assert
            Assert.Empty(cart.Items!);
        }
    }
}