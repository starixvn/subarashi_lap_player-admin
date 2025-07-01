using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Helpers
{
    public class ProductHelperTests
    {
        [Theory]
        [InlineData("NG", "Number Game")]
        [InlineData("SB", "Sportsbook")]
        [InlineData("VS", "Virtual Sports")]
        [InlineData("VS180", "Virtual Soccer")]
        public void GetProductName_ExistingProductCode_ReturnsProductName(string productCode, string expected)
        {
            // Act
            var result = ProductHelper.GetProductName(productCode);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetProductName_NonExistingProductCodeWithDefaultValue_ReturnsDefaultValue()
        {
            // Arrange
            var productCode = "NonExistentCode";
            var defaultValue = "DefaultName";

            // Act
            var result = ProductHelper.GetProductName(productCode, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void GetProductName_NonExistingProductCodeWithoutDefaultValue_ReturnsProductCode()
        {
            // Arrange
            var productCode = "NonExistentCode";

            // Act
            var result = ProductHelper.GetProductName(productCode);

            // Assert
            Assert.Equal(productCode, result);
        }
    }
} 