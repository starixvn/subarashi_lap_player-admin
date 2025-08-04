namespace Fanex.Athena.PlayerAdmin.API.UnitTests.ViewModels
{
    using API.ViewModels;
    using Xunit;

    public class SiteSettingViewModelTest
    {
        [Fact]
        public void SiteSettingViewModel_DefaultConstructor_CreatesInstance()
        {
            // Act
            var viewModel = new SiteSettingViewModel();

            // Assert
            Assert.NotNull(viewModel);
            Assert.Equal(0, viewModel.UserLevel);
            Assert.False(viewModel.IsAllowedCustomerComm);
            Assert.False(viewModel.IsEnableSingleWallet);
            Assert.False(viewModel.IsShowSabaIntelligence);
            Assert.False(viewModel.IsSearchWithMatchPreciseOnly);
            Assert.False(viewModel.IsShowMobile);
        }

        [Fact]
        public void SiteSettingViewModel_SetProperties_StoresValuesCorrectly()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = 5;
            viewModel.IsAllowedCustomerComm = true;
            viewModel.IsEnableSingleWallet = true;
            viewModel.IsShowSabaIntelligence = false;
            viewModel.IsSearchWithMatchPreciseOnly = true;
            viewModel.IsShowMobile = true;

            // Assert
            Assert.Equal(5, viewModel.UserLevel);
            Assert.True(viewModel.IsAllowedCustomerComm);
            Assert.True(viewModel.IsEnableSingleWallet);
            Assert.False(viewModel.IsShowSabaIntelligence);
            Assert.True(viewModel.IsSearchWithMatchPreciseOnly);
            Assert.True(viewModel.IsShowMobile);
        }

        [Fact]
        public void SiteSettingViewModel_WithAllTrueValues_StoresCorrectly()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = 10;
            viewModel.IsAllowedCustomerComm = true;
            viewModel.IsEnableSingleWallet = true;
            viewModel.IsShowSabaIntelligence = true;
            viewModel.IsSearchWithMatchPreciseOnly = true;
            viewModel.IsShowMobile = true;

            // Assert
            Assert.Equal(10, viewModel.UserLevel);
            Assert.True(viewModel.IsAllowedCustomerComm);
            Assert.True(viewModel.IsEnableSingleWallet);
            Assert.True(viewModel.IsShowSabaIntelligence);
            Assert.True(viewModel.IsSearchWithMatchPreciseOnly);
            Assert.True(viewModel.IsShowMobile);
        }

        [Fact]
        public void SiteSettingViewModel_WithAllFalseValues_StoresCorrectly()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = 1;
            viewModel.IsAllowedCustomerComm = false;
            viewModel.IsEnableSingleWallet = false;
            viewModel.IsShowSabaIntelligence = false;
            viewModel.IsSearchWithMatchPreciseOnly = false;
            viewModel.IsShowMobile = false;

            // Assert
            Assert.Equal(1, viewModel.UserLevel);
            Assert.False(viewModel.IsAllowedCustomerComm);
            Assert.False(viewModel.IsEnableSingleWallet);
            Assert.False(viewModel.IsShowSabaIntelligence);
            Assert.False(viewModel.IsSearchWithMatchPreciseOnly);
            Assert.False(viewModel.IsShowMobile);
        }

        [Fact]
        public void SiteSettingViewModel_WithNegativeUserLevel_StoresCorrectly()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = -1;

            // Assert
            Assert.Equal(-1, viewModel.UserLevel);
        }

        [Fact]
        public void SiteSettingViewModel_WithMaxUserLevel_StoresCorrectly()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = int.MaxValue;

            // Assert
            Assert.Equal(int.MaxValue, viewModel.UserLevel);
        }

        [Fact]
        public void SiteSettingViewModel_WithZeroUserLevel_StoresCorrectly()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = 0;

            // Assert
            Assert.Equal(0, viewModel.UserLevel);
        }

        [Fact]
        public void SiteSettingViewModel_WithMixedBooleanValues_StoresCorrectly()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = 7;
            viewModel.IsAllowedCustomerComm = true;
            viewModel.IsEnableSingleWallet = false;
            viewModel.IsShowSabaIntelligence = true;
            viewModel.IsSearchWithMatchPreciseOnly = false;
            viewModel.IsShowMobile = true;

            // Assert
            Assert.Equal(7, viewModel.UserLevel);
            Assert.True(viewModel.IsAllowedCustomerComm);
            Assert.False(viewModel.IsEnableSingleWallet);
            Assert.True(viewModel.IsShowSabaIntelligence);
            Assert.False(viewModel.IsSearchWithMatchPreciseOnly);
            Assert.True(viewModel.IsShowMobile);
        }

        [Fact]
        public void SiteSettingViewModel_MultipleAssignments_StoresLatestValues()
        {
            // Arrange
            var viewModel = new SiteSettingViewModel();

            // Act
            viewModel.UserLevel = 1;
            viewModel.UserLevel = 5;
            viewModel.UserLevel = 10;

            viewModel.IsAllowedCustomerComm = false;
            viewModel.IsAllowedCustomerComm = true;

            // Assert
            Assert.Equal(10, viewModel.UserLevel);
            Assert.True(viewModel.IsAllowedCustomerComm);
        }
    }
}