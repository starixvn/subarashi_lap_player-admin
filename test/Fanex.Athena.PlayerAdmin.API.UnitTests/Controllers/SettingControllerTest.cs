namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    using API.Controllers;
    using API.DTO;
    using API.Services;
    using API.ViewModels;
    using Core.Constants;
    using Core.Models;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class SettingControllerTest
    {
        private readonly SettingController _controller;
        private readonly Mock<IContextService> _mockContextService;

        public SettingControllerTest()
        {
            _mockContextService = new Mock<IContextService>();
            _controller = new SettingController(_mockContextService.Object);
        }

        [Fact]
        public void Constructor_WithValidContextService_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void GetSiteSetting_ReturnsJsonResultWithSiteSettingViewModel()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.UserLevel).Returns(5);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.IsShowSabaIntelligence).Returns(false);
            _mockContextService.Setup(x => x.IsSearchWithMatchPreciseOnly).Returns(true);
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(true);

            // Act
            var result = _controller.GetSiteSetting();

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var viewModel = Assert.IsType<SiteSettingViewModel>(jsonResult.Value);


            Assert.True(viewModel.IsAllowedCustomerComm);
            Assert.Equal(5, viewModel.UserLevel);
            Assert.True(viewModel.IsEnableSingleWallet);
            Assert.False(viewModel.IsShowSabaIntelligence);
            Assert.True(viewModel.IsSearchWithMatchPreciseOnly);
            Assert.True(viewModel.IsShowMobile);
        }

        [Fact]
        public void GetSiteSetting_WithDifferentValues_ReturnsCorrectSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            _mockContextService.Setup(x => x.UserLevel).Returns(1);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(false);
            _mockContextService.Setup(x => x.IsShowSabaIntelligence).Returns(true);
            _mockContextService.Setup(x => x.IsSearchWithMatchPreciseOnly).Returns(false);
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(false);

            // Act
            var result = _controller.GetSiteSetting();

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var viewModel = Assert.IsType<SiteSettingViewModel>(jsonResult.Value);


            Assert.False(viewModel.IsAllowedCustomerComm);
            Assert.Equal(1, viewModel.UserLevel);
            Assert.False(viewModel.IsEnableSingleWallet);
            Assert.True(viewModel.IsShowSabaIntelligence);
            Assert.False(viewModel.IsSearchWithMatchPreciseOnly);
            Assert.False(viewModel.IsShowMobile);
        }

        [Fact]
        public void GetPlayerInfoSetting_ReturnsJsonResultWithPlayerInfoSettingViewModel()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(true);
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BankInfo)).Returns(false);
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.ActivityRecord)).Returns(true);

            // Act
            var result = _controller.GetPlayerInfoSetting();

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var viewModel = Assert.IsType<PlayerInfoSettingViewModel>(jsonResult.Value);


            Assert.True(viewModel.IsShowPlayerProfile);
            Assert.False(viewModel.IsShowBankInformation);
            Assert.True(viewModel.IsShowActivityRecord);
        }

        [Fact]
        public void GetPlayerInfoSetting_WithAllFalseRoles_ReturnsAllFalseSettings()
        {
            // Arrange
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(false);
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BankInfo)).Returns(false);
            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.ActivityRecord)).Returns(false);

            // Act
            var result = _controller.GetPlayerInfoSetting();

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            var viewModel = Assert.IsType<PlayerInfoSettingViewModel>(jsonResult.Value);


            Assert.False(viewModel.IsShowPlayerProfile);
            Assert.False(viewModel.IsShowBankInformation);
            Assert.False(viewModel.IsShowActivityRecord);
        }

        [Fact]
        public async Task GetDepositSetting_ReturnsOkWithDepositSettingViewModel()
        {
            // Arrange
            var mockDepositSettingService = new Mock<IDepositSettingService>();
            var expectedCommGroups = new List<CommGroupViewModel>
            {
                new() { Id = "1", Name = "Standard" },
                new() { Id = "2", Name = "Premium" }
            };
            var expectedRiskLevels = new List<RiskLevel>
            {
                new() { Key = "LOW", Name = "Low Risk" },
                new() { Key = "HIGH", Name = "High Risk" }
            };
            var expectedOddsGroups = new List<string> { "A", "B", "C" };

            _mockContextService.Setup(x => x.LicenseeId).Returns(123);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(expectedOddsGroups);


            mockDepositSettingService.Setup(x => x.GetCommGroupList(123))
                .ReturnsAsync(expectedCommGroups);
            mockDepositSettingService.Setup(x => x.GetRiskLevelList("en"))
                .ReturnsAsync(expectedRiskLevels);

            // Act
            var result = await _controller.GetDepositSetting(mockDepositSettingService.Object);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var viewModel = Assert.IsType<DepositSettingViewModel>(okResult.Value);


            Assert.Equal(expectedCommGroups, viewModel.CommGroups);
            Assert.Equal(expectedRiskLevels, viewModel.RiskLevels);
            Assert.Equal(expectedOddsGroups, viewModel.OddsGroups);
        }

        [Fact]
        public async Task GetDepositSetting_WithEmptyLists_ReturnsEmptyCollections()
        {
            // Arrange
            var mockDepositSettingService = new Mock<IDepositSettingService>();
            var emptyCommGroups = new List<CommGroupViewModel>();
            var emptyRiskLevels = new List<RiskLevel>();
            var emptyOddsGroups = new List<string>();

            _mockContextService.Setup(x => x.LicenseeId).Returns(456);
            _mockContextService.Setup(x => x.Language).Returns("zh");
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(emptyOddsGroups);


            mockDepositSettingService.Setup(x => x.GetCommGroupList(456))
                .ReturnsAsync(emptyCommGroups);
            mockDepositSettingService.Setup(x => x.GetRiskLevelList("zh"))
                .ReturnsAsync(emptyRiskLevels);

            // Act
            var result = await _controller.GetDepositSetting(mockDepositSettingService.Object);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var viewModel = Assert.IsType<DepositSettingViewModel>(okResult.Value);


            Assert.Empty(viewModel.CommGroups);
            Assert.Empty(viewModel.RiskLevels);
            Assert.Empty(viewModel.OddsGroups);
        }

        [Fact]
        public async Task GetSubsidiarySetting_WithSubsidiaries_ReturnsOkWithSubsidiaryData()
        {
            // Arrange
            var mockSubsidiaryService = new Mock<ISubsidiaryService>();
            var expectedSubsidiaries = new List<SelectTreeItem>
            {
                new() { Value = "1", Name = "Subsidiary1" },
                new() { Value = "2", Name = "Subsidiary2" }
            };

            _mockContextService.Setup(x => x.SubsidiaryViewType).Returns(SubsidiaryViewType.NormalManagement);
            _mockContextService.Setup(x => x.SiteName).Returns("TestSite");
            _mockContextService.Setup(x => x.AgentList).Returns(new Dictionary<int, int[]> { { 1, new[] { 1, 2 } } });
            _mockContextService.Setup(x => x.WhiteLabelID).Returns(123);


            mockSubsidiaryService.Setup(x =>
                    x.GetSubsidiaryList("TestSite", It.IsAny<IReadOnlyDictionary<int, int[]>>(), 123, SubsidiaryViewType.NormalManagement))
                .ReturnsAsync(expectedSubsidiaries);

            // Act
            var result = await _controller.GetSubsidiarySetting(mockSubsidiaryService.Object);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);

            // The result is an anonymous object, so we need to use reflection
            var resultValue = okResult.Value;
            var subsidiaryViewTypeProperty = resultValue.GetType().GetProperty("SubsidiaryViewType");
            var subsidiaryListProperty = resultValue.GetType().GetProperty("SubsidiaryList");

            Assert.NotNull(subsidiaryViewTypeProperty);
            Assert.NotNull(subsidiaryListProperty);
            Assert.Equal(SubsidiaryViewType.NormalManagement, subsidiaryViewTypeProperty.GetValue(resultValue));
            Assert.Equal(expectedSubsidiaries, subsidiaryListProperty.GetValue(resultValue));
        }

        [Fact]
        public async Task GetSubsidiarySetting_WithNoSubsidiaries_ReturnsNotManagementViewType()
        {
            // Arrange
            var mockSubsidiaryService = new Mock<ISubsidiaryService>();
            var emptySubsidiaries = new List<SelectTreeItem>();

            _mockContextService.Setup(x => x.SubsidiaryViewType).Returns(SubsidiaryViewType.NormalManagement);
            _mockContextService.Setup(x => x.SiteName).Returns("TestSite");
            _mockContextService.Setup(x => x.AgentList).Returns(new Dictionary<int, int[]> { { 1, new[] { 1, 2 } } });
            _mockContextService.Setup(x => x.WhiteLabelID).Returns(123);


            mockSubsidiaryService.Setup(x =>
                    x.GetSubsidiaryList("TestSite", It.IsAny<IReadOnlyDictionary<int, int[]>>(), 123, SubsidiaryViewType.NormalManagement))
                .ReturnsAsync(emptySubsidiaries);

            // Act
            var result = await _controller.GetSubsidiarySetting(mockSubsidiaryService.Object);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);


            // The result is an anonymous object, so we need to use reflection
            var resultValue = okResult.Value;
            var subsidiaryViewTypeProperty = resultValue.GetType().GetProperty("SubsidiaryViewType");
            var subsidiaryListProperty = resultValue.GetType().GetProperty("SubsidiaryList");

            Assert.NotNull(subsidiaryViewTypeProperty);
            Assert.NotNull(subsidiaryListProperty);
            Assert.Equal(SubsidiaryViewType.NotManagement, subsidiaryViewTypeProperty.GetValue(resultValue));
            Assert.Equal(emptySubsidiaries, subsidiaryListProperty.GetValue(resultValue));
        }
    }
}