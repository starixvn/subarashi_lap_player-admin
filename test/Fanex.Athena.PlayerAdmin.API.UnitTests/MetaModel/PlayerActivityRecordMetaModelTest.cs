namespace Fanex.Athena.PlayerAdmin.API.UnitTests.MetaModel
{
    using API.MetaModel;
    using Core.Models;
    using Xunit;

    public class PlayerActivityRecordMetaModelTest
    {
        [Fact]
        public void PlayerActivityRecordMetaModel_DefaultConstructor_CreatesInstance()
        {
            // Act
            var metaModel = new PlayerActivityRecordMetaModel();

            // Assert
            Assert.NotNull(metaModel);
            Assert.Equal(0, metaModel.CustId);
            Assert.Equal(0, metaModel.CurrencyId);
            Assert.Null(metaModel.SiteName);
        }

        [Fact]
        public void PlayerActivityRecordMetaModel_SetProperties_StoresValuesCorrectly()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel();

            // Act
            metaModel.CustId = 123;
            metaModel.CurrencyId = 1;
            metaModel.SiteName = "TestSite";

            // Assert
            Assert.Equal(123, metaModel.CustId);
            Assert.Equal(1, metaModel.CurrencyId);
            Assert.Equal("TestSite", metaModel.SiteName);
        }

        [Fact]
        public void PlayerActivityRecordMetaModel_ImplementsISiteNameMetaModel()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel();

            // Act & Assert
            Assert.IsAssignableFrom<ISiteNameMetaModel>(metaModel);
        }

        [Fact]
        public void PlayerActivityRecordMetaModel_ImplementsICustomerIdentityMetaModel()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel();

            // Act & Assert
            Assert.IsAssignableFrom<ICustomerIdentityMetaModel>(metaModel);
        }

        [Fact]
        public void PlayerActivityRecordMetaModel_WithNegativeValues_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel();

            // Act
            metaModel.CustId = -1;
            metaModel.CurrencyId = -1;

            // Assert
            Assert.Equal(-1, metaModel.CustId);
            Assert.Equal(-1, metaModel.CurrencyId);
        }

        [Fact]
        public void PlayerActivityRecordMetaModel_WithMaxValues_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel();

            // Act
            metaModel.CustId = int.MaxValue;
            metaModel.CurrencyId = int.MaxValue;

            // Assert
            Assert.Equal(int.MaxValue, metaModel.CustId);
            Assert.Equal(int.MaxValue, metaModel.CurrencyId);
        }

        [Fact]
        public void PlayerActivityRecordMetaModel_WithEmptyStrings_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel();

            // Act
            metaModel.SiteName = "";

            // Assert
            Assert.Equal("", metaModel.SiteName);
        }

        [Fact]
        public void PlayerActivityRecordMetaModel_WithSpecialCharacters_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel();

            // Act
            metaModel.SiteName = "Test-Site_123@domain.com";

            // Assert
            Assert.Equal("Test-Site_123@domain.com", metaModel.SiteName);
        }
    }
}