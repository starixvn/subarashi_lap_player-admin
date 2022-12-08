using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class AGCasinoBetLimitControllerTests
    {
        private IAGCasinoBetLimitService _service;
        private IResourceService _resourceService;
        private IContextService _contextService;

        [SetUp]
        public void SetUp()
        {
            _service = Substitute.For<IAGCasinoBetLimitService>();
            _resourceService = Substitute.For<IResourceService>();
            _contextService = Substitute.For<IContextService>();
        }

        [Test]
        public async Task GetSetting_WhenCall_ReturnCorrecResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitSelectMetaModel>();
            var groupBetting = FakeHelper.CreateMany<AgCasinoGame>().ToList();
            var betSetting = FakeHelper.Create<AgCasinoBetSetting>();
            betSetting.IsEnabled = betSetting.IsLineEnabled = true;
            _contextService.IsInRoles(default, default).ReturnsForAnyArgs(true);
            _service.GetGroupBettingsAsync(default, default).ReturnsForAnyArgs(groupBetting);
            _service.GetBetSettingAsync(default).ReturnsForAnyArgs(betSetting);

            var controller = CreateController();

            //Act
            var result = await controller.GetSetting(metaModel);
            var data = result.Value;

            //Assert
            Assert.AreEqual(groupBetting, data.GetPropertyValue<IList<AgCasinoGame>>("Data"));
            Assert.AreEqual(betSetting, data.GetPropertyValue<AgCasinoBetSetting>("BetSetting"));
            Assert.IsTrue(data.GetPropertyValue<bool>("Editable"));

        }

        [TestCase(0, "")]
        [TestCase(1, "Error")]
        public async Task UpdateStatus_WhenCall_ReturnCorrecValue(int code, string message)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var updateResult = new ResultInfo { Code = code, Message = message };
            _service.UpdateStatusAsync(metaModel).Returns(updateResult);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateStatus(metaModel);
            var data = result.Value as ResultInfo;

            //Assert
            Assert.IsNotNull(data);
            Assert.AreSame(updateResult, data);
        }

        [Test]
        public async Task UpdateSetting_WhenNotEnableBetSetting_ReturnFunctionDeniedResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            var betSetting = FakeHelper.Create<AgCasinoBetSetting>();
            betSetting.IsEnabled = false;
            _service.GetBetSettingAsync(metaModel.CustId).Returns(betSetting);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateSetting(metaModel);
            var data = result.Value as ResultInfo;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(data);
                Assert.IsFalse(data.IsSuccessful);
                Assert.AreEqual(ViewResources.FunctionDeniedMsg, data.Message);
            });
        }

        [TestCase(0, "")]
        [TestCase(1, "Error")]
        public async Task UpdateSetting_WhenEnableBetSetting_ReturnCorrectResult(int code, string msg)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            var betSetting = FakeHelper.Create<AgCasinoBetSetting>();
            var updateResult = new ResultInfo { Code = code, Message = msg };
            betSetting.IsEnabled = true;
            _service.GetBetSettingAsync(metaModel.CustId).Returns(betSetting);
            _service.UpdateSettingAsync(metaModel).Returns(updateResult);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateSetting(metaModel);
            var data = result.Value as ResultInfo;

            //Assert
            Assert.AreSame(updateResult, data);
        }


        private AGCasinoBetLimitController CreateController()
            => new AGCasinoBetLimitController(_contextService, _resourceService, _service);
    }
}
