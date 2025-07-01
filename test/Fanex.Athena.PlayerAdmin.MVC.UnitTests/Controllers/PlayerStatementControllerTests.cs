using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.Controllers;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    public class PlayerStatementControllerTests
    {
        private readonly Mock<IPlayerStatementService> _mockPlayerStatementService;
        private readonly Mock<IContextService> _mockContextService;
        private readonly PlayerStatementController _controller;

        public PlayerStatementControllerTests()
        {
            _mockPlayerStatementService = new Mock<IPlayerStatementService>();
            _mockContextService = new Mock<IContextService>();
            _controller = new PlayerStatementController(_mockPlayerStatementService.Object, _mockContextService.Object);
        }

        [Fact]
        public void Index_WithValidParameters_ReturnsViewResult()
        {
            // Arrange
            var custId = 123;
            var currencyId = 1;
            var today = DateTime.Today;

            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(true);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);
            Assert.Equal(custId, viewModel.CustId);
            Assert.Equal(currencyId, viewModel.CurrencyId);
            Assert.Equal(new DateTime(today.Year, today.Month, 1), viewModel.FromDate);
            Assert.Equal(today, viewModel.ToDate);
            Assert.Equal(new DateTime(today.Year - 2, 1, 1), viewModel.MinDate);
            Assert.Equal(new DateTime(today.Year + 5, 12, 31), viewModel.MaxDate);
            Assert.True(viewModel.IsShowSabaCoins);
        }

        [Fact]
        public void Index_WithDifferentContextSettings_ReturnsCorrectViewModel()
        {
            // Arrange
            var custId = 456;
            var currencyId = 2;

            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(false);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);
            Assert.Equal(custId, viewModel.CustId);
            Assert.Equal(currencyId, viewModel.CurrencyId);
            Assert.False(viewModel.IsShowSabaCoins);
        }

        [Fact]
        public async Task GetStatementUrl_WithValidMetaModel_ReturnsContentResult()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel { CustId = 123 };
            var expectedUrl = "https://example.com/statement/123";
            var contextService = new Mock<IContextService>();

            contextService.Setup(x => x.IsShowIpVN).Returns(true);
            _mockPlayerStatementService.Setup(x => x.GetUrl(metaModel))
                .ReturnsAsync(expectedUrl);

            // Act
            var result = await _controller.GetStatementUrl(metaModel, contextService.Object);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal(expectedUrl, contentResult.Content);
            Assert.True(metaModel.IsShowIpVN);
        }

        [Fact]
        public async Task GetStatementUrl_WithDifferentIpVNSettings_SetsCorrectValue()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel { CustId = 123 };
            var expectedUrl = "https://example.com/statement/123";
            var contextService = new Mock<IContextService>();

            contextService.Setup(x => x.IsShowIpVN).Returns(false);
            _mockPlayerStatementService.Setup(x => x.GetUrl(metaModel))
                .ReturnsAsync(expectedUrl);

            // Act
            var result = await _controller.GetStatementUrl(metaModel, contextService.Object);

            // Assert
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.Equal(expectedUrl, contentResult.Content);
            Assert.False(metaModel.IsShowIpVN);
        }

        [Fact]
        public async Task GetStatementUrl_WithNullMetaModel_ThrowsException()
        {
            // Arrange
            PlayerStatementMetaModel metaModel = null;
            var contextService = new Mock<IContextService>();

            contextService.Setup(x => x.IsShowIpVN).Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _controller.GetStatementUrl(metaModel, contextService.Object));
        }

        [Fact]
        public async Task GetStatementUrl_WithNullContextService_ThrowsException()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel { CustId = 123 };
            IContextService contextService = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _controller.GetStatementUrl(metaModel, contextService));
        }
    }
} 