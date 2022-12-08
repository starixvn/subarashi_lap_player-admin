

using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class ThirdPartyBetLimitControllerTests
    {
        private IThirdPartyBetLimitService _service;
        private IResourceService _resourceService;
        private IContextService _contextService;

        [SetUp]
        public void SetUp()
        {
            _service = Substitute.For<IThirdPartyBetLimitService>();
            _resourceService= Substitute.For<IResourceService>();
            _contextService= Substitute.For<IContextService>();
        }

        [Test]
        public async Task GetSetting_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitSelectMetaModel>();
            var expectedSetting = FakeHelper.Create<ThirdPartyBetSetting>();
            _service.GetSettingAsync(metaModel.CustId, metaModel.ProductId).Returns(expectedSetting);

            var controller = CreateController();

            //Act
            var result = await controller.GetSetting(metaModel);
            var model = result.Value;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.AreEqual(expectedSetting.IsEnabled, model.StatusEnable);
            });
        }

        [TestCase(0, "")]
        [TestCase(1, "Error")]
        public async Task UpdateStatus_WhenCall_ReturnCorrectResult(int code, string msg)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedMsg = code == 0 ? ViewResources.UpdateSuccess : msg;
            var expectedResult = new ResultInfo { Code = code, Message = msg };
            _service.UpdateStatusAsync(metaModel).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateStatus(metaModel);
            var model = result.Value as ResultInfo;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.AreEqual(code, model.Code);
                Assert.AreEqual(expectedMsg, model.Message);
            });
        }

        private ThirdPartyBetLimitController CreateController()
            => new ThirdPartyBetLimitController(_contextService, _resourceService, _service);
    }
}
