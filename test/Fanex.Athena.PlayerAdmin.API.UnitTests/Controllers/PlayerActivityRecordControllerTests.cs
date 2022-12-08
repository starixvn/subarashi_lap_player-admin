using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerActivityRecordControllerTests
    {
        private IPlayerActivityRecordService _playerActivityRecordService;

        [SetUp]
        public void SetUp()
        {
            _playerActivityRecordService = Substitute.For<IPlayerActivityRecordService>();
        }

        [Test]
        public async Task Index_WhenNotValidCustId_ReturnNull()
        {
            //Arrange
            var model = FakeHelper.Create<PlayerActivityRecordMetaModel>();
            model.CustId = 0;

            var controller = CreateController();

            //Act
            var result = await controller.Index(model);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Index_WhenValidCustId_CallGetActivityRecodeMethod()
        {
            //Arrange
            var model = FakeHelper.Create<PlayerActivityRecordMetaModel>();
            model.CustId = 1;

            var controller = CreateController();

            //Act
            await controller.Index(model);

            //Assert
            await _playerActivityRecordService.Received(1).GetActivityRecord(model);
        }

        private PlayerActivityRecordController CreateController() 
            => new PlayerActivityRecordController(_playerActivityRecordService);
    }
}
