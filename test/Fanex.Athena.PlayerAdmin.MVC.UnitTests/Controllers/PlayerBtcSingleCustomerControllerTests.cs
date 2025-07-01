using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
    public class PlayerBtcSingleCustomerControllerTests
    {
        private readonly Mock<ILicenseeService> _mockLicenseeService;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IBetListApiClient> _mockBetListApiClient;
        private readonly PlayerBtcSingleCustomerController _controller;

        public PlayerBtcSingleCustomerControllerTests()
        {
            _mockLicenseeService = new Mock<ILicenseeService>();
            _mockContextService = new Mock<IContextService>();
            _mockBetListApiClient = new Mock<IBetListApiClient>();
            _controller = new PlayerBtcSingleCustomerController(_mockLicenseeService.Object, _mockContextService.Object);
        }

        [Fact]
        public async Task Index_Get_WithValidParameters_ReturnsViewResult()
        {
            // Arrange
            var userName = "testuser";
            var currencyName = "USD";
            var productAccess = new ProductAccess(new List<Core.Models.Product>());

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(1, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);
            Assert.Equal(userName, viewModel.UserName);
            Assert.Equal(currencyName, viewModel.CurrencyName);
            Assert.True(viewModel.IsAllowedCustomerComm);
            Assert.Equal(productAccess.ProductCodeListString, viewModel.ProductCodeListString);
            Assert.True(_controller.ViewBag.IsAllowTxIDColumn);
        }

        [Fact]
        public async Task Index_Get_WithNullParameters_ReturnsViewResult()
        {
            // Arrange
            var productAccess = new ProductAccess(new List<Core.Models.Product>());

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(false);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(1, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index((string)null, (string)null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);
            Assert.Null(viewModel.UserName);
            Assert.Null(viewModel.CurrencyName);
            Assert.False(viewModel.IsAllowedCustomerComm);
            Assert.False(_controller.ViewBag.IsAllowTxIDColumn);
        }

        [Fact]
        public async Task Index_Post_WithValidModel_ReturnsFromJsonString()
        {
            // Arrange
            var metaModel = new PlayerBtcSingleCustomerMetaModel 
            { 
                UserName = "testuser", 
                PageSize = 10, 
                PageIndex = 1, 
                IsExport = false,
                SortBy = "0",
                SortOrder = "0"
            };
            var expectedJson = "{\"data\":\"test\"}";

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockBetListApiClient.Setup(x => x.PostStringAsync(It.IsAny<string>(), It.IsAny<PlayerBtcSingleCustomerMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(expectedJson);

            // Act
            var result = await _controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            var fromJsonString = Assert.IsType<FromJsonString>(result);
        }

        [Fact]
        public async Task Index_Post_WithExportModel_ReturnsFileResult()
        {
            // Arrange
            var metaModel = new PlayerBtcSingleCustomerMetaModel 
            { 
                UserName = "testuser", 
                PageSize = 10, 
                PageIndex = 1, 
                IsExport = true,
                SortBy = "0",
                SortOrder = "0"
            };
            var httpResponse = new HttpResponseMessage();
            var content = new StringContent("test data");
            httpResponse.Content = content;

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockBetListApiClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<PlayerBtcSingleCustomerMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
        }

        [Fact]
        public async Task Index_Post_WithExportModelAndNoContent_ReturnsNoContentResult()
        {
            // Arrange
            var metaModel = new PlayerBtcSingleCustomerMetaModel 
            { 
                UserName = "testuser", 
                PageSize = 10, 
                PageIndex = 1, 
                IsExport = true,
                SortBy = "0",
                SortOrder = "0"
            };
            var httpResponse = new HttpResponseMessage(HttpStatusCode.NoContent);

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockBetListApiClient.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<PlayerBtcSingleCustomerMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Index_Post_AssignsMetaModelValues_Correctly()
        {
            // Arrange
            var metaModel = new PlayerBtcSingleCustomerMetaModel 
            { 
                UserName = "testuser", 
                PageSize = 10, 
                PageIndex = 1, 
                IsExport = false,
                SortBy = "0",
                SortOrder = "0"
            };

            _mockContextService.Setup(x => x.LicenseeId).Returns(1);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockBetListApiClient.Setup(x => x.PostStringAsync(It.IsAny<string>(), It.IsAny<PlayerBtcSingleCustomerMetaModel>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync("{}");

            // Act
            await _controller.Index(_mockBetListApiClient.Object, metaModel);

            // Assert
            Assert.True(metaModel.IsAllowTxIDColumn);
            Assert.Equal("112,113,111,102,99,101,103,2", metaModel.Status); // Actual StatusTypes values
            Assert.Equal("All", metaModel.Platform);
            Assert.Equal("0,1,2", metaModel.LiveStatus); // LiveStatusTypes values
            Assert.Equal("All", metaModel.SportIds);
            Assert.Equal("All", metaModel.BetTypeIds);
            Assert.Equal(0, metaModel.CashOutMode);
        }
    }
} 