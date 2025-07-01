using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.Controllers;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    public class PlayerWinLossControllerTests
    {
        private readonly Mock<ILicenseeService> _mockLicenseeService;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IReportApiClient> _mockReportApiClient;
        private readonly PlayerWinLossController _controller;

        public PlayerWinLossControllerTests()
        {
            _mockLicenseeService = new Mock<ILicenseeService>();
            _mockContextService = new Mock<IContextService>();
            _mockReportApiClient = new Mock<IReportApiClient>();
            _controller = new PlayerWinLossController(_mockLicenseeService.Object, _mockContextService.Object);
        }

        [Fact]
        public async Task Index_Get_WithValidParameters_ReturnsViewResult()
        {
            // Arrange
            var userName = "testuser";
            var currencyName = "USD";
            var productAccess = new ProductAccess(new List<Core.Models.Product>());
            
            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(1, "WinLossAnalysis"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerFastCrossLinkViewModel>(viewResult.Model);
            Assert.Equal(userName, viewModel.UserName);
            Assert.Equal(currencyName, viewModel.CurrencyName);
            Assert.Equal(productAccess.ProductCodeListString, viewModel.ProductCodeListString);
        }

        [Fact]
        public async Task Index_Get_WithNullParameters_ReturnsViewResult()
        {
            // Arrange
            var productAccess = new ProductAccess(new List<Core.Models.Product>());
            
            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(1, "WinLossAnalysis"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index((string)null, (string)null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerFastCrossLinkViewModel>(viewResult.Model);
            Assert.Null(viewModel.UserName);
            Assert.Null(viewModel.CurrencyName);
        }

        [Fact]
        public async Task Index_Post_WithValidModel_ReturnsFromJsonString()
        {
            // Arrange
            var metaModel = new PlayerWinLossMetaModel { Username = "testuser", BaseCurrency = "USD" };
            var expectedJson = "{\"data\":\"test\"}";
            var currencyList = new Dictionary<int, string> { { 1, "USD" }, { 2, "EUR" } };
            var productAccess = new ProductAccess(new List<Core.Models.Product>());

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(1, "WinLossAnalysis"))
                .ReturnsAsync(productAccess);
            _mockReportApiClient.Setup(x => x.PostStringAsync(It.IsAny<string>(), It.IsAny<PlayerWinLossMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(expectedJson);

            // Act
            var result = await _controller.Index(_mockReportApiClient.Object, metaModel);

            // Assert
            var fromJsonString = Assert.IsType<FromJsonString>(result);
        }

        [Fact]
        public async Task Index_Post_AssignsMetaModelValues_Correctly()
        {
            // Arrange
            var metaModel = new PlayerWinLossMetaModel { Username = "testuser", BaseCurrency = "USD" };
            var currencyList = new Dictionary<int, string> { { 1, "USD" }, { 2, "EUR" } };
            var productAccess = new ProductAccess(new List<Core.Models.Product>());

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(1, "WinLossAnalysis"))
                .ReturnsAsync(productAccess);
            _mockReportApiClient.Setup(x => x.PostStringAsync(It.IsAny<string>(), It.IsAny<PlayerWinLossMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync("{}");

            // Act
            await _controller.Index(_mockReportApiClient.Object, metaModel);

            // Assert
            Assert.Equal("testuser", metaModel.SearchUserName);
            Assert.Equal("All", metaModel.SportIds);
            Assert.Equal("All", metaModel.BetTypeIds);
            Assert.Equal("All", metaModel.StatusIds);
            Assert.Equal("All", metaModel.PlatformIds);
            Assert.Equal("All", metaModel.LiveStatusIds);
            Assert.False(metaModel.IsViewCashOutOnly);
            Assert.Equal(GroupByMode.Player, metaModel.GroupMode);
            Assert.Equal(BreakdownMode.Product, metaModel.BreakdownMode);
            Assert.Equal(ViewInTimeMode.Financial, metaModel.ViewInTime);
            Assert.True(metaModel.IsShowInOrginalCurrency);
            Assert.False(metaModel.IsExcludedTestData);
            Assert.False(metaModel.IsVipOnly);
            Assert.Equal("All", metaModel.OddsGroup);
            Assert.Equal("All", metaModel.CustomerClass);
            Assert.Equal(SearchMode.MatchPrecise, metaModel.SearchMode);
            Assert.False(metaModel.SubsidiarySearch);
            Assert.Equal(0, metaModel.SortBy);
            Assert.Equal(0, metaModel.SortOrder);
        }
    }
} 