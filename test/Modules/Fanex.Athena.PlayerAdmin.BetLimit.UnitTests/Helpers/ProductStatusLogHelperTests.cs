namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Helpers
{
    using AutoFixture;
    using BetLimit.Helpers;
    using DataDomains;
    using MetaModel;

    public class ProductStatusLogHelperTests
    {
        private readonly IFixture _fixture;

        public ProductStatusLogHelperTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void CreateLogEvent_ShouldReturnEvent_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            metaModel.ProductId = 123;
            metaModel.Status = true;
            metaModel.OldStatus = false;

            // Act
            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(metaModel.CustId, result.CustID);
            Assert.Equal(metaModel.UserId, result.UserId);
            Assert.Equal(metaModel.SiteName, result.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, result.LicenseeCustId);
            Assert.NotNull(result.Detail);
        }

        [Fact]
        public void CreateLogEvent_ShouldSetCorrectDetailProperties_WhenUpdateIsSuccessful()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            metaModel.ProductId = 456;
            metaModel.Status = true;
            metaModel.OldStatus = false;

            // Act
            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, true);

            // Assert
            Assert.NotNull(result.Detail);
            Assert.Equal(LogActionDetailId.UpdatingProductStatus, result.Detail.DetailedId);
            Assert.Equal("456", result.Detail.ProductId);
            Assert.Equal(ProductStatusLogHelper.Success, result.Detail.Status);
            Assert.Equal("Disable", result.Detail.Old);
            Assert.Equal("Enable", result.Detail.New);
        }

        [Fact]
        public void CreateLogEvent_ShouldSetCorrectDetailProperties_WhenUpdateFails()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            metaModel.ProductId = 789;
            metaModel.Status = false;
            metaModel.OldStatus = true;

            // Act
            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, false);

            // Assert
            Assert.NotNull(result.Detail);
            Assert.Equal(LogActionDetailId.UpdatingProductStatus, result.Detail.DetailedId);
            Assert.Equal("789", result.Detail.ProductId);
            Assert.Equal(ProductStatusLogHelper.False, result.Detail.Status);
            Assert.Equal("Enable", result.Detail.Old);
            Assert.Equal("Disable", result.Detail.New);
        }

        [Theory]
        [InlineData(true, false, "Enable", "Disable")]
        [InlineData(false, true, "Disable", "Enable")]
        [InlineData(true, true, "Enable", "Enable")]
        [InlineData(false, false, "Disable", "Disable")]
        public void CreateLogEvent_ShouldSetCorrectStatusValues_WhenDifferentStatusCombinations(
            bool newStatus,
            bool oldStatus,
            string expectedNew,
            string expectedOld)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            metaModel.Status = newStatus;
            metaModel.OldStatus = oldStatus;

            // Act
            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, true);

            // Assert
            Assert.Equal(expectedOld, result.Detail.Old);
            Assert.Equal(expectedNew, result.Detail.New);
        }

        [Theory]
        [InlineData(true, ProductStatusLogHelper.Success)]
        [InlineData(false, ProductStatusLogHelper.False)]
        public void CreateLogEvent_ShouldSetCorrectStatus_WhenDifferentSuccessValues(bool isSuccessful, string expectedStatus)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();

            // Act
            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, isSuccessful);

            // Assert
            Assert.Equal(expectedStatus, result.Detail.Status);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(456)]
        [InlineData(789)]
        public void CreateLogEvent_ShouldSetCorrectProductId_WhenDifferentProductIds(int productId)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            metaModel.ProductId = productId;

            // Act
            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, true);

            // Assert
            Assert.Equal(productId.ToString(), result.Detail.ProductId);
        }

        [Fact]
        public void CreateLogEvent_ShouldSetCorrectDetailedId_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();

            // Act
            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, true);

            // Assert
            Assert.Equal(LogActionDetailId.UpdatingProductStatus, result.Detail.DetailedId);
        }

        [Fact]
        public void CreateLogEvent_ShouldPreserveAllMetaModelProperties_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();

            // Act

            var result = ProductStatusLogHelper.CreateLogEvent(metaModel, true);

            // Assert
            Assert.Equal(metaModel.CustId, result.CustID);
            Assert.Equal(metaModel.UserId, result.UserId);
            Assert.Equal(metaModel.SiteName, result.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, result.LicenseeCustId);
        }
    }
}