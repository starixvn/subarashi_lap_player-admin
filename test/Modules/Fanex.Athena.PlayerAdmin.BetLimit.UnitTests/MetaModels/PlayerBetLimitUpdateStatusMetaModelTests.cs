namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.MetaModels
{
    using AutoFixture;
    using MetaModel;

    public class PlayerBetLimitUpdateStatusMetaModelTests
    {
        private readonly IFixture _fixture;

        public PlayerBetLimitUpdateStatusMetaModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IsValid_ShouldAlwaysReturnTrue_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsValid_ShouldAlwaysReturnTrue_WhenDifferentStatusValues(bool status, bool oldStatus)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            metaModel.Status = status;
            metaModel.OldStatus = oldStatus;

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenCreated()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel();
            const int custId = 123;
            const string siteName = "TestSite";
            const int userId = 456;
            const string username = "testuser";
            const int currencyId = 789;
            const string ip = "192.168.1.1";
            const string language = "en";
            const int productId = 101;
            const int licenseeCustId = 202;
            const bool status = true;
            const bool oldStatus = false;

            // Act
            metaModel.CustId = custId;
            metaModel.SiteName = siteName;
            metaModel.UserId = userId;
            metaModel.Username = username;
            metaModel.CurrencyId = currencyId;
            metaModel.Ip = ip;
            metaModel.Language = language;
            metaModel.ProductId = productId;
            metaModel.LicenseeCustId = licenseeCustId;
            metaModel.Status = status;
            metaModel.OldStatus = oldStatus;

            // Assert
            Assert.Equal(custId, metaModel.CustId);
            Assert.Equal(siteName, metaModel.SiteName);
            Assert.Equal(userId, metaModel.UserId);
            Assert.Equal(username, metaModel.Username);
            Assert.Equal(currencyId, metaModel.CurrencyId);
            Assert.Equal(ip, metaModel.Ip);
            Assert.Equal(language, metaModel.Language);
            Assert.Equal(productId, metaModel.ProductId);
            Assert.Equal(licenseeCustId, metaModel.LicenseeCustId);
            Assert.Equal(status, metaModel.Status);
            Assert.Equal(oldStatus, metaModel.OldStatus);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues_WhenCreated()
        {
            // Act
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel();

            // Assert
            Assert.Equal(0, metaModel.CustId);
            Assert.Null(metaModel.SiteName);
            Assert.Equal(0, metaModel.UserId);
            Assert.Null(metaModel.Username);
            Assert.Equal(0, metaModel.CurrencyId);
            Assert.Null(metaModel.Ip);
            Assert.Null(metaModel.Language);
            Assert.Equal(0, metaModel.ProductId);
            Assert.Equal(0, metaModel.LicenseeCustId);
            Assert.False(metaModel.Status);
            Assert.False(metaModel.OldStatus);
            Assert.True(metaModel.IsValid);
        }
    }
}