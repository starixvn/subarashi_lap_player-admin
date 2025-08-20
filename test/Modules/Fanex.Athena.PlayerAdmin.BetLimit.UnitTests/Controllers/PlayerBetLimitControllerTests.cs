namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    using AutoFixture;
    using BetLimit.Controllers;
    using BetLimit.DTO;
    using BetLimit.Repositories.Interfaces;
    using Core.Models;
    using Core.Services;
    using Core.Settings;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Moq;
    using ViewModels;

    public class PlayerBetLimitControllerTests
    {
        private readonly IOptions<AppSetting> _appSetting;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly PlayerBetLimitController _controller;
        private readonly IFixture _fixture;
        private readonly Mock<ILicenseeService> _licenseeServiceMock;
        private readonly Mock<IPlayerBetLimitRepository> _playerBetLimitRepositoryMock;

        public PlayerBetLimitControllerTests()
        {
            _fixture = new Fixture();
            _playerBetLimitRepositoryMock = new Mock<IPlayerBetLimitRepository>();
            _licenseeServiceMock = new Mock<ILicenseeService>();
            _contextServiceMock = new Mock<IContextService>();
            _appSetting = Options.Create(new AppSetting { Env = "TEST" });


            _controller = new PlayerBetLimitController(_playerBetLimitRepositoryMock.Object, _licenseeServiceMock.Object);
        }

        [Fact]
        public async Task Index_ShouldReturnViewResult_WhenCalled()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 1;
            const int licenseeCustId = 456;
            const string siteName = "TestSite";
            const int licenseeId = 789;


            var products = _fixture.CreateMany<PlayerBetLimitProduct>().ToList();
            var productAccess = new ProductAccess(new List<Product>
                { new() { ProductCode = "PROD1", ProductName = "Product 1" }, new() { ProductCode = "PROD2", ProductName = "Product 2" } });
            _contextServiceMock.Setup(x => x.SiteName).Returns(siteName);
            _contextServiceMock.Setup(x => x.LicenseeId).Returns(licenseeId);
            _playerBetLimitRepositoryMock.Setup(x => x.GetEnableBetLimitProducts(siteName, It.IsAny<string>())).ReturnsAsync(products);
            _licenseeServiceMock.Setup(x => x.GetProductAccess(licenseeId, It.IsAny<string>())).ReturnsAsync(productAccess);

            // Act

            var result = await _controller.Index(_contextServiceMock.Object, _appSetting, custId, currencyId, licenseeCustId);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result;
            var model = viewResult.Model as PlayerBetLimitProductViewModel;
            Assert.NotNull(model);
            Assert.Equal(custId, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
            Assert.Equal(licenseeCustId, model.LicenseeCustId);
            Assert.NotNull(model.ProductList);
        }

        [Fact]
        public async Task Index_ShouldSetFirstProductAsDefault_WhenProductsExist()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 1;
            const int licenseeCustId = 456;
            const string siteName = "TestSite";
            const int licenseeId = 789;


            var products = new List<PlayerBetLimitProduct>
            {
                new() { ProductId = 1, ProductCode = "PROD1" },
                new() { ProductId = 2, ProductCode = "PROD2" }
            };
            var productAccess = new ProductAccess(new List<Product>
                { new() { ProductCode = "PROD1", ProductName = "Product 1" }, new() { ProductCode = "PROD2", ProductName = "Product 2" } });

            _contextServiceMock.Setup(x => x.SiteName).Returns(siteName);
            _contextServiceMock.Setup(x => x.LicenseeId).Returns(licenseeId);
            _playerBetLimitRepositoryMock.Setup(x => x.GetEnableBetLimitProducts(siteName, It.IsAny<string>())).ReturnsAsync(products);
            _licenseeServiceMock.Setup(x => x.GetProductAccess(licenseeId, It.IsAny<string>())).ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(_contextServiceMock.Object, _appSetting, custId, currencyId, licenseeCustId);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result;
            var model = viewResult.Model as PlayerBetLimitProductViewModel;
            Assert.NotNull(model);
            Assert.Equal(1, model.ProductId);
        }

        [Fact]
        public async Task Index_ShouldCreateProductDictionary_WhenProductsExist()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 1;
            const int licenseeCustId = 456;
            const string siteName = "TestSite";
            const int licenseeId = 789;


            var products = new List<PlayerBetLimitProduct>
            {
                new() { ProductId = 1, ProductCode = "PROD1" },
                new() { ProductId = 2, ProductCode = "PROD2" }
            };
            var productAccess = new ProductAccess(new List<Product>
                { new() { ProductCode = "PROD1", ProductName = "Product 1" }, new() { ProductCode = "PROD2", ProductName = "Product 2" } });

            _contextServiceMock.Setup(x => x.SiteName).Returns(siteName);
            _contextServiceMock.Setup(x => x.LicenseeId).Returns(licenseeId);
            _playerBetLimitRepositoryMock.Setup(x => x.GetEnableBetLimitProducts(siteName, It.IsAny<string>())).ReturnsAsync(products);
            _licenseeServiceMock.Setup(x => x.GetProductAccess(licenseeId, It.IsAny<string>())).ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(_contextServiceMock.Object, _appSetting, custId, currencyId, licenseeCustId);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result;
            var model = viewResult.Model as PlayerBetLimitProductViewModel;
            Assert.NotNull(model);
            Assert.Equal(2, model.ProductList.Count);
            Assert.True(model.ProductList.ContainsKey(1));
            Assert.True(model.ProductList.ContainsKey(2));
            Assert.Equal("Product 1", model.ProductList[1]);
            Assert.Equal("Product 2", model.ProductList[2]);
        }

        [Fact]
        public async Task Index_ShouldUseEnvironmentFromAppSetting_WhenGettingProducts()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 1;
            const int licenseeCustId = 456;
            const string siteName = "TestSite";
            const int licenseeId = 789;
            const string environment = "PROD";


            var appSettingWithEnv = Options.Create(new AppSetting { Env = environment });
            var products = _fixture.CreateMany<PlayerBetLimitProduct>().ToList();
            var productAccess = _fixture.Create<ProductAccess>();

            _contextServiceMock.Setup(x => x.SiteName).Returns(siteName);
            _contextServiceMock.Setup(x => x.LicenseeId).Returns(licenseeId);
            _playerBetLimitRepositoryMock.Setup(x => x.GetEnableBetLimitProducts(siteName, It.IsAny<string>())).ReturnsAsync(products);
            _licenseeServiceMock.Setup(x => x.GetProductAccess(licenseeId, It.IsAny<string>())).ReturnsAsync(productAccess);

            // Act
            var result = await _controller.Index(_contextServiceMock.Object, appSettingWithEnv, custId, currencyId, licenseeCustId);

            // Assert
            Assert.IsType<ViewResult>(result);
            _playerBetLimitRepositoryMock.Verify(x => x.GetEnableBetLimitProducts(siteName, environment), Times.Once);
        }
    }
}