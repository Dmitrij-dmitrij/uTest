using UnitTestWriting.Domain;
using Xunit;
using NUnit;
using NUnit.Framework;


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
            Xunit.Assert.True(true);
        }
        [Fact]
        public void Cart_15Discount()
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
            Xunit.Assert.Equal(cart.Discount, 15);
        }
        [Fact]
        public void Cart_NoPromo()
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
            Xunit.Assert.Null(cart.PromoCode);
        }
               
        [TestCase(0,0,2,1)]
        [TestCase(3,3,2,1)]
        public void GetFullDiscount_NoBirthNoCouponNoPremiumUser_ReturnDiscount(int discount,  int expectedResult, int purchaseMonth, int purchaseDay)
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",                
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            cart.Discount = discount;
            
            // Act
            var fullDiscount = cart.GetFullDiscount(new DateTime(2025, purchaseMonth, purchaseDay));

            // Assert
            Xunit.Assert.Equal(fullDiscount, expectedResult);
        }
        [TestCase(10,3,13,2,1)]
        [TestCase(0,3,3,2,1)]
        [TestCase(20, 5, 25, 1, 10)]
        public void GetFullDiscount_NoBirthNoPremiumUser_ReturnDiscount(int discount, int coupon, int expectedResult, int purchaseMonth, int purchaseDay)
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            cart.Discount = discount;
            cart.ApplyPromo(new PromoCode(coupon, "pCode", TimeSpan.FromDays(2)));

            // Act
            var fullDiscount = cart.GetFullDiscount(new DateTime(2025, purchaseMonth, purchaseDay));

            // Assert
            Xunit.Assert.Equal(fullDiscount, expectedResult);
        }
        
        [TestCase(4,6,15)]
        [TestCase(0, 15, 20)]        
        public void GetFullDiscount_BirthDayNoPremiumUser_ReturnDiscount(int discount, int coupon, int expectedResult)
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            cart.Discount = discount;
            cart.ApplyPromo(new PromoCode(coupon, "pCode", TimeSpan.FromDays(2)));

            // Act
            var fullDiscount = cart.GetFullDiscount(new DateTime(2025, 01, 01));

            // Assert
            Xunit.Assert.Equal(fullDiscount, expectedResult);
        }
        
        [Test]
        public void GetFullDiscount_BirthDayNoCouponNoDiscountNoPremiumUser_ReturnDiscount()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            
            // Act
            var fullDiscount = cart.GetFullDiscount(new DateTime(2025, 01, 01));

            // Assert
            Xunit.Assert.Equal(fullDiscount, 5);
        }

    }
}
