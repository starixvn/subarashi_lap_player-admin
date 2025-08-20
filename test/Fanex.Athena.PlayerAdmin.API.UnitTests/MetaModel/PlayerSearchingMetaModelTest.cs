namespace Fanex.Athena.PlayerAdmin.API.UnitTests.MetaModel
{
    using API.MetaModel;
    using Core.Models;

    public class PlayerSearchingMetaModelTest
    {
        [Fact]
        public void PlayerSearchingMetaModel_DefaultConstructor_CreatesInstance()
        {
            // Act
            var metaModel = new PlayerSearchingMetaModel();

            // Assert
            Assert.NotNull(metaModel);
            Assert.Equal(0, metaModel.SearchOption);
            Assert.Null(metaModel.UserName);
            Assert.Null(metaModel.OddsGroup);
            Assert.Null(metaModel.SpecialLevel);
            Assert.Equal(0, metaModel.BaFilter);
            Assert.Null(metaModel.SubsidiaryList);
            Assert.False(metaModel.SubsidiarySearch);
            Assert.Equal(0, metaModel.PageSize);
            Assert.Equal(0, metaModel.PageIndex);
        }

        [Fact]
        public void PlayerSearchingMetaModel_SetProperties_StoresValuesCorrectly()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act
            metaModel.SearchOption = 1;
            metaModel.UserName = "testuser";
            metaModel.OddsGroup = "A";
            metaModel.SpecialLevel = "5";
            metaModel.BaFilter = 2;
            metaModel.SubsidiaryList = "Sub1,Sub2";
            metaModel.SubsidiarySearch = true;
            metaModel.PageSize = 20;
            metaModel.PageIndex = 1;

            // Assert
            Assert.Equal(1, metaModel.SearchOption);
            Assert.Equal("testuser", metaModel.UserName);
            Assert.Equal("A", metaModel.OddsGroup);
            Assert.Equal("5", metaModel.SpecialLevel);
            Assert.Equal(2, metaModel.BaFilter);
            Assert.Equal("Sub1,Sub2", metaModel.SubsidiaryList);
            Assert.True(metaModel.SubsidiarySearch);
            Assert.Equal(20, metaModel.PageSize);
            Assert.Equal(1, metaModel.PageIndex);
        }

        [Fact]
        public void PlayerSearchingMetaModel_InheritsFromPlayerListBaseMetaModel()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act & Assert
            Assert.IsAssignableFrom<PlayerListBaseMetaModel>(metaModel);
        }

        [Fact]
        public void PlayerSearchingMetaModel_ImplementsISubsidiarySearchMetaModel()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act & Assert
            Assert.IsAssignableFrom<ISubsidiarySearchMetaModel>(metaModel);
        }

        [Fact]
        public void PlayerSearchingMetaModel_InheritsBaseProperties()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act
            metaModel.SiteName = "TestSite";
            metaModel.CurrencyIdList = "1,2,3";
            metaModel.AgentListXml = "<agents></agents>";
            metaModel.CustomerClass = "VIP";
            metaModel.CommGroups = "Standard,Premium";
            metaModel.IsDeposited = 1;

            // Assert
            Assert.Equal("TestSite", metaModel.SiteName);
            Assert.Equal("1,2,3", metaModel.CurrencyIdList);
            Assert.Equal("<agents></agents>", metaModel.AgentListXml);
            Assert.Equal("VIP", metaModel.CustomerClass);
            Assert.Equal("Standard,Premium", metaModel.CommGroups);
            Assert.Equal(1, metaModel.IsDeposited);
        }

        [Fact]
        public void PlayerSearchingMetaModel_WithMaxByteValues_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act
            metaModel.SearchOption = byte.MaxValue;
            metaModel.BaFilter = byte.MaxValue;

            // Assert
            Assert.Equal(byte.MaxValue, metaModel.SearchOption);
            Assert.Equal(byte.MaxValue, metaModel.BaFilter);
        }

        [Fact]
        public void PlayerSearchingMetaModel_WithLargePagingValues_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act
            metaModel.PageSize = 1000;
            metaModel.PageIndex = 999;

            // Assert
            Assert.Equal(1000, metaModel.PageSize);
            Assert.Equal(999, metaModel.PageIndex);
        }

        [Fact]
        public void PlayerSearchingMetaModel_WithEmptyStrings_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act
            metaModel.UserName = "";
            metaModel.OddsGroup = "";
            metaModel.SpecialLevel = "";
            metaModel.SubsidiaryList = "";

            // Assert
            Assert.Equal("", metaModel.UserName);
            Assert.Equal("", metaModel.OddsGroup);
            Assert.Equal("", metaModel.SpecialLevel);
            Assert.Equal("", metaModel.SubsidiaryList);
        }

        [Fact]
        public void PlayerSearchingMetaModel_WithSpecialCharacters_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act
            metaModel.UserName = "user@test.com";
            metaModel.OddsGroup = "A+";
            metaModel.SpecialLevel = "5-10";
            metaModel.SubsidiaryList = "Sub-1,Sub_2,Sub@3";

            // Assert
            Assert.Equal("user@test.com", metaModel.UserName);
            Assert.Equal("A+", metaModel.OddsGroup);
            Assert.Equal("5-10", metaModel.SpecialLevel);
            Assert.Equal("Sub-1,Sub_2,Sub@3", metaModel.SubsidiaryList);
        }

        [Fact]
        public void PlayerSearchingMetaModel_WithNegativePagingValues_StoresCorrectly()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel();

            // Act
            metaModel.PageSize = -1;
            metaModel.PageIndex = -1;

            // Assert
            Assert.Equal(-1, metaModel.PageSize);
            Assert.Equal(-1, metaModel.PageIndex);
        }
    }
}