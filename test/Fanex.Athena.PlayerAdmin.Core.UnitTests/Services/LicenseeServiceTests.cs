using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class LicenseeServiceTests
    {
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IDbFactory> _dbFactoryMock;
        private readonly Mock<IOptions<AppSetting>> _appSettingOptionsMock;
        private readonly Mock<IOptions<FunctionAllowedProductsSetting>> _functionSettingOptionsMock;
        private LicenseeService _licenseeService;

        public LicenseeServiceTests()
        {
            _cacheMock = new Mock<IMemoryCache>();
            _dbFactoryMock = new Mock<IDbFactory>();
            _appSettingOptionsMock = new Mock<IOptions<AppSetting>>();
            _functionSettingOptionsMock = new Mock<IOptions<FunctionAllowedProductsSetting>>();
            
            var appSettings = new AppSetting();
            _appSettingOptionsMock.Setup(x => x.Value).Returns(appSettings);
        }

        [Fact]
        public void GetAllowedProductsByFunction_ExistingFunction_ReturnsAllowedProducts()
        {
            // Arrange
            var functionName = "WinLossAnalysis";
            var expectedProducts = new[] { "ProductA", "ProductB" };
            var functionSettings = new FunctionAllowedProductsSetting
            {
                WinLossAnalysis = expectedProducts
            };
            _functionSettingOptionsMock.Setup(x => x.Value).Returns(functionSettings);
            _licenseeService = new LicenseeService(
                _cacheMock.Object, 
                _dbFactoryMock.Object, 
                _appSettingOptionsMock.Object, 
                _functionSettingOptionsMock.Object);

            // Act
            var result = _licenseeService.GetAllowedProductsByFunction(functionName);

            // Assert
            Assert.Equal(expectedProducts, result);
        }

        [Fact]
        public void GetAllowedProductsByFunction_NonExistingFunction_ReturnsNull()
        {
            // Arrange
            var functionName = "NonExistingFunction";
            var functionSettings = new FunctionAllowedProductsSetting();
            _functionSettingOptionsMock.Setup(x => x.Value).Returns(functionSettings);
            _licenseeService = new LicenseeService(
                _cacheMock.Object, 
                _dbFactoryMock.Object, 
                _appSettingOptionsMock.Object, 
                _functionSettingOptionsMock.Object);

            // Act
            var result = _licenseeService.GetAllowedProductsByFunction(functionName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllowedSubProductOptions_NoSubProducts_ReturnsOriginalOptions()
        {
            // Arrange
            var offeredSubProductList = new List<string>();
            var productOptions = new List<ProductOption>
            {
                new ProductOption { Code = "ProductA", Status = true }
            };
            _licenseeService = new LicenseeService(
                _cacheMock.Object, 
                _dbFactoryMock.Object, 
                _appSettingOptionsMock.Object, 
                _functionSettingOptionsMock.Object);

            // Act
            var result = _licenseeService.GetAllowedSubProductOptions(offeredSubProductList, productOptions);

            // Assert
            Assert.Equal(productOptions, result);
        }

        [Fact]
        public void GetAllowedSubProductOptions_WithSubProducts_ReturnsCombinedOptions()
        {
            // Arrange
            var offeredSubProductList = new List<string> { "SubProductA", "SubProductB" };
            var productOptions = new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true }
            };
            _licenseeService = new LicenseeService(
                _cacheMock.Object, 
                _dbFactoryMock.Object, 
                _appSettingOptionsMock.Object, 
                _functionSettingOptionsMock.Object);

            // Act
            var result = _licenseeService.GetAllowedSubProductOptions(offeredSubProductList, productOptions);

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains(result, p => p.Code == "SubProductA");
            Assert.Contains(result, p => p.Code == "SubProductB");
        }

        [Fact]
        public void GetAllowedSubProductCodeList_NoSubProducts_ReturnsOriginalCodes()
        {
            // Arrange
            var offeredSubProductList = new List<string>();
            var productCodes = "ProductA,ProductB";
            _licenseeService = new LicenseeService(
                _cacheMock.Object, 
                _dbFactoryMock.Object, 
                _appSettingOptionsMock.Object, 
                _functionSettingOptionsMock.Object);

            // Act
            var result = _licenseeService.GetAllowedSubProductCodeList(offeredSubProductList, productCodes);

            // Assert
            Assert.Equal(productCodes, result);
        }

        [Fact]
        public void GetAllowedSubProductCodeList_WithSubProducts_ReturnsCombinedCodes()
        {
            // Arrange
            var offeredSubProductList = new List<string> { "SubProductA", "SubProductB" };
            var productCodes = "SB,ProductC";
            _licenseeService = new LicenseeService(
                _cacheMock.Object, 
                _dbFactoryMock.Object, 
                _appSettingOptionsMock.Object, 
                _functionSettingOptionsMock.Object);

            // Act
            var result = _licenseeService.GetAllowedSubProductCodeList(offeredSubProductList, productCodes);

            // Assert
            Assert.Equal("SB,ProductC,SubProductA,SubProductB", result);
        }
    }
} 