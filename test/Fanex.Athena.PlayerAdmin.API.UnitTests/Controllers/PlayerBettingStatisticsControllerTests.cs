
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerBettingStatisticsControllerTests
    {
        private IPlayerBettingStatisticsService _playerBettingStatisticsService;

        [SetUp]
        public void SetUp()
        {
            _playerBettingStatisticsService = Substitute.For<IPlayerBettingStatisticsService>();
        }

        [Test]
        public async Task GetData_WhenCall_CallRelevantMethod()
        {
            //Arrange
            var model = FakeHelper.Create<PlayerBettingStatisticsMetaModel>();

            var controller = CreateController();

            //Act
            await controller.GetData(model);

            //Assert
            await _playerBettingStatisticsService.Received().GetPlayerBettingStatistics(model);
        }

        private PlayerBettingStatisticsController CreateController()
            => new PlayerBettingStatisticsController(_playerBettingStatisticsService);
    }
}
