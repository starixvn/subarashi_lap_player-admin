namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    using Core.ConsoleClient;
    using Core.Extensions;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using MVC.Controllers;
    using MVC.ViewModels;

    public class PlayerBetListControllerTest
    {
        private readonly PlayerBetListController _controller;
        private readonly Mock<IContextService> _mockContextService;

        public PlayerBetListControllerTest()
        {
            _mockContextService = new Mock<IContextService>();
            _controller = new PlayerBetListController(_mockContextService.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void Running_WithValidParameters_ReturnsViewWithCorrectModel()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;
            var currencyList = new Dictionary<int, string>
            {
                { 456, "USD" },
                { 789, "EUR" }
            };

            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);

            // Act
            var result = _controller.Running(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerRunningBetListViewModel>(viewResult.Model);

            Assert.Equal(custId, model.CustId);
            Assert.Equal("USD", model.Currency);
            Assert.True((bool)viewResult.ViewData["IsAllowTxIDColumn"]);
        }

        [Fact]
        public void Running_WithNonExistentCurrencyId_ReturnsViewWithNullCurrency()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 999; // Non-existent currency
            var currencyList = new Dictionary<int, string>
            {
                { 456, "USD" },
                { 789, "EUR" }
            };

            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(false);

            // Act
            var result = _controller.Running(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerRunningBetListViewModel>(viewResult.Model);

            Assert.Equal(custId, model.CustId);
            Assert.Null(model.Currency);
            Assert.False((bool)viewResult.ViewData["IsAllowTxIDColumn"]);
        }

        [Fact]
        public void Running_WithEmptyCurrencyList_ReturnsViewWithNullCurrency()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;
            var currencyList = new Dictionary<int, string>();

            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);

            // Act
            var result = _controller.Running(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerRunningBetListViewModel>(viewResult.Model);

            Assert.Equal(custId, model.CustId);
            Assert.Null(model.Currency);
        }

        [Fact]
        public void Running_WithZeroCustId_ReturnsViewWithZeroCustId()
        {
            // Arrange
            const int custId = 0;
            const int currencyId = 456;
            var currencyList = new Dictionary<int, string>
            {
                { 456, "USD" }
            };

            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);

            // Act
            var result = _controller.Running(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerRunningBetListViewModel>(viewResult.Model);

            Assert.Equal(0, model.CustId);
        }

        [Fact]
        public void Running_WithNegativeCustId_ReturnsViewWithNegativeCustId()
        {
            // Arrange
            const int custId = -1;
            const int currencyId = 456;
            var currencyList = new Dictionary<int, string>
            {
                { 456, "USD" }
            };

            _mockContextService.Setup(x => x.CurrencyList).Returns(currencyList);

            // Act
            var result = _controller.Running(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerRunningBetListViewModel>(viewResult.Model);

            Assert.Equal(-1, model.CustId);
        }

        [Fact]
        public async Task GetRunningBetList_WithValidMetaModel_CallsApiClientCorrectly()
        {
            // Arrange
            var mockBetListApiClient = new Mock<IBetListApiClient>();
            var metaModel = new PlayerRunningBetListMetaModel
            {
                CustId = 123,
                IsExport = false
            };
            var expectedResult = "{\"Success\": true, \"Data\": \"test data\"}";

            _mockContextService.Setup(x => x.IsShowIP).Returns(true);
            _mockContextService.Setup(x => x.IsShowIpVN).Returns(false);
            _mockContextService.Setup(x => x.SpecialPlatform).Returns(1);
            _mockContextService.Setup(x => x.Language).Returns("en");

            mockBetListApiClient.Setup(x => x.PostStringAsync("/api/PlayerAdminBetList/GetBetList", metaModel, "en", 100))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetRunningBetList(mockBetListApiClient.Object, metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FromJsonString>(result);

            Assert.True(metaModel.IsShowIp);
            Assert.False(metaModel.IsShowIpVN);
            Assert.Equal(1, metaModel.SpecialPlatform);

            mockBetListApiClient.Verify(x => x.PostStringAsync("/api/PlayerAdminBetList/GetBetList", metaModel, "en", 100), Times.Once);
        }

        [Fact]
        public async Task GetRunningBetList_WithExportTrue_CallsExportMethod()
        {
            // Arrange
            var mockBetListApiClient = new Mock<IBetListApiClient>();
            var metaModel = new PlayerRunningBetListMetaModel
            {
                CustId = 123,
                IsExport = true
            };
            var testStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
            var content = new StreamContent(testStream);
            var httpResponse = new HttpResponseMessage
            {
                Content = content
            };

            _mockContextService.Setup(x => x.IsShowIP).Returns(false);
            _mockContextService.Setup(x => x.IsShowIpVN).Returns(true);
            _mockContextService.Setup(x => x.SpecialPlatform).Returns(1);
            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.Language).Returns("en");

            mockBetListApiClient.Setup(x => x.PostAsync("/api/PlayerAdminBetList/Export", metaModel, "en", 100))
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _controller.GetRunningBetList(mockBetListApiClient.Object, metaModel);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsAssignableFrom<FileResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);

            Assert.False(metaModel.IsShowIp);
            Assert.True(metaModel.IsShowIpVN);
            Assert.Equal(1, metaModel.SpecialPlatform);
            Assert.True(metaModel.IsAllowTxIDColumn);

            mockBetListApiClient.Verify(x => x.PostAsync("/api/PlayerAdminBetList/Export", metaModel, "en", 100), Times.Once);
        }

        [Fact]
        public async Task GetRunningBetList_WithNullMetaModel_ThrowsNullReferenceException()
        {
            // Arrange
            var mockBetListApiClient = new Mock<IBetListApiClient>();
            PlayerRunningBetListMetaModel metaModel = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _controller.GetRunningBetList(mockBetListApiClient.Object, metaModel));
        }

        [Fact]
        public async Task GetRunningBetList_ApiClientThrowsException_PropagatesException()
        {
            // Arrange
            var mockBetListApiClient = new Mock<IBetListApiClient>();
            var metaModel = new PlayerRunningBetListMetaModel();

            _mockContextService.Setup(x => x.Language).Returns("en");

            mockBetListApiClient.Setup(x => x.PostStringAsync("/api/PlayerAdminBetList/GetBetList", metaModel, "en", 100))
                .ThrowsAsync(new InvalidOperationException("API error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _controller.GetRunningBetList(mockBetListApiClient.Object, metaModel));
        }
    }
}