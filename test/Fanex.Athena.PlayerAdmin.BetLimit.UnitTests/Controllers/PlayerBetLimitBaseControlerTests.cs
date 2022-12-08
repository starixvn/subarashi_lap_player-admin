
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.MockClasses;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class PlayerBetLimitBaseControlerTests
    {
        private const string I4DId = "223";

        private IContextService _contextService;
        private IResourceService _resourceService;

        [SetUp]
        public void Setup()
        {
            _contextService = Substitute.For<IContextService>();
            _resourceService = Substitute.For<IResourceService>();
        }

        [Test]
        public void AddSettingTypeName_WhenBetTypeIsParlay_ReturnCorrectValue()
        {
            //Arrange
            var betLimit = FakeHelper.Create<PlayerBetLimit>();
            betLimit.BetType = BetType.Parlay;
            betLimit.SportType = SportType.Soccer.ToString();
            var expectedValue = ViewResources.SoccerParlay;

            var controller = CreateController();

            //Act
            var result = controller.Public_AddSettingTypeName(new List<PlayerBetLimit>() { betLimit });

            //Assert
            Assert.AreEqual(expectedValue, result.First().BetSettingType);
        }

        [Test]
        public void AddSettingTypeName_WhenBetTypeIsNotParlay_ReturnCorerctValue()
        {
            //Arrange
            var betLimit = FakeHelper.Create<PlayerBetLimit>();
            betLimit.BetType = BetType.DefaultBetType;
            betLimit.SportType = SportType.Soccer.ToString();
            var expectedSportName = "sport";
            _resourceService.GetSportName(default).ReturnsForAnyArgs(expectedSportName);

            var controller = CreateController();

            //Act
            var result = controller.Public_AddSettingTypeName(new List<PlayerBetLimit> { betLimit });

            //Assert
            Assert.That(result.First().BetSettingType, Contains.Substring(expectedSportName));
        }

        [Test]
        public void AddSettingTypeName_WhenBetTypeIsNotParlayAndSportyIsI4D_ReturnCorrectResult()
        {
            //Arrange
            var betLimit = FakeHelper.Create<PlayerBetLimit>();
            betLimit.BetType = BetType.DefaultBetType;
            betLimit.SportType = I4DId;
            var expectedSportName = "sport";
            _resourceService.GetBettype(betLimit.BetType).ReturnsForAnyArgs(expectedSportName);

            var controller = CreateController();

            //Act
            var result = controller.Public_AddSettingTypeName(new List<PlayerBetLimit> { betLimit });

            //Assert
            Assert.That(result.First().BetSettingType, Contains.Substring(expectedSportName));
        }

        [Test]
        public void GetParlaySettingTypeName_WhenIdIsSoccer_ReturnSoccerParlayTypeName()
        {
            //Arrange
            var soccerId = "1";
            var controller = CreateController();

            //Act
            var result = controller.Public_GetParlaySettingTypeName(soccerId);

            //Assert
            Assert.AreEqual(ViewResources.SoccerParlay, result);
        }

        [Test]
        public void GetParlaySettingTypeName_WhenIdIsOtherSport_ReturnMixSportParlayTypeName()
        {
            //Arrange
            var soccerId = "99";
            var controller = CreateController();

            //Act
            var result = controller.Public_GetParlaySettingTypeName(soccerId);

            //Assert
            Assert.AreEqual(ViewResources.MixSportsParlay, result);
        }

        [Test]
        public void GetParlaySettingTypeName_WhenIdIsNotSpecial_CallGetSportName()
        {
            //Arrange
            var id = "109";
            var controller = CreateController();

            //Act
            controller.Public_GetParlaySettingTypeName(id);

            //Assert
            _resourceService.Received(1).GetSportName(id);
        }

        [Test]
        public void GetSportName_WhenIdIsSabaSoccer_ReturnCorrectValue()
        {
            //Arrange
            var id = SportType.SabaSoccer.ToString();
            var controller = CreateController();

            //Act
            var result = controller.Public_GetSportName(id);

            //Assert
            _resourceService.Received(1).GetSportName(SportType.Soccer);
            Assert.That(result, Contains.Substring(ViewResources.Saba));
        }

        [Test]
        public void GetSportName_WhenIdIsSabaBasketBall_ReturnCorrectValue()
        {
            //Arrange
            var id = SportType.SabaBasketBall.ToString();
            var controller = CreateController();

            //Act
            var result = controller.Public_GetSportName(id);

            //Assert
            _resourceService.Received(1).GetSportName(SportType.BasketBall);
            Assert.That(result, Contains.Substring(ViewResources.Saba));
        }

        [Test]
        public void GetSportName_WhenIdIsNotSpecial_ReturnCorrectValue()
        {
            //Arrange
            var id = "100000";
            var controller = CreateController();

            //Act
            controller.Public_GetSportName(id);

            //Assert
            _resourceService.Received(1).GetSportName(id);
        }

        private PlayerBetLimitBaseControlerTestable CreateController()
            => new PlayerBetLimitBaseControlerTestable(_contextService, _resourceService);
    }
}
