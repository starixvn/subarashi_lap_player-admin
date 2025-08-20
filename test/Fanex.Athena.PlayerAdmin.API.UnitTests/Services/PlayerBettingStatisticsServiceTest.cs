namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using System.Data;
    using System.Data.Common;
    using API.DTO;
    using API.MetaModel;
    using API.Services;
    using AutoFixture;
    using Core.Repositories;
    using Data;
    using Moq;

    public class PlayerBettingStatisticsServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IDbFactory> _dbFactoryMock;
        private readonly PlayerBettingStatisticsService _playerBettingStatisticsService;

        public PlayerBettingStatisticsServiceTest()
        {
            _fixture = new Fixture();
            _dbFactoryMock = new Mock<IDbFactory>();
            _playerBettingStatisticsService = new PlayerBettingStatisticsService(_dbFactoryMock.Object);
        }

        [Fact]
        public async Task GetPlayerBettingStatistics_ShouldReturnCorrectData_WhenReaderHasData()
        {
            var metaModel = _fixture.Create<PlayerBettingStatisticsMetaModel>();

            var readerMock = new Mock<IDataReader>();
            var objectDbMock = new Mock<IObjectDb>();
            var expectedResult = _fixture.Create<PlayerBettingStatistics>();
            
            var readerResults = new List<Action<Mock<IDataReader>>>
            {
                reader =>
                {
                    reader.SetupSequence(r => r.Read())
                        .Returns(true)
                        .Returns(false);
                    reader.Setup(r => r["Won"]).Returns(expectedResult.WonTicketCount);
                    reader.Setup(r => r["Lose"]).Returns(expectedResult.LoseTicketCount);
                    reader.Setup(r => r["Draw"]).Returns(expectedResult.DrawTicketCount);
                    reader.Setup(r => r["Cancelled"]).Returns(expectedResult.CancelledTicketCount);
                },
                reader =>
                {
                    reader.SetupSequence(r => r.Read())
                        .Returns(true)
                        .Returns(false);
                    reader.Setup(r => r["Margin"]).Returns(expectedResult.Margin);
                    reader.Setup(r => r["AverageBetSize"]).Returns(expectedResult.AverageBetSize);
                    reader.Setup(r => r["Winnings"]).Returns(expectedResult.Winnings);
                    reader.Setup(r => r["Loss"]).Returns(expectedResult.Loss);
                }
            };
            
            var resultSetIndex = 0;
            readerMock.Setup(x => x.NextResult()).Returns(() =>
            {
                resultSetIndex++;
                if (resultSetIndex < readerResults.Count)
                {
                    readerResults[resultSetIndex](readerMock);
                    return true;
                }
                return false;
            });

            readerResults[0](readerMock);
            
            objectDbMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(readerMock.Object);
            _dbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(objectDbMock.Object);
            
            var result = await _playerBettingStatisticsService.GetPlayerBettingStatistics(metaModel);
            
            Assert.Equal(expectedResult.WonTicketCount, result.WonTicketCount);
            Assert.Equal(expectedResult.LoseTicketCount, result.LoseTicketCount);
            Assert.Equal(expectedResult.DrawTicketCount, result.DrawTicketCount);
            Assert.Equal(expectedResult.CancelledTicketCount, result.CancelledTicketCount);
            Assert.Equal(expectedResult.Margin, result.Margin);
            Assert.Equal(expectedResult.AverageBetSize, result.AverageBetSize);
            Assert.Equal(expectedResult.Winnings, result.Winnings);
            Assert.Equal(expectedResult.Loss, result.Loss);
        }
    }
}
