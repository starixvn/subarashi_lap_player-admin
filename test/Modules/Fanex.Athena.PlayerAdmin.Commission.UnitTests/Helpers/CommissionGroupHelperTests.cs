namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Helpers
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Commission.Helpers;
    using Core.Models;

    public class CommissionGroupHelperTests
    {
        private readonly Fixture _fixture;

        public CommissionGroupHelperTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
        }

        [Theory]
        [InlineData("SB", 0)]
        [InlineData("Parlay", 1)]
        [InlineData("NG", 2)]
        [InlineData("CS", 3)]
        [InlineData("UnknownProduct", int.MaxValue)]
        public void ProductListComparer_ShouldReturnCorrectIndex_WhenProductCodeProvided(string productCode, int expectedIndex)
        {
            // Act
            var result = CommissionGroupHelper.ProductListComparer(productCode);

            // Assert
            Assert.Equal(expectedIndex, result);
        }

        [Fact]
        public void GetProductPermissions_ShouldReturnProductAccess_WhenProductCodeExists()
        {
            // Arrange
            var productAccess = new ProductAccess(new List<Product> { new() { ProductCode = "SB" } });
            var productCode = "SB";

            // Act
            var result = CommissionGroupHelper.GetProductPermissions(productAccess, productCode);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetProductPermissions_ShouldUseFollowPermissions_WhenProductCodeIsParlay()
        {
            // Arrange
            var productAccess = new ProductAccess(new List<Product> { new() { ProductCode = "SB" } });
            var productCode = "Parlay";

            // Act
            var result = CommissionGroupHelper.GetProductPermissions(productAccess, productCode);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0.1234, 1, 12)]
        [InlineData(0.1234, 2, 3)]
        [InlineData(0.1234, 3, 4)]
        [InlineData(0.0567, 1, 5)]
        [InlineData(0.0567, 2, 6)]
        [InlineData(0.0567, 3, 7)]
        public void GetCommPart_ShouldReturnCorrectPart_WhenValidPositionProvided(decimal comm, int position, int expectedResult)
        {
            // Act
            var result = CommissionGroupHelper.GetCommPart(comm, position);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(4, 0)]
        [InlineData(-1, 0)]
        public void GetCommPart_ShouldReturnZero_WhenInvalidPositionProvided(int position, int expectedResult)
        {
            // Arrange
            var comm = 0.1234m;

            // Act
            var result = CommissionGroupHelper.GetCommPart(comm, position);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0.0000, 1, 0)]
        [InlineData(0.0000, 2, 0)]
        [InlineData(0.0000, 3, 0)]
        public void GetCommPart_ShouldReturnZero_WhenCommissionIsZero(decimal comm, int position, int expectedResult)
        {
            // Act
            var result = CommissionGroupHelper.GetCommPart(comm, position);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0.1500, 1, 15)]
        [InlineData(0.1500, 2, 0)]
        [InlineData(0.1500, 3, 0)]
        public void GetCommPart_ShouldHandleMaxValue_WhenCommissionIsMaximum(decimal comm, int position, int expectedResult)
        {
            // Act
            var result = CommissionGroupHelper.GetCommPart(comm, position);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}