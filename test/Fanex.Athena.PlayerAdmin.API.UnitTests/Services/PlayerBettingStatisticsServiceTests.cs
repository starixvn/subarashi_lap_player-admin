using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    public class PlayerBettingStatisticsServiceTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _reader;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _reader = Substitute.For<IDataReader>();
        }

        [Test]
        public async Task GetPlayerBettingStatistics_WhenCall_CallAndReturnCorrectly()
        {
            //Arrange
            _dbFactory.GetObjectDb("Lrf_PlayerBettingStatistics").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_reader);
            var model = FakeHelper.Create<PlayerBettingStatisticsMetaModel>();
            var expectedResult = FakeHelper.Create<PlayerBettingStatistics>();
            _reader.Read().Returns(true, true, false);
            _reader.NextResult().Returns(true, false);
            SetUpDataReturnFromReader(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.GetPlayerBettingStatistics(model);

            //Assert
            Assert.Multiple(() =>
            {
                AssertReceiveCallSetUpParameters(model);
                AssertCorrectResult(expectedResult, result);
            });

        }

        private PlayerBettingStatisticsService CreateService()
            => new PlayerBettingStatisticsService(_dbFactory);

        private void SetUpDataReturnFromReader(PlayerBettingStatistics expectedResult)
        {
            _reader["Won"].Returns(expectedResult.WonTicketCount);
            _reader["Lose"].Returns(expectedResult.LoseTicketCount);
            _reader["Draw"].Returns(expectedResult.DrawTicketCount);
            _reader["Cancelled"].Returns(expectedResult.CancelledTicketCount);
            _reader["Margin"].Returns(expectedResult.Margin);
            _reader["AverageBetSize"].Returns(expectedResult.AverageBetSize);
            _reader["Winnings"].Returns(expectedResult.Winnings);
            _reader["Loss"].Returns(expectedResult.Loss);
        }

        private void AssertReceiveCallSetUpParameters(PlayerBettingStatisticsMetaModel model)
        {
            _objectDb.Received().SetParameterValue("CustID", model.CustId);
            _objectDb.Received().SetParameterValue("FromDate", model.FromDate);
            _objectDb.Received().SetParameterValue("ToDate", model.ToDate);
            _objectDb.Received().SetParameterValue("MoveBetsDay", model.MoveBetsDay);
            _objectDb.Received().SetParameterValue("BalanceUpDay", model.BalanceUpDay);
            _objectDb.Received().SetParameterValue("ProductList", model.ProductCodeList);
        }

        private void AssertCorrectResult(PlayerBettingStatistics expectedResult, PlayerBettingStatistics result)
        {
            Assert.AreEqual(expectedResult.WonTicketCount, result.WonTicketCount);
            Assert.AreEqual(expectedResult.LoseTicketCount, result.LoseTicketCount);
            Assert.AreEqual(expectedResult.DrawTicketCount, result.DrawTicketCount);
            Assert.AreEqual(expectedResult.CancelledTicketCount, result.CancelledTicketCount);
            Assert.AreEqual(expectedResult.Margin, result.Margin);
            Assert.AreEqual(expectedResult.AverageBetSize, result.AverageBetSize);
            Assert.AreEqual(expectedResult.Winnings, result.Winnings);
            Assert.AreEqual(expectedResult.Loss, result.Loss);
        }
    }
}
