using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerBettingStatisticsControllerTests
    {
        private readonly Mock<IPlayerBettingStatisticsService> _serviceMock;
        private readonly PlayerBettingStatisticsController _controller;

        public PlayerBettingStatisticsControllerTests()
        {
            _serviceMock = new Mock<IPlayerBettingStatisticsService>();
            _controller = new PlayerBettingStatisticsController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetData_WithValidMetaModel_ReturnsOkResultWithStatistics()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel 
            { 
                CustId = 123,
                FromDate = DateTime.Now.AddDays(-30),
                ToDate = DateTime.Now,
                MoveBetsDay = DateTime.Now,
                BalanceUpDay = DateTime.Now,
                CurrencyId = 1,
                ProductCodeList = "SB"
            };

            var expectedStatistics = new PlayerBettingStatistics
            {
                WonTicketCount = 10,
                LoseTicketCount = 5,
                DrawTicketCount = 2,
                CancelledTicketCount = 1,
                Margin = 0.05m,
                AverageBetSize = 100.50m,
                Winnings = 500.00m,
                Loss = -200.00m
            };

            _serviceMock.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStatistics = Assert.IsType<PlayerBettingStatistics>(okResult.Value);
            
            Assert.Equal(expectedStatistics.WonTicketCount, returnedStatistics.WonTicketCount);
            Assert.Equal(expectedStatistics.LoseTicketCount, returnedStatistics.LoseTicketCount);
            Assert.Equal(expectedStatistics.DrawTicketCount, returnedStatistics.DrawTicketCount);
            Assert.Equal(expectedStatistics.CancelledTicketCount, returnedStatistics.CancelledTicketCount);
            Assert.Equal(expectedStatistics.Margin, returnedStatistics.Margin);
            Assert.Equal(expectedStatistics.AverageBetSize, returnedStatistics.AverageBetSize);
            Assert.Equal(expectedStatistics.Winnings, returnedStatistics.Winnings);
            Assert.Equal(expectedStatistics.Loss, returnedStatistics.Loss);
            Assert.Equal(expectedStatistics.NetWinLoss, returnedStatistics.NetWinLoss);
            Assert.Equal(expectedStatistics.HasData, returnedStatistics.HasData);

            _serviceMock.Verify(x => x.GetPlayerBettingStatistics(It.Is<PlayerBettingStatisticsMetaModel>(m => 
                m.CustId == metaModel.CustId &&
                m.FromDate.Date == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) &&
                m.ToDate.Date == DateTime.Now.Date)), Times.Once);
        }

        [Fact]
        public async Task GetData_WithEmptyStatistics_ReturnsOkResultWithEmptyData()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel 
            { 
                CustId = 456,
                FromDate = DateTime.Now.AddDays(-30),
                ToDate = DateTime.Now
            };

            var emptyStatistics = new PlayerBettingStatistics
            {
                WonTicketCount = 0,
                LoseTicketCount = 0,
                DrawTicketCount = 0,
                CancelledTicketCount = 0,
                Margin = 0,
                AverageBetSize = 0,
                Winnings = 0,
                Loss = 0
            };

            _serviceMock.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(emptyStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStatistics = Assert.IsType<PlayerBettingStatistics>(okResult.Value);
            
            Assert.Equal(0, returnedStatistics.WonTicketCount);
            Assert.Equal(0, returnedStatistics.LoseTicketCount);
            Assert.Equal(0, returnedStatistics.DrawTicketCount);
            Assert.Equal(0, returnedStatistics.CancelledTicketCount);
            Assert.Equal(0, returnedStatistics.Margin);
            Assert.Equal(0, returnedStatistics.AverageBetSize);
            Assert.Equal(0, returnedStatistics.Winnings);
            Assert.Equal(0, returnedStatistics.Loss);
            Assert.Equal(0, returnedStatistics.NetWinLoss);
            Assert.False(returnedStatistics.HasData);

            _serviceMock.Verify(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()), Times.Once);
        }

        [Fact]
        public async Task GetData_WithNullMetaModel_ReturnsOkResultWithServiceCall()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel();
            var expectedStatistics = new PlayerBettingStatistics();

            _serviceMock.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            var result = await _controller.GetData(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<PlayerBettingStatistics>(okResult.Value);

            _serviceMock.Verify(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()), Times.Once);
        }

        [Fact]
        public async Task GetData_WithServiceException_PropagatesException()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel { CustId = 789 };
            var expectedException = new InvalidOperationException("Service error");

            _serviceMock.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ThrowsAsync(expectedException);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetData(metaModel));
            Assert.Equal("Service error", exception.Message);

            _serviceMock.Verify(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()), Times.Once);
        }

        [Fact]
        public async Task GetData_UpdatesMetaModelDatesCorrectly()
        {
            // Arrange
            var metaModel = new PlayerBettingStatisticsMetaModel 
            { 
                CustId = 123,
                FromDate = DateTime.MinValue,
                ToDate = DateTime.MinValue
            };

            var expectedStatistics = new PlayerBettingStatistics();
            _serviceMock.Setup(x => x.GetPlayerBettingStatistics(It.IsAny<PlayerBettingStatisticsMetaModel>()))
                .ReturnsAsync(expectedStatistics);

            // Act
            await _controller.GetData(metaModel);

            // Assert
            _serviceMock.Verify(x => x.GetPlayerBettingStatistics(It.Is<PlayerBettingStatisticsMetaModel>(m => 
                m.FromDate.Date == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) &&
                m.ToDate.Date == DateTime.Now.Date)), Times.Once);
        }
    }
} 