namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.MetaModels
{
    using AutoFixture;
    using MetaModel;

    public class PlayerBetLimitUpdateMetaModelTests
    {
        private readonly IFixture _fixture;

        public PlayerBetLimitUpdateMetaModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenSettingIsNotNull()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<string>
            {
                Setting = "test setting"
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenSettingIsNull()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<string>
            {
                Setting = null
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenSettingIsDefault()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<int>
            {
                Setting = default
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenSettingIsEnumerableWithItems()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<string>>
            {
                Setting = new[] { "item1", "item2" }
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenSettingIsEmptyEnumerable()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<string>>
            {
                Setting = Enumerable.Empty<string>()
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenSettingIsNullEnumerable()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<string>>
            {
                Setting = null
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenSettingIsListWithItems()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<List<PlayerBetLimitMetaModel>>
            {
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    _fixture.Create<PlayerBetLimitMetaModel>()
                }
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenSettingIsEmptyList()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<List<string>>
            {
                Setting = new List<string>()
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(42)]
        [InlineData(-5)]
        public void IsValid_ShouldReturnTrue_WhenSettingIsNonDefaultInt(int value)
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<int>
            {
                Setting = value
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_ShouldReturnTrue_WhenSettingIsNonNullString(string value)
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<string>
            {
                Setting = value
            };

            // Act
            var result = metaModel.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenCreated()
        {
            // Arrange
            var metaModel = new PlayerBetLimitUpdateMetaModel<string>();
            const int custId = 123;
            const string siteName = "TestSite";
            const int userId = 456;
            const int currencyId = 789;
            const string ip = "192.168.1.1";
            const string language = "en";
            const int productId = 101;
            const int licenseeCustId = 202;
            const string setting = "test setting";

            // Act
            metaModel.CustId = custId;
            metaModel.SiteName = siteName;
            metaModel.UserId = userId;
            metaModel.CurrencyId = currencyId;
            metaModel.Ip = ip;
            metaModel.Language = language;
            metaModel.ProductId = productId;
            metaModel.LicenseeCustId = licenseeCustId;
            metaModel.Setting = setting;

            // Assert
            Assert.Equal(custId, metaModel.CustId);
            Assert.Equal(siteName, metaModel.SiteName);
            Assert.Equal(userId, metaModel.UserId);
            Assert.Equal(currencyId, metaModel.CurrencyId);
            Assert.Equal(ip, metaModel.Ip);
            Assert.Equal(language, metaModel.Language);
            Assert.Equal(productId, metaModel.ProductId);
            Assert.Equal(licenseeCustId, metaModel.LicenseeCustId);
            Assert.Equal(setting, metaModel.Setting);
        }
    }
}