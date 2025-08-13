namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Helpers
{
    using AutoFixture;
    using BetLimit.Helpers;
    using Core.Constants;
    using DataDomains;
    using MetaModel;

    public class ChargeCommLogHelperTests
    {
        private readonly IFixture _fixture;

        public ChargeCommLogHelperTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void CreateLogEvent_ShouldReturnEvent_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = 5.5;
            metaModel.OldComm = 3.0;
            const int productId = ProductTypes.Exchange;

            // Act
            var result = ChargeCommLogHelper.CreateLogEvent(metaModel, productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(metaModel.CustId, result.CustID);
            Assert.Equal(metaModel.UserId, result.UserId);
            Assert.Equal(metaModel.SiteName, result.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, result.LicenseeCustId);
            Assert.NotNull(result.Detail);
        }

        [Fact]
        public void CreateLogEvent_ShouldSetCorrectDetailProperties_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = 6.0;
            metaModel.OldComm = 4.5;
            const int productId = ProductTypes.Exchange;

            // Act
            var result = ChargeCommLogHelper.CreateLogEvent(metaModel, productId);

            // Assert
            Assert.NotNull(result.Detail);
            Assert.Equal(ChargeCommLogHelper.Success, result.Detail.Status);
            Assert.Equal(LogActionDetailId.UpdatingChargeComm, result.Detail.DetailedId);
            Assert.Equal(productId.ToString(), result.Detail.ProductId);
            Assert.Equal("4.5%", result.Detail.Old);
            Assert.Equal("6%", result.Detail.New);
        }

        [Theory]
        [InlineData(2.5, 5.0, "2.5%", "5%")]
        [InlineData(0.0, 3.5, "0%", "3.5%")]
        [InlineData(7.0, 7.0, "7%", "7%")]
        public void CreateLogEvent_ShouldFormatCommissionCorrectly_WhenDifferentValues(double oldComm, double newComm, string expectedOld, string expectedNew)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = newComm;
            metaModel.OldComm = oldComm;
            const int productId = ProductTypes.Exchange;

            // Act
            var result = ChargeCommLogHelper.CreateLogEvent(metaModel, productId);

            // Assert
            Assert.Equal(expectedOld, result.Detail.Old);
            Assert.Equal(expectedNew, result.Detail.New);
        }

        [Theory]
        [InlineData(ProductTypes.Exchange)]
        [InlineData(ProductTypes.Sportsbook)]
        [InlineData(ProductTypes.Lottery)]
        public void CreateLogEvent_ShouldSetCorrectProductId_WhenDifferentProductIds(int productId)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();

            // Act
            var result = ChargeCommLogHelper.CreateLogEvent(metaModel, productId);

            // Assert
            Assert.Equal(productId.ToString(), result.Detail.ProductId);
        }

        [Fact]
        public void CreateLogEvent_ShouldSetStatusToSuccess_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            const int productId = ProductTypes.Exchange;

            // Act
            var result = ChargeCommLogHelper.CreateLogEvent(metaModel, productId);

            // Assert
            Assert.Equal(ChargeCommLogHelper.Success, result.Detail.Status);
        }

        [Fact]
        public void CreateLogEvent_ShouldSetCorrectDetailedId_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            const int productId = ProductTypes.Exchange;

            // Act
            var result = ChargeCommLogHelper.CreateLogEvent(metaModel, productId);

            // Assert
            Assert.Equal(LogActionDetailId.UpdatingChargeComm, result.Detail.DetailedId);
        }
    }
}