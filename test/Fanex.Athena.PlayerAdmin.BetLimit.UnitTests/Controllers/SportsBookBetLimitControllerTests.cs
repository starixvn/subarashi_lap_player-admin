
using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class SportsBookBetLimitControllerTests
    {
        private ISportsBookBetLimitService _service;
        private IContextService _contextService;
        private IResourceService _resourceService;

        [SetUp]
        public void Setup()
        {
            _service = Substitute.For<ISportsBookBetLimitService>();
            _contextService= Substitute.For<IContextService>();
            _resourceService= Substitute.For<IResourceService>();
        }

        [Test]
        public async Task GetSetting_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitSelectMetaModel>();
            var expectedData = FakeHelper.CreateMany<PlayerBetLimit>();
            var firstData = expectedData.First();
            firstData.SportType = $"{SportType.Soccer}";
            firstData.BetType = BetType.DefaultBetType;
            _service.GetSettingAsync(metaModel.CustId).Returns(expectedData);

            var controller = CreateController();

            //Act
            var result = await controller.GetSetting(metaModel);
            var model = result.Value as PlayerBetLimitViewModel;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.IsTrue(model.EnableCopy);
                Assert.AreSame(expectedData, model.Data);
            });
        }

        [TestCase(0, "")]
        [TestCase(1, "Error")]
        public async Task UpdateSetting_WhenCall_ReturnCorrectResult(int code, string msg)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var expectedMsg = code == 0 ? ViewResources.UpdateSuccess : msg;
            var expectedResult = new ResultInfo { Code= code, Message = msg };
            _service.UpdateSettingAsync(metaModel).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateSetting(metaModel);
            var model = result.Value as ResultInfo;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.AreEqual(code, model.Code);
                Assert.AreEqual(expectedMsg, model.Message);
            });
        }

        private SportsBookBetLimitController CreateController()
            => new SportsBookBetLimitController(_contextService, _resourceService, _service);
    }
}
