using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class SettingControllerTests
    {
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Mock<IDepositSettingService> _depositSettingServiceMock;
        private readonly Mock<ISubsidiaryService> _subsidiaryServiceMock;
        private readonly SettingController _controller;

        public SettingControllerTests()
        {
            _contextServiceMock = new Mock<IContextService>();
            _depositSettingServiceMock = new Mock<IDepositSettingService>();
            _subsidiaryServiceMock = new Mock<ISubsidiaryService>();

            _controller = new SettingController(_contextServiceMock.Object);
        }

        [Fact]
        public void GetSiteSetting_ReturnsCorrectSiteSettingViewModel()
        {
            // Arrange
            _contextServiceMock.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _contextServiceMock.Setup(x => x.UserLevel).Returns(1);
            _contextServiceMock.Setup(x => x.IsEnableSingleWallet).Returns(false);
            _contextServiceMock.Setup(x => x.IsShowSabaIntelligence).Returns(true);
            _contextServiceMock.Setup(x => x.IsSearchWithMatchPreciseOnly).Returns(false);
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(true);

            // Act
            var result = _controller.GetSiteSetting();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var siteSetting = Assert.IsType<SiteSettingViewModel>(jsonResult.Value);
            
            Assert.True(siteSetting.IsAllowedCustomerComm);
            Assert.Equal(1, siteSetting.UserLevel);
            Assert.False(siteSetting.IsEnableSingleWallet);
            Assert.True(siteSetting.IsShowSabaIntelligence);
            Assert.False(siteSetting.IsSearchWithMatchPreciseOnly);
            Assert.True(siteSetting.IsShowMobile);
        }

        [Fact]
        public void GetPlayerInfoSetting_ReturnsCorrectPlayerInfoSettingViewModel()
        {
            // Arrange
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(true);
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BankInfo)).Returns(false);
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.ActivityRecord)).Returns(true);

            // Act
            var result = _controller.GetPlayerInfoSetting();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var playerInfoSetting = Assert.IsType<PlayerInfoSettingViewModel>(jsonResult.Value);
            
            Assert.True(playerInfoSetting.IsShowPlayerProfile);
            Assert.False(playerInfoSetting.IsShowBankInformation);
            Assert.True(playerInfoSetting.IsShowActivityRecord);
        }

        [Fact]
        public async Task GetDepositSetting_ReturnsCorrectDepositSettingViewModel()
        {
            // Arrange
            var expectedRiskLevels = new List<RiskLevel>
            {
                new RiskLevel { Id = "1", Name = "Low" },
                new RiskLevel { Id = "2", Name = "Medium" }
            };

            var expectedCommGroups = new List<CommGroupViewModel>
            {
                new CommGroupViewModel { Id = "1", Name = "Group A" },
                new CommGroupViewModel { Id = "2", Name = "Group B" }
            };

            var expectedOddsGroups = new[] { "A", "B", "C" };

            _contextServiceMock.Setup(x => x.LicenseeId).Returns(123);
            _contextServiceMock.Setup(x => x.Language).Returns("en-US");
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(expectedOddsGroups);

            _depositSettingServiceMock.Setup(x => x.GetCommGroupList(123))
                .ReturnsAsync(expectedCommGroups);
            _depositSettingServiceMock.Setup(x => x.GetRiskLevelList("en-US"))
                .ReturnsAsync(expectedRiskLevels);

            // Act
            var result = await _controller.GetDepositSetting(_depositSettingServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var depositSetting = Assert.IsType<DepositSettingViewModel>(okResult.Value);
            
            Assert.Equal(expectedRiskLevels, depositSetting.RiskLevels);
            Assert.Equal(expectedCommGroups, depositSetting.CommGroups);
            Assert.Equal(expectedOddsGroups, depositSetting.OddsGroups);
        }

        [Fact]
        public async Task GetSubsidiarySetting_WithEmptySubsidiaryList_ReturnsNotManagement()
        {
            // Arrange
            var emptySubsidiaryList = Enumerable.Empty<SelectTreeItem>();

            _contextServiceMock.Setup(x => x.SubsidiaryViewType).Returns(SubsidiaryViewType.NormalManagement);
            _contextServiceMock.Setup(x => x.SiteName).Returns("test-site");
            _contextServiceMock.Setup(x => x.AgentList).Returns(new Dictionary<int, int[]>());
            _contextServiceMock.Setup(x => x.WhiteLabelID).Returns(1);

            _subsidiaryServiceMock.Setup(x => x.GetSubsidiaryList("test-site", It.IsAny<IReadOnlyDictionary<int, int[]>>(), 1, SubsidiaryViewType.NormalManagement))
                .ReturnsAsync(emptySubsidiaryList);

            // Act
            var result = await _controller.GetSubsidiarySetting(_subsidiaryServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            // Use reflection to access anonymous object properties
            var subsidiaryViewTypeProperty = response.GetType().GetProperty("SubsidiaryViewType");
            var subsidiaryListProperty = response.GetType().GetProperty("SubsidiaryList");
            
            Assert.NotNull(subsidiaryViewTypeProperty);
            Assert.NotNull(subsidiaryListProperty);
            
            var subsidiaryViewType = subsidiaryViewTypeProperty.GetValue(response);
            var subsidiaryList = subsidiaryListProperty.GetValue(response);
            
            Assert.Equal(SubsidiaryViewType.NotManagement, subsidiaryViewType);
            Assert.Equal(emptySubsidiaryList, subsidiaryList);
        }

        [Fact]
        public async Task GetSubsidiarySetting_WithSubsidiaryList_ReturnsCorrectViewType()
        {
            // Arrange
            var subsidiaryList = new List<SelectTreeItem>
            {
                new SelectTreeItem { Value = "sub1", Name = "Subsidiary 1" },
                new SelectTreeItem { Value = "sub2", Name = "Subsidiary 2" }
            };

            _contextServiceMock.Setup(x => x.SubsidiaryViewType).Returns(SubsidiaryViewType.MasterManagement);
            _contextServiceMock.Setup(x => x.SiteName).Returns("test-site");
            _contextServiceMock.Setup(x => x.AgentList).Returns(new Dictionary<int, int[]>());
            _contextServiceMock.Setup(x => x.WhiteLabelID).Returns(1);

            _subsidiaryServiceMock.Setup(x => x.GetSubsidiaryList("test-site", It.IsAny<IReadOnlyDictionary<int, int[]>>(), 1, SubsidiaryViewType.MasterManagement))
                .ReturnsAsync(subsidiaryList);

            // Act
            var result = await _controller.GetSubsidiarySetting(_subsidiaryServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            // Use reflection to access anonymous object properties
            var subsidiaryViewTypeProperty = response.GetType().GetProperty("SubsidiaryViewType");
            var subsidiaryListProperty = response.GetType().GetProperty("SubsidiaryList");
            
            Assert.NotNull(subsidiaryViewTypeProperty);
            Assert.NotNull(subsidiaryListProperty);
            
            var subsidiaryViewType = subsidiaryViewTypeProperty.GetValue(response);
            var actualSubsidiaryList = subsidiaryListProperty.GetValue(response);
            
            Assert.Equal(SubsidiaryViewType.MasterManagement, subsidiaryViewType);
            Assert.Equal(subsidiaryList, actualSubsidiaryList);
        }

        [Fact]
        public void GetSiteSetting_WithAllFalseSettings_ReturnsCorrectValues()
        {
            // Arrange
            _contextServiceMock.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            _contextServiceMock.Setup(x => x.UserLevel).Returns(0);
            _contextServiceMock.Setup(x => x.IsEnableSingleWallet).Returns(false);
            _contextServiceMock.Setup(x => x.IsShowSabaIntelligence).Returns(false);
            _contextServiceMock.Setup(x => x.IsSearchWithMatchPreciseOnly).Returns(false);
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(false);

            // Act
            var result = _controller.GetSiteSetting();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var siteSetting = Assert.IsType<SiteSettingViewModel>(jsonResult.Value);
            
            Assert.False(siteSetting.IsAllowedCustomerComm);
            Assert.Equal(0, siteSetting.UserLevel);
            Assert.False(siteSetting.IsEnableSingleWallet);
            Assert.False(siteSetting.IsShowSabaIntelligence);
            Assert.False(siteSetting.IsSearchWithMatchPreciseOnly);
            Assert.False(siteSetting.IsShowMobile);
        }

        [Fact]
        public void GetPlayerInfoSetting_WithAllFalsePermissions_ReturnsCorrectValues()
        {
            // Arrange
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.PlayerProfile)).Returns(false);
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BankInfo)).Returns(false);
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.ActivityRecord)).Returns(false);

            // Act
            var result = _controller.GetPlayerInfoSetting();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var playerInfoSetting = Assert.IsType<PlayerInfoSettingViewModel>(jsonResult.Value);
            
            Assert.False(playerInfoSetting.IsShowPlayerProfile);
            Assert.False(playerInfoSetting.IsShowBankInformation);
            Assert.False(playerInfoSetting.IsShowActivityRecord);
        }

        [Fact]
        public async Task GetDepositSetting_WithEmptyData_ReturnsEmptyCollections()
        {
            // Arrange
            var emptyRiskLevels = Enumerable.Empty<RiskLevel>();
            var emptyCommGroups = Enumerable.Empty<CommGroupViewModel>();
            var emptyOddsGroups = Enumerable.Empty<string>();

            _contextServiceMock.Setup(x => x.LicenseeId).Returns(123);
            _contextServiceMock.Setup(x => x.Language).Returns("en-US");
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(emptyOddsGroups);

            _depositSettingServiceMock.Setup(x => x.GetCommGroupList(123))
                .ReturnsAsync(emptyCommGroups);
            _depositSettingServiceMock.Setup(x => x.GetRiskLevelList("en-US"))
                .ReturnsAsync(emptyRiskLevels);

            // Act
            var result = await _controller.GetDepositSetting(_depositSettingServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var depositSetting = Assert.IsType<DepositSettingViewModel>(okResult.Value);
            
            Assert.Empty(depositSetting.RiskLevels);
            Assert.Empty(depositSetting.CommGroups);
            Assert.Empty(depositSetting.OddsGroups);
        }

        [Fact]
        public async Task GetSubsidiarySetting_WithNullWhiteLabelID_HandlesCorrectly()
        {
            // Arrange
            var subsidiaryList = new List<SelectTreeItem>
            {
                new SelectTreeItem { Value = "sub1", Name = "Subsidiary 1" }
            };

            _contextServiceMock.Setup(x => x.SubsidiaryViewType).Returns(SubsidiaryViewType.NormalManagement);
            _contextServiceMock.Setup(x => x.SiteName).Returns("test-site");
            _contextServiceMock.Setup(x => x.AgentList).Returns(new Dictionary<int, int[]>());
            _contextServiceMock.Setup(x => x.WhiteLabelID).Returns((int?)null);

            _subsidiaryServiceMock.Setup(x => x.GetSubsidiaryList("test-site", It.IsAny<IReadOnlyDictionary<int, int[]>>(), null, SubsidiaryViewType.NormalManagement))
                .ReturnsAsync(subsidiaryList);

            // Act
            var result = await _controller.GetSubsidiarySetting(_subsidiaryServiceMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            // Use reflection to access anonymous object properties
            var subsidiaryViewTypeProperty = response.GetType().GetProperty("SubsidiaryViewType");
            var subsidiaryListProperty = response.GetType().GetProperty("SubsidiaryList");
            
            Assert.NotNull(subsidiaryViewTypeProperty);
            Assert.NotNull(subsidiaryListProperty);
            
            var subsidiaryViewType = subsidiaryViewTypeProperty.GetValue(response);
            var actualSubsidiaryList = subsidiaryListProperty.GetValue(response);
            
            Assert.Equal(SubsidiaryViewType.NormalManagement, subsidiaryViewType);
            Assert.Equal(subsidiaryList, actualSubsidiaryList);
        }
    }
} 