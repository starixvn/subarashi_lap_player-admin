namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    using Core.Models;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using MVC.Controllers;
    using MVC.ViewModels;

    public class PlayerBtcSingleCustomerControllerTest
    {
        private readonly PlayerBtcSingleCustomerController _controller;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<ILicenseeService> _mockLicenseeService;

        public PlayerBtcSingleCustomerControllerTest()
        {
            _mockLicenseeService = new Mock<ILicenseeService>();
            _mockContextService = new Mock<IContextService>();
            _controller = new PlayerBtcSingleCustomerController(_mockLicenseeService.Object, _mockContextService.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public async Task Index_WithValidParameters_ReturnsViewWithCorrectModel()
        {
            // Arrange
            const string userName = "testuser";
            const string currencyName = "USD";
            const int licenseeId = 123;
            var products = new List<Product>
            {
                new() { ProductCode = "SPORT", ProductName = "Sports" },
                new() { ProductCode = "CASINO", ProductName = "Casino" }
            };
            var productAccess = new ProductAccess(products);

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);

            Assert.Equal(userName, model.UserName);
            Assert.Equal(currencyName, model.CurrencyName);
            Assert.True(model.IsAllowedCustomerComm);
            Assert.Equal("SPORT,CASINO", model.ProductCodeListString);
        }

        [Fact]
        public async Task Index_WithWhitespaceInParameters_TrimsParameters()
        {
            // Arrange
            const string userName = "  testuser  ";
            const string currencyName = "  USD  ";
            const int licenseeId = 123;
            var products = new List<Product>
            {
                new() { ProductCode = "SPORT", ProductName = "Sports" }
            };
            var productAccess = new ProductAccess(products);

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);

            Assert.Equal("testuser", model.UserName);
            Assert.Equal("USD", model.CurrencyName);
            Assert.False(model.IsAllowedCustomerComm);
        }

        [Fact]
        public async Task Index_WithNullParameters_HandlesNullValues()
        {
            // Arrange
            string userName = null;
            string currencyName = null;
            const int licenseeId = 123;
            var products = new List<Product>
            {
                new() { ProductCode = "CASINO", ProductName = "Casino" }
            };
            var productAccess = new ProductAccess(products);

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);

            Assert.Null(model.UserName);
            Assert.Null(model.CurrencyName);
            Assert.True(model.IsAllowedCustomerComm);
            Assert.Equal("CASINO", model.ProductCodeListString);
        }

        [Fact]
        public async Task Index_WithEmptyParameters_HandlesEmptyValues()
        {
            // Arrange
            const string userName = "";
            const string currencyName = "";
            const int licenseeId = 123;
            var products = new List<Product>();
            var productAccess = new ProductAccess(products);

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(false);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);

            Assert.Equal("", model.UserName);
            Assert.Equal("", model.CurrencyName);
            Assert.False(model.IsAllowedCustomerComm);
            Assert.Equal("", model.ProductCodeListString);
        }

        [Fact]
        public async Task Index_LicenseeServiceThrowsException_PropagatesException()
        {
            // Arrange
            const string userName = "testuser";
            const string currencyName = "USD";
            const int licenseeId = 123;

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ThrowsAsync(new InvalidOperationException("Service error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Index(userName, currencyName));
        }

        [Fact]
        public async Task Index_CallsLicenseeServiceWithCorrectParameters()
        {
            // Arrange
            const string userName = "testuser";
            const string currencyName = "USD";
            const int licenseeId = 456;
            var productAccess = new ProductAccess(new List<Product>());

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            await _controller.Index(userName, currencyName);

            // Assert
            _mockLicenseeService.Verify(x => x.GetSingleWalletProductAccess(456, "BettingTicketCenter"), Times.Once);
        }

        [Fact]
        public async Task Index_WithSpecialCharactersInUserName_HandlesCorrectly()
        {
            // Arrange
            const string userName = "test@user.com";
            const string currencyName = "EUR";
            const int licenseeId = 123;
            var products = new List<Product>
            {
                new() { ProductCode = "SPORT", ProductName = "Sports" }
            };
            var productAccess = new ProductAccess(products);

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);

            Assert.Equal("test@user.com", model.UserName);
            Assert.Equal("EUR", model.CurrencyName);
        }

        [Fact]
        public async Task Index_WithNullProductAccess_HandlesNullProductAccess()
        {
            // Arrange
            const string userName = "testuser";
            const string currencyName = "USD";
            const int licenseeId = 123;

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync((ProductAccess)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _controller.Index(userName, currencyName));
        }

        [Fact]
        public async Task Index_ChecksIsAllowedCustomerCommProperty()
        {
            // Arrange
            const string userName = "testuser";
            const string currencyName = "USD";
            const int licenseeId = 123;
            var products = new List<Product>
            {
                new() { ProductCode = "SPORT", ProductName = "Sports" }
            };
            var productAccess = new ProductAccess(products);

            _mockContextService.Setup(x => x.LicenseeId).Returns(licenseeId);
            _mockContextService.Setup(x => x.IsAllowedCustomerComm).Returns(true);
            _mockLicenseeService.Setup(x => x.GetSingleWalletProductAccess(licenseeId, "BettingTicketCenter"))
                .ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(userName, currencyName);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerBtcSingleCustomerViewModel>(viewResult.Model);

            Assert.True(model.IsAllowedCustomerComm);
            _mockContextService.Verify(x => x.IsAllowedCustomerComm, Times.Once);
        }
    }
}