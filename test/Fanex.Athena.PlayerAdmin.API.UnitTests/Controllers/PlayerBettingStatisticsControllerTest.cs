namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    using API.Controllers;
    using API.DTO;
    using API.MetaModel;
    using API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    public class PlayerBettingStatisticsControllerTest
    {
        private readonly PlayerBettingStatisticsController _controller;
        private readonly Mock<IPlayerBettingStatisticsService> _mockService;

        public PlayerBettingStatisticsControllerTest()
        {
            _mockService = new Mock<IPlayerBettingStatisticsService>();
            _controller = new PlayerBettingStatisticsController(_mockService.Object);
        }

        [Fact]
        public void Constructor_WithValidService_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public async Task GetData_WithValidMetaModel_ReturnsOkWithStatistics()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel
            {
                CustId = 123,
                ProductCodeList = "SB,CS",
                CurrencyId = 1
            };

            var expectedStatistics = new PlayerBettingStatistics
            {
                WonTicketCount = 100,
                Winnings = 50.25m,
                Loss = -1000.00m
            };

            _mockService.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedStatistics, okResult.Value);
            _mockService.Verify(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()), Times.Once);
        }

        [Fact]
        public async Task GetData_SetsCorrectDateRange_CallsServiceWithCurrentMonthDates()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel
            {
                CustId = 123,
                ProductCodeList = "SB",
                CurrencyId = 1
            };

            var expectedStatistics = new PlayerBettingStatistics
            {
                WonTicketCount = 50,
                Winnings = 25.00m,
                Loss = 0m
            };

            _mockService.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Verify that the service was called with a meta model that has dates set

            _mockService.Verify(x => x.GetPlayerBettingStatistics(It.Is<PlayerBettingStatisticsMetaModel>(m =>
                m.FromDate.Day == 1 && // First day of current month
                m.FromDate.Month == DateTime.Now.Month &&
                m.FromDate.Year == DateTime.Now.Year &&
                m.ToDate.Date <= DateTime.Now.Date)), Times.Once);
        }

        [Fact]
        public async Task GetData_WithServiceReturningNull_ReturnsOkWithNull()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel
            {
                CustId = 123,
                ProductCodeList = "SB",
                CurrencyId = 1
            };

            _mockService.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync((PlayerBettingStatistics)null);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }

        [Fact]
        public async Task GetData_WithServiceThrowingException_ThrowsException()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel
            {
                CustId = 123,
                ProductCodeList = "SB",
                CurrencyId = 1
            };

            _mockService.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.GetData(metaModel));
        }

        [Fact]
        public async Task GetData_WithDifferentProductCodes_CallsServiceWithCorrectProductList()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel
            {
                CustId = 456,
                ProductCodeList = "CS,LC,AG",
                CurrencyId = 2
            };

            var expectedStatistics = new PlayerBettingStatistics
            {
                WonTicketCount = 200,
                Winnings = 0m,
                Loss = -100.50m
            };

            _mockService.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedStatistics, okResult.Value);


            _mockService.Verify(x => x.GetPlayerBettingStatistics(It.Is<PlayerBettingStatisticsMetaModel>(m => m.CustId == 456 &&
                m.ProductCodeList == "CS,LC,AG" &&
                m.CurrencyId == 2)), Times.Once);
        }

        [Fact]
        public async Task GetData_WithCompleteStatistics_ReturnsAllProperties()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel
            {
                CustId = 789,
                ProductCodeList = "SB,CS,LC,AG",
                CurrencyId = 3
            };

            var expectedStatistics = new PlayerBettingStatistics
            {
                WonTicketCount = 500,
                LoseTicketCount = 100,
                DrawTicketCount = 50,
                CancelledTicketCount = 10,
                Margin = 250.75m,
                AverageBetSize = 9.50m,
                Winnings = 1000.00m,
                Loss = -500.00m
            };

            _mockService.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualStatistics = Assert.IsType<PlayerBettingStatistics>(okResult.Value);


            Assert.Equal(expectedStatistics.WonTicketCount, actualStatistics.WonTicketCount);
            Assert.Equal(expectedStatistics.LoseTicketCount, actualStatistics.LoseTicketCount);
            Assert.Equal(expectedStatistics.DrawTicketCount, actualStatistics.DrawTicketCount);
            Assert.Equal(expectedStatistics.CancelledTicketCount, actualStatistics.CancelledTicketCount);
            Assert.Equal(expectedStatistics.Margin, actualStatistics.Margin);
            Assert.Equal(expectedStatistics.AverageBetSize, actualStatistics.AverageBetSize);
            Assert.Equal(expectedStatistics.Winnings, actualStatistics.Winnings);
            Assert.Equal(expectedStatistics.Loss, actualStatistics.Loss);
        }

        [Fact]
        public async Task GetData_WithZeroCustId_StillCallsService()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel
            {
                CustId = 0,
                ProductCodeList = "SB",
                CurrencyId = 1
            };

            var expectedStatistics = new PlayerBettingStatistics
            {
                WonTicketCount = 0,
                Winnings = 0m,
                Loss = 0m
            };

            _mockService.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedStatistics, okResult.Value);
            _mockService.Verify(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()), Times.Once);
        }
    }
}