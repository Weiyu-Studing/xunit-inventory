using InventorySystem;
using Xunit;

namespace SellingTest
{
    public class SellingTests
    {
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

        
    }
}
