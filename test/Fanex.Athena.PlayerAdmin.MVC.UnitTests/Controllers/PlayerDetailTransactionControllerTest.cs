namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using MVC.Controllers;
    using MVC.Services.Interfaces;
    using MVC.ViewModels;

    public class PlayerDetailTransactionControllerTest
    {
        private readonly PlayerDetailTransactionController _controller;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IPlayerDetailTransactionService> _mockService;

        public PlayerDetailTransactionControllerTest()
        {
            _mockService = new Mock<IPlayerDetailTransactionService>();
            _mockContextService = new Mock<IContextService>();
            _controller = new PlayerDetailTransactionController(_mockService.Object, _mockContextService.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void Index_WithValidParameters_ReturnsViewWithCorrectModel()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(false);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerDetailTransactionReportViewModel>(viewResult.Model);

            Assert.Equal(custId, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
            Assert.True(model.IsEnableSingleWallet);
            Assert.False(model.IsShowSabaCoins);
        }

        [Fact]
        public void Index_WithDifferentContextSettings_ReturnsViewWithCorrectSettings()
        {
            // Arrange
            const int custId = 789;
            const int currencyId = 101;
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(false);
            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(true);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerDetailTransactionReportViewModel>(viewResult.Model);

            Assert.Equal(custId, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
            Assert.False(model.IsEnableSingleWallet);
            Assert.True(model.IsShowSabaCoins);
        }

        [Fact]
        public async Task GetDetailTransactionReport_WithValidCriteria_ReturnsOkWithData()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123,
                CurrencyId = 456
            };
            var expectedData = new PlayerDetailTransactionReportViewModel
            {
                CustId = 123,
                CurrencyId = 456,
                DetailTransactions = new List<PlayerDetailTransactionViewModel>()
            };

            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockService.Setup(x => x.GetDetailTransactionReport(It.IsAny<PlayerDetailTransactionsMetaModel>()))
                .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetDetailTransactionReport(criteria);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedData, okResult.Value);
            Assert.True(criteria.IsEnableSingleWallet);

            _mockService.Verify(x => x.GetDetailTransactionReport(criteria), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactionReport_WithNullCriteria_ThrowsNullReferenceException()
        {
            // Arrange
            PlayerDetailTransactionsMetaModel criteria = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _controller.GetDetailTransactionReport(criteria));
        }

        [Fact]
        public async Task GetDetailTransactionReport_ServiceThrowsException_PropagatesException()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();
            _mockService.Setup(x => x.GetDetailTransactionReport(It.IsAny<PlayerDetailTransactionsMetaModel>()))
                .ThrowsAsync(new InvalidOperationException("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetDetailTransactionReport(criteria));
        }

        [Fact]
        public void Index_WithZeroCustId_ReturnsViewWithZeroCustId()
        {
            // Arrange
            const int custId = 0;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerDetailTransactionReportViewModel>(viewResult.Model);

            Assert.Equal(0, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
        }

        [Fact]
        public void Index_WithNegativeCustId_ReturnsViewWithNegativeCustId()
        {
            // Arrange
            const int custId = -1;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerDetailTransactionReportViewModel>(viewResult.Model);

            Assert.Equal(-1, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
        }
    }
}