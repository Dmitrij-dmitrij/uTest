using UnitTestWriting.Domain;
using Xunit;

namespace UnitTestWriting.Tests.Domain
{
    public class CartTests
    {
        [Fact]
        public void Test()
        {
            // Arrange

            // Act

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void Cart_15Discount_OK()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Test",
                UpdatedAt = DateTime.Now,
            });
            // Act
            cart.Discount = 15;

            // Assert
            Assert.Equal(cart.Discount,15);
        }
        [Fact]
        public void Cart_NoPromo_OK()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Test",
                UpdatedAt = DateTime.Now                
            });
            // Act
            cart.Discount = 51;

            // Assert
            Assert.Null (cart.PromoCode);
        }

    }
}
