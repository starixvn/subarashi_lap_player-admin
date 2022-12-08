using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.Extensions.Options;
using NPOI.OpenXmlFormats;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerBaStatusControllerTests
    {
        private IPlayerBaStatusService _playerBaStatusService;
        private IContextService _contextService;
        private ILicenseeService _licenseeService;
        private IOptions<AppSetting> _options;
        private AppSetting _appSetting;

        [SetUp]
        public void Setup()
        {
            _playerBaStatusService = Substitute.For<IPlayerBaStatusService>();
            _contextService= Substitute.For<IContextService>();
            _licenseeService= Substitute.For<ILicenseeService>(); 
            _options= Substitute.For<IOptions<AppSetting>>();
            _appSetting= FakeHelper.Create<AppSetting>();
            _options.Value.Returns(_appSetting);
        }

        [Test]
        public async Task Update_WhenIsAllowUpdateBA_ReturnCorrectResult()
        {
            //Arrange
            var model = FakeHelper.Create<BaStatusUpdatingMetaModel>();
            var expectedResult = FakeHelper.Create<ResultViewModel>();
            _playerBaStatusService.Update(model).Returns(expectedResult);

            _licenseeService.IsAllowUpdateBA(default, default, default).ReturnsForAnyArgs(true);

            var controller = CreateController();

            //Act
            var result = await controller.Update(_contextService, _licenseeService, _options, model);

            //Assert
            Assert.AreSame(expectedResult, result.Value);
        }

        [Test]
        public async Task Update_WhenNotAllowUpdateBA_ReturnFalseResult()
        {
            //Arrange
            var model = FakeHelper.Create<BaStatusUpdatingMetaModel>();
            _licenseeService.IsAllowUpdateBA(default, default, default).ReturnsForAnyArgs(false);

            var controller = CreateController();

            //Act
            var result = await controller.Update(_contextService, _licenseeService, _options, model);

            //Assert
            Assert.IsFalse(result.Value.Success);
            Assert.AreEqual(1, result.Value.ErrorCode);
        }

        private PlayerBaStatusController CreateController()
            => new PlayerBaStatusController(_playerBaStatusService);
    }
}
