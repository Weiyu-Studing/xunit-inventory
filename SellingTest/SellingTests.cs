using InventorySystem;
using Xunit;

namespace SellingTest
{
    public class SellingTests
    {
        //-----------------------------------------------------------Happy Path------------------------------------------------------------------
        private readonly InventoryOrderService _service = new();
        // test 1 : buy 1 item
        [Fact]
        public void Buy1Item()
        {
            // Arrange
            _service.AddProduct(new Product { Id = "1", Name = "Iphone ProMax 18", UnitPrice = 2399m, StockQuantity = 1000 });

            // Act
            var res = _service.ProcessOrder("1", 1, 0);

            // Assert
            Assert.True(res.IsSuccess);
            Assert.Equal(999, _service.GetProduct("1")!.StockQuantity);
        }

        // test 2 : buy 10 item see if 10% discount works. 
        ///***Bug is here, I fixed it at InventorySystemService.cs line 61 ***
        [Fact]
        public void Buy10ItemTryIfHasTenDiscount()
        {
            // Arrange
            _service.AddProduct(new Product { Id = "2", Name = "Apple", UnitPrice = 100m, StockQuantity = 10000 });

            // Act
            var res = _service.ProcessOrder("2", 10, 0);

            // Assert
            Assert.True(res.IsSuccess);
            Assert.Equal(900m, res.TotalCost);
        }

        // test 3 : buy 66 item see if 20% discount works.
        [Fact]
        public void Buy66ItemTryIfHasTwentyDiscount()
        {
            // Arrange
            _service.AddProduct(new Product { Id = "3", Name = "The mystery Gift", UnitPrice = 100m, StockQuantity = 999 });

            // Act
            var res = _service.ProcessOrder("3", 66, 0);

            // Assert
            Assert.True(res.IsSuccess);
            Assert.Equal(5280m, res.TotalCost);
        }
        //-----------------------------------------------------------Edge------------------------------------------------------------------
        // test 4: buy all stock items see if works
        [Fact]
        public void BuyAllItem()
        {
            // Arrange
            _service.AddProduct(new Product { Id = "4", Name = "Banana", UnitPrice = 2m, StockQuantity = 66 });

            // Act
            var res = _service.ProcessOrder("4", 66, 0);

            // Assert
            Assert.True(res.IsSuccess);
            Assert.Equal(0, _service.GetProduct("4")!.StockQuantity);
        }

        // text 5: buy overload items see if give correct "Insufficient stock."
        [Fact]
        public void BuyOverloadItemSeeIfBlock()
        {
            // Arrange
            _service.AddProduct(new Product { Id = "5", Name = "Robot", UnitPrice = 39999m, StockQuantity = 1 });

            // Act
            var res = _service.ProcessOrder("5", 2, 0);

            // Assert
            Assert.False(res.IsSuccess);
            Assert.Equal("Insufficient stock.", res.Message);
            Assert.Equal(1, _service.GetProduct("5")!.StockQuantity);
        }

        // text 6: buy Incorrect Id items see if give correct "Product not found."
        [Fact]
        public void BuyIncorrectIdItem()
        {
            // Arrange
            _service.AddProduct(new Product { Id = "6", Name = "Rabbit", UnitPrice = 20m, StockQuantity = 8 });

            // Act
            var res = _service.ProcessOrder("233", 2, 0);

            // Assert
            Assert.False(res.IsSuccess);
            Assert.Equal("Product not found.", res.Message);
        }
        //-----------------------------------------------------------Exception------------------------------------------------------------------

        // text 7: add items pass null see if give "Invalid product details."
        [Fact]
        public void AddNullItemIfThrowException()
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() => _service.AddProduct(null!));

            // Assert
            Assert.Equal("Invalid product details.", exception.Message);
        }

        // text 8: add items with no Id see if give "Invalid product details."
        [Fact]
        public void AddNoIdItem()
        {
            // Arrange
            Product emptyIdProduct = new Product { Id = "", Name = "Air", UnitPrice = 0m, StockQuantity = 99999 };

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _service.AddProduct(emptyIdProduct));

            // Assert
            Assert.Equal("Invalid product details.", exception.Message);
        }

        // text 9: buy 0 quantity item see if works, expected not works and pass .False(res.IsSuccess).
        //***bug 2 test, change InventorySystemService.cs line 49 to quantity <= 0 ***//
        [Fact]
        public void BuyZeroItem_ReturnFail()
        {
            // Arrange
            _service.AddProduct(new Product { Id = "9", Name = "Tiger(Toy)", UnitPrice = 5m, StockQuantity = 100 });

            // Act
            var res = _service.ProcessOrder("9", 0, 0);

            // Assert
            Assert.False(res.IsSuccess);
            Assert.Equal("Quantity must be positive.", res.Message);
        }

    }
}
