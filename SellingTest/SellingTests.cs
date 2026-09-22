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
    }
}
