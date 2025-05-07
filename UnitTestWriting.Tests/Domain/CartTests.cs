using UnitTestWriting.Domain;
using Xunit;
using NUnit;
using NUnit.Framework;
using System.Xml.Linq;
using System.ComponentModel;


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
               
        //[TestCase(0,0,2,1)]
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
        [TestCase(20, 5, 25, 2, 01)]
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
        [TestCase(90, 6, 96)]
        [TestCase(90, 5, 100)]
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

        /// <summary>
        /// 1. Корзина пуста, скидка = 10. Результат - 0.
        /// </summary>
        [Test]
        public void GetFullPrice_EmptyCart_Return0()
        {
            // Arrange
            var cart = new Cart(new User()
            {   
            Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });

            // Act
            var fullPrice = cart.GetFullPrice(DateTime.Now);

        // Assert
        Xunit.Assert.Equal(0, fullPrice);
        }
        
        /// <summary>
        /// 2. В корзина: 2шт товар1 за 5р, 1шт товар2 за 11р, 1шт товар3 за 0р, скидка = 0. Результат - 21.
        /// </summary> 
        public void GetFullPrice_3ProductsNoDiscount_Return21()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });

            // Act            
            cart.AddProduct(new Product() { Id= Guid.NewGuid() , Name = "Product1", Price = 5 }, 2);
            cart.AddProduct(new Product() { Id= Guid.NewGuid(), Name = "Product2", Price = 11 }, 1);
            cart.AddProduct(new Product() { Id= Guid.NewGuid(), Name = "Product3", Price = 0 }, 1);

            var fullPrice = cart.GetFullPrice(DateTime.Now);

            // Assert
            Xunit.Assert.Equal(fullPrice, 21);
        }

        [Test]
        /// <summary>
        /// 3. В корзина: 2шт товар1 за 5р, 1шт товар2 за 11р, скидка = 10. Результат - 18,9.
        /// </summary>
        public void GetFullPrice_2ProductsDiscount10_Return18()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            // Act           
            cart.AddProduct(new Product() { Id = Guid.NewGuid(), Name = "Product1", Price = 5 }, 2);
            cart.AddProduct(new Product() { Id = Guid.NewGuid(), Name = "Product2", Price = 11 }, 1);
            cart.Discount = 10;
                       
            var fullPrice = cart.GetFullPrice(DateTime.Now);

            // Assert
            Xunit.Assert.Equal(fullPrice, 18);
        }

        [Test]
        /// <summary>
        /// 4. В корзина: 3шт товар1 за 12р, скидка = 0. Результат - 36.*/
        /// </summary>
        public void GetFullPrice_ProductNoDiscount_Return36()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });

            // Act            
            cart.AddProduct(new Product() { Id = Guid.NewGuid(), Name = "Product1", Price = 12 }, 3);
                        
            var fullPrice = cart.GetFullPrice(DateTime.Now);

            // Assert
            Xunit.Assert.Equal(fullPrice, 36);
        }

        /// <summary>
        /// 1. Добавлен 0/-1 шт товар1.Результат - исключение ArgumentOutOfRangeException.
        /// </summary>
        [TestCase(0)]
        [TestCase(-1)]
        public void AddProduct_AddXProduct_ThrowArgumentOutOfRangeException(int amount)
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            //var amount = 0;

            // Act            
            var actualException = Xunit.Assert.Throws<ArgumentOutOfRangeException>(() => 
                cart.AddProduct(new Product() { Id = Guid.NewGuid(), Name = "Product1", Price = 12 }, amount));
            
            var expectedException = new ArgumentOutOfRangeException(nameof(amount));
            

            // Assert
            Xunit.Assert.Equal(actualException!.Message, expectedException.Message);
        }
                        
        [Test]
        public void AddProduct_AddSameProduct_ReturnAmountProduct()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });

            var product = new Product() { Id = Guid.NewGuid(), Name = "Product1", Price = 12 };
            // Act            
            cart.AddProduct(product, 3);
            cart.AddProduct(product, 5);

            // Assert
            (Product Product, int Amount)[] products = cart.Products;

            var cnt = products.Sum(x =>  x.Amount);            
            
            Xunit.Assert.Equal(cnt, 8);
        }
        [Test]
        public void AddProduct_AddDiferendProducts_ReturnAmountProducts()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });

            var product1 = new Product() { Id = Guid.NewGuid(), Name = "Product1", Price = 12 };
            var product2 = new Product() { Id = Guid.NewGuid(), Name = "Product2", Price = 120 };
            // Act            
            cart.AddProduct(product1, 3);
            cart.AddProduct(product2, 5);

            // Assert
            (Product Product, int Amount)[] products = cart.Products;

            var cnt1 = products.First(x => x.Product.Id == product1.Id ).Amount;
            var cnt2 = products.First(x => x.Product.Id == product2.Id).Amount;

            Xunit.Assert.Equal(cnt1, 3);
            Xunit.Assert.Equal(cnt2, 5);
        }

        [Test]
        public void ApplyDiscount_ApplyDiscountTwice_ThrowException()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false                
            });
            cart.ApplyDiscount(10);

            // Act            
            var actualException = Xunit.Assert.Throws<Exception> (() =>
               cart.ApplyDiscount(15));
            var expectedException = new Exception("Скидка уже применена");

            // Assert
            Xunit.Assert.Equal(actualException.Message, expectedException.Message);
        }

        [Test]
        public void ApplyDiscount_TooMuchDiscount_ThrowArgumentException()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = true
            });
            
            cart.ApplyPromo(new PromoCode(90, "code", TimeSpan.FromDays(5)));
            //-------------------------------
            // Act            
            var actualException = Xunit.Assert.Throws<ArgumentException>(() =>
               cart.ApplyDiscount(15));
            var expectedException = new ArgumentException("Общая скидка не может быть больше 100%");

            // Assert
            Xunit.Assert.Equal(actualException.Message, expectedException.Message);
        }

        /// <summary>
        /// 2. Скидка = 105%. Результат - исключение.        
        /// 4. Скидка = 100%. Результат - исключение.
        /// 5. Скидка = 0%. Результат - исключение.         
        /// </summary>
        [TestCase(105)]
        [TestCase(100)]       
        [TestCase(0)]
        [TestCase(-10)]
        public void ApplyDiscount_Discount_ThrowException(int discount)
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });            
            cart.ApplyPromo(new PromoCode(5, "code", TimeSpan.FromDays(5)));

            // Act            
            var actualException = Xunit.Assert.Throws<ArgumentOutOfRangeException>(() =>
               cart.ApplyDiscount(discount));
            var expectedException = new ArgumentOutOfRangeException(nameof(discount));

            // Assert
            Xunit.Assert.Equal(actualException.Message , expectedException.Message);
        }
        /// <summary>
        /// 3. Скидка = 10, купон = 20. Результат - 30.
        /// 7. Скидка = 70%, купон = 0. Результат - 70. 
        /// </summary>
        [TestCase(10, 20, 30)]
        [TestCase(70, 0, 70)]
        [TestCase(99, 0, 99)]
        [TestCase(1, 0, 1)]
        public void ApplyDiscount_DiscountCoupon_ReturnSum(int discount, int coupon, int expectedDiscount )
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            if (coupon != 0)
                cart.ApplyPromo(new PromoCode(coupon, "code", TimeSpan.FromDays(5)));

            // Act            
            cart.ApplyDiscount(discount);
            
            // Assert
            Xunit.Assert.Equal(cart.GetFullDiscount(DateTime.Now) , expectedDiscount);
        }
        
        [Test]
        public void ApplyPromo_ApplyPromoTwice_ThrowException()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            var newPromo = new PromoCode(5, "code", TimeSpan.FromDays(5));
            cart.ApplyPromo(newPromo);

            // Act            
            var actualException = Xunit.Assert.Throws<Exception>(() =>
               cart.ApplyPromo(newPromo));
            var expectedException = new Exception("Промокод уже применён");

            // Assert
            Xunit.Assert.Equal(actualException.Message, expectedException.Message);
        }

        /// <summary>
        /// 2. Скидка = 10, купон = 20. Результат - купон на 20.
        /// </summary>        
        [Test]        
        public void ApplyPromo_DiscountCoupon_ReturnSum()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            cart.ApplyPromo(new PromoCode(10, "code", TimeSpan.FromDays(5)));

            // Act            
            cart.ApplyDiscount(15);

            // Assert
            Xunit.Assert.Equal(cart.PromoCode.Discount , 10);
        }
        
        /// <summary>
        /// 3. Скидка = 70%, купон = 30. Результат - исключение. 
        /// </summary>
        [Test]
        public void ApplyPromo_MaxDiscountCoupon_ThrowArgumentException()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
            
            cart.ApplyDiscount(70);
            // Act                        
            var actualException = Xunit.Assert.Throws<ArgumentException>(() =>
               cart.ApplyPromo(new PromoCode(30, "code", TimeSpan.FromDays(5))));
            var expectedException = new ArgumentException("Общая скидка не может быть больше 100%");

            // Assert
            Xunit.Assert.Equal(actualException.Message, expectedException.Message);
        }
        /// <summary>
        ///Купон для обычного покупателя.Результат - исключение.
        /// </summary>
        [Test]
        public void ApplyPromo_PremiumCouponNoPremiumCustomer_ThrowException()
        {
            // Arrange
            var cart = new Cart(new User()
            {
                Name = "Person",
                BirthDate = new DateTime(2025, 01, 01),
                Premium = false
            });
                        
            // Act                        
            var actualException = Xunit.Assert.Throws<Exception>(() =>
               cart.ApplyPromo(new PromoCode(51, "code", TimeSpan.FromDays(5))));
            var expectedException = new Exception("Промокод только для пользователей премиальных аккаунтов");

            // Assert
            Xunit.Assert.Equal(actualException.Message, expectedException.Message);
        }
        // 115, 55 не покрыты
    }
}
