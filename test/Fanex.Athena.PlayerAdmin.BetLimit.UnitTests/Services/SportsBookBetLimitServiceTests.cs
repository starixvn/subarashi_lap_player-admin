using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class SportsBookBetLimitServiceTests
    {

        private IDcsGameBetLimitRepository _betSetting;
        private IPublisher _publisher;

        [SetUp]
        public void Setup()
        {
            _betSetting = Substitute.For<IDcsGameBetLimitRepository>();
            _publisher= Substitute.For<IPublisher>();
        }

        [Test]
        public async Task GetSettingAsync_WhenCall_ReturnAllBetLimitWithCorrectOrder()
        {
            //Arrange
            var betLimits = CreatePlayerBetLimits();
            var expectedFirstItem = betLimits.First(_ => _.SportType == SportType.Soccer.ToString() && _.BetType == BetType.DefaultBetType);
            var custId = FakeHelper.Create<int>();
            _betSetting.GetSettingAsync(custId, ProductTypes.Sportsbook).Returns(betLimits);

            var service = CreateService();

            //Act
            var result = await service.GetSettingAsync(custId);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(betLimits.Count(), result.Count());
                Assert.AreSame(expectedFirstItem, result.First());
            });
        }

        [Test]
        public async Task UpdateSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _betSetting.UpdateSettingAsync(metaModel).Returns(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateSettingAsync(metaModel);

            //Assert
            Assert.AreSame(expectedResult, result);
            await _publisher.Received(1).Publish(Arg.Any<PlayerUpdatedBetLimitEvent>());
        }

        private SportsBookBetLimitService CreateService()
            => new SportsBookBetLimitService( _betSetting, _publisher );

        private IEnumerable<PlayerBetLimit> CreatePlayerBetLimits()
            => new List<PlayerBetLimit>
            {
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.Parlay},
                new PlayerBetLimit { SportType = SportType.SabaSoccer.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.Esports.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.SabaBasketBall.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.FootBall.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.Tennis.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.BaseBall.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.Golf.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.Motorsports.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.OtherSports.ToString(), BetType = BetType.DefaultBetType},
                new PlayerBetLimit { SportType = SportType.OtherSports.ToString(), BetType = BetType.Parlay}
            };
    }
}
