using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.Controllers;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    public class PlayerBetListControllerTests
    {
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly PlayerBetListController _controller;

        public PlayerBetListControllerTests()
        {
            _mockContextService = new Mock<IContextService>();
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _controller = new PlayerBetListController(_mockContextService.Object);
        }

        [Fact]
        public void Running_WithValidParameters_ReturnsViewResult()
        {
            // Arrange
            var custId = 123;
            var currencyId = 1;
            var currencyList = new Dictionary<int, string> { { 1, "USD" }, { 2, "EUR" } };

            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);

            // Act
            var result = _controller.Running(custId, currencyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerRunningBetListViewModel>(viewResult.Model);
            Assert.Equal(custId, viewModel.CustId);
            Assert.Equal("USD", viewModel.Currency);
            Assert.True(_controller.ViewBag.IsAllowTxIDColumn);
        }

        [Fact]
        public void Running_WithInvalidCurrencyId_ReturnsViewResultWithNullCurrency()
        {
            // Arrange
            var custId = 123;
            var currencyId = 999; // Non-existent currency
            var currencyList = new Dictionary<int, string> { { 1, "USD" }, { 2, "EUR" } };

            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(false);

            // Act
            var result = _controller.Running(custId, currencyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerRunningBetListViewModel>(viewResult.Model);
            Assert.Equal(custId, viewModel.CustId);
            Assert.Null(viewModel.Currency);
            Assert.False(_controller.ViewBag.IsAllowTxIDColumn);
        }

        [Fact]
        public async Task GetRunningBetList_WithValidModel_ReturnsFromJsonString()
        {
            // Arrange
            var metaModel = new PlayerRunningBetListMetaModel { CustId = 123, IsExport = false };
            var expectedJson = "{\"data\":\"test\"}";

            _mockContextService.Setup(x => x.IsShowIP).Returns(true);
            _mockContextService.Setup(x => x.IsShowIpVN).Returns(false);
            _mockContextService.Setup(x => x.SpecialPlatform).Returns(1);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockBetListApiClient.Setup(x => x.PostStringAsync(It.IsAny<string>(), It.IsAny<PlayerRunningBetListMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(expectedJson);

            // Act
            var result = await _controller.GetRunningBetList(_mockBetListApiClient.Object, metaModel);

            // Assert
            var fromJsonString = Assert.IsType<FromJsonString>(result);
            Assert.True(metaModel.IsShowIp);
            Assert.False(metaModel.IsShowIpVN);
            Assert.Equal(1, metaModel.SpecialPlatform);
        }

        [Fact]
        public async Task GetRunningBetList_WithExportModel_ReturnsFileResult()
        {
            // Arrange
            var metaModel = new PlayerRunningBetListMetaModel { CustId = 123, IsExport = true };
            var httpResponse = new HttpResponseMessage();
            var content = new StringContent("test data");
            httpResponse.Content = content;

            _mockContextService.Setup(x => x.IsShowIP).Returns(false);
            _mockContextService.Setup(x => x.IsShowIpVN).Returns(true);
            _mockContextService.Setup(x => x.SpecialPlatform).Returns(2);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockBetListApiClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<PlayerRunningBetListMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _controller.GetRunningBetList(_mockBetListApiClient.Object, metaModel);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.True(metaModel.IsAllowTxIDColumn);
        }

        [Fact]
        public async Task GetRunningBetList_WithExportModel_SetsCorrectMetaModelValues()
        {
            // Arrange
            var metaModel = new PlayerRunningBetListMetaModel { CustId = 123, IsExport = true };
            var httpResponse = new HttpResponseMessage();
            var content = new StringContent("test data");
            httpResponse.Content = content;

            _mockContextService.Setup(x => x.IsShowIP).Returns(true);
            _mockContextService.Setup(x => x.IsShowIpVN).Returns(false);
            _mockContextService.Setup(x => x.SpecialPlatform).Returns(3);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockBetListApiClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<PlayerRunningBetListMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(httpResponse);

            // Act
            await _controller.GetRunningBetList(_mockBetListApiClient.Object, metaModel);

            // Assert
            Assert.True(metaModel.IsShowIp);
            Assert.False(metaModel.IsShowIpVN);
            Assert.Equal(3, metaModel.SpecialPlatform);
            Assert.True(metaModel.IsAllowTxIDColumn);
        }
    }
} 