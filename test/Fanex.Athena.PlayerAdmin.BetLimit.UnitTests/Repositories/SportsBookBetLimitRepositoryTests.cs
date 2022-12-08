
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.TestHelper;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    public class SportsBookBetLimitRepositoryTests
    {
        private const string MixedSportId = "99MP";
        private const string SoccerParlay = "1MP";

        private static readonly string[] SportTypes =
{
            SportType.Soccer.ToString(),
            SoccerParlay,
            SportType.BasketBall.ToString(),
            SportType.FootBall.ToString(),
            SportType.Tennis.ToString(),
            SportType.BaseBall.ToString(),
            SportType.Golf.ToString(),
            SportType.Motorsports.ToString(),
            SportType.OtherSports.ToString()
        };

        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _dataReader = Substitute.For<IDataReader>();
        }

        [Test]
        public async Task GetSettingAsync_WhenNotDataFromDB_ReturnEmptyResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var isShowMixedSport = FakeHelper.Create<bool>();
            _dbFactory.GetObjectDb("Lrf_CustomerSelEditCustomer").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.Read().Returns(false);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetSettingAsync(custId, isShowMixedSport);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetSettingAsync_WhenNotShowMixedSport_ReturnCorrectResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var isShowMixedSport = false;
            var playerBetLimit = FakeHelper.Create<PlayerBetLimit>();
            _dbFactory.GetObjectDb("Lrf_CustomerSelEditCustomer").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.Read().Returns(true, false);
            AssignPlayerBetLimitToReader(playerBetLimit);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetSettingAsync(custId, isShowMixedSport);

            //Assert
            Assert.AreEqual(SportTypes.Count(), result.Count());
        }

        [Test]
        public async Task GetSettingAsync_WhenIsShowMixedSport_ReturnCorrectResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var isShowMixedSport = true;
            var playerBetLimit = FakeHelper.Create<PlayerBetLimit>();
            _dbFactory.GetObjectDb("Lrf_CustomerSelEditCustomer").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.Read().Returns(true, false);
            AssignPlayerBetLimitToReader(playerBetLimit);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetSettingAsync(custId, isShowMixedSport);

            //Assert
            Assert.AreEqual(SportTypes.Count() + 1, result.Count());
        }

        [Test]
        public async Task UpdateSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var isShowMixedSport = true;
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _dbFactory.GetObjectDb("Lrf_CustomerUpdMember_All").Returns(_objectDb);
            _objectDb.AssignResultToParameter(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.UpdateSettingAsync(metaModel, isShowMixedSport);

            //Assert
            Assert.AreEqual(expectedResult.Code, result.Code);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }

        private SportsBookBetLimitRepository CreateRepo()
            => new SportsBookBetLimitRepository(_dbFactory);

        private void AssignPlayerBetLimitToReader(PlayerBetLimit playerBetLimits)
        {
            foreach (var item in SportTypes)
            {
                var suffix = GetSportSuffix(item);
                _dataReader["maxbet" + suffix].Returns(playerBetLimits.MaxBet);
                _dataReader["minbet" + suffix].Returns(playerBetLimits.MinBet);
                _dataReader["MaxPerMatch" + suffix].Returns(playerBetLimits.MaxPerMatch);
                _dataReader["MaxPerMatch" + suffix + "_Max"].Returns(playerBetLimits.MaxPerMatchMax);
                _dataReader["MaxPerMatch" + suffix + "_Min"].Returns(playerBetLimits.MaxPerMatchMin);
                _dataReader["MaxBet" + suffix + "_Min"].Returns(playerBetLimits.BetValueMax);
                _dataReader["MinBet" + suffix + "_Max"].Returns(playerBetLimits.BetValueMin);
            }

            var mixSportSuffix = GetSportSuffix(MixedSportId);
            _dataReader["maxbet" + mixSportSuffix].Returns(playerBetLimits.MaxBet);
            _dataReader["minbet" + mixSportSuffix].Returns(playerBetLimits.MinBet);
            _dataReader["MaxPerMatch" + mixSportSuffix].Returns(playerBetLimits.MaxPerMatch);
            _dataReader["MaxPerMatch" + mixSportSuffix + "_Max"].Returns(playerBetLimits.MaxPerMatchMax);
            _dataReader["MaxPerMatch" + mixSportSuffix + "_Min"].Returns(playerBetLimits.MaxPerMatchMin);
            _dataReader["MaxBet" + mixSportSuffix + "_Min"].Returns(playerBetLimits.BetValueMax);
            _dataReader["MinBet" + mixSportSuffix + "_Max"].Returns(playerBetLimits.BetValueMin);
        }

        private static string GetSportSuffix(string sportId) => sportId != SportType.Soccer.ToString() ? "v" + sportId : string.Empty;
    }
}
