using Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers;
using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Services;
using Fanex.Athena.PlayerAdmin.BatchUpdate.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.Extensions.Options;
using NPOI.OpenXmlFormats.Dml.Chart;
using NSubstitute;
using NSubstitute.Exceptions;
using NuGet.Frameworks;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Controllers
{
    public class PlayerBatchUpdateControllerTests
    {
        private IPlayerBatchUpdateService _service;
        private IContextService _contextService;
        private ILicenseeService _licenseeService;
        private IOptions<AppSetting> _options;
        private AppSetting _appSetting;

        [SetUp]
        public void Setup()
        {
            _service = Substitute.For<IPlayerBatchUpdateService>();
            _contextService= Substitute.For<IContextService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _options = Substitute.For<IOptions<AppSetting>>();
            _appSetting = FakeHelper.Create<AppSetting>();
            _options.Value.Returns(_appSetting);
        }

        [Test]
        public void UpdateBAStatus_WhenCallWithGetRequest_ReturnCorrectView()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBatchUpdateMetaModel>();
            var controller = CreateController();

            //Act
            var result = controller.UpdateBAStatus(metaModel);

            //Assert
            Assert.IsTrue(result.Model is BasePlayerBatchUpdateViewModel);
        }

        [Test]
        public async Task UpdateBAStatus_WhenIsAllowUpdateBA_CallUpdateBAMethod()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            _licenseeService.IsAllowUpdateBA(default, default, default).ReturnsForAnyArgs(true);
            var controller = CreateController();

            //Act
            await controller.UpdateBAStatus(_contextService, _licenseeService, _options, metaModel);

            //Assert
            await _service.Received(1).UpdateBAStatus(metaModel);
        }

        [Test]
        public async Task UpdateBAStatus_WhenIsNotAllowUpdateBA_ReturnFalseResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateBAStatusApiMetaModel>();
            _licenseeService.IsAllowUpdateBA(default, default, default).ReturnsForAnyArgs(false);
            var controller = CreateController();

            //Act
            var result = await controller.UpdateBAStatus(_contextService, _licenseeService, _options, metaModel);

            //Assert
            Assert.IsFalse(result.Value.GetPropertyValue<bool>("IsSuccessful"));
        }

        [Test]
        public void UpdateOddsGroup_WhenCallWithGetRequest_ReturnConrrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBatchUpdateMetaModel>();
            var expectedOfferedOddsGroupList = FakeHelper.CreateMany<string>();
            _contextService.OfferedOddsGroupList.Returns(expectedOfferedOddsGroupList);
            var controller = CreateController();

            //Act
            var result = controller.UpdateOddsGroup(metaModel, _contextService);

            //Assert
            Assert.IsTrue(result.Model is BasePlayerBatchUpdateViewModel);
            Assert.AreSame(expectedOfferedOddsGroupList, controller.ViewBag.OddsGroups);
        }

        [Test]
        public async Task UpdateOddsGroup_WhenNotValidOddGroup_ReturnFalsResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            _contextService.OfferedOddsGroupList.Returns(Enumerable.Empty<string>());
            var controller = CreateController();

            //Act
            var json = await controller.UpdateOddsGroup(metaModel, _contextService);
            var result = json.Value as ResultInfo;

            //Assert
            Assert.AreEqual(1, result.Code);
            Assert.AreEqual(ViewResources.FunctionDeniedMsg, result.Message);
        }

        [Test]
        public async Task UpdateOddsGroup_WhenValidOddGroup_CallUpdateMethod()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateOddsGroupApiMetaModel>();
            var oddGroups = FakeHelper.CreateMany<string>();
            metaModel.OddsGroupId = oddGroups.First();
            _contextService.OfferedOddsGroupList.Returns(oddGroups);
            var controller = CreateController();

            //Act
            await controller.UpdateOddsGroup(metaModel, _contextService);

            //Assert
            await _service.Received(1).UpdateOddsGroup(metaModel);
        }

        [Test]
        public void UpdateSpecialLevel_WhenCallFromGetRequest_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBatchUpdateMetaModel>();
            var controller = CreateController();

            //Act
            var result = controller.UpdateSpecialLevel(metaModel);

            //Assert
            Assert.IsTrue(result.Model is BasePlayerBatchUpdateViewModel);
        }

        [Test]
        public async Task UpdateSpecialLevel_WhenCallFromApi_CallUpdateMethod()
        {
            //Arrange
            var metaModel = FakeHelper.Create<UpdateSpecialLevelApiMetaModel>();
            var controller = CreateController();

            //Act
            await controller.UpdateSpecialLevel(metaModel);

            //Assert
            await _service.Received(1).UpdateSpecialLevel(metaModel);
        }

        private PlayerBatchUpdateController CreateController()
            => new PlayerBatchUpdateController(_service);
    }
}
