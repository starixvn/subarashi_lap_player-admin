
using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class DcsBetLimitControllerTests
    {
        private IDcsBetLimitService _service;
        private IContextService _contextService;
        private IResourceService _resourceService;

        [SetUp]
        public void SetUp() 
        {
            _service = Substitute.For<IDcsBetLimitService>();
            _contextService= Substitute.For<IContextService>();
            _resourceService= Substitute.For<IResourceService>();
        }

        [Test]
        public async Task GetSetting_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitSelectMetaModel>();
            var betLimits = FakeHelper.CreateMany<PlayerBetLimit>();
            _contextService.IsInRoles(default, default).ReturnsForAnyArgs(true);
            _service.GetSettingAsync(metaModel.CustId, metaModel.ProductId).Returns(betLimits);
            _service.GetStatusAsync(metaModel.CustId, metaModel.ProductId).Returns(true);

            var controller = CreateController();

            //Act
            var result = await controller.GetSetting(metaModel);
            var model = result.Value as PlayerBetLimitViewModel;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(model);
                Assert.IsTrue(model.Editable);
                Assert.AreSame(betLimits, model.Data);
            });
        }

        [TestCase(0, "")]
        [TestCase(1, "Error")]
        public async Task UpdateSetting_WhenCall_ReturnCorrectResult(int code, string msg)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var expectedResult = new ResultInfo { Code= code, Message = msg };
            var expectedMsg = expectedResult.IsSuccessful ? ViewResources.UpdateSuccess : expectedResult.Message;
            _service.UpdateSettingAsync(metaModel).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateSetting(metaModel);
            var data = result.Value as IResultInfo;

            //
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedResult, data);
            Assert.AreEqual(expectedMsg, data.Message);
        }

        [TestCase(0, "")]
        [TestCase(1, "Error")]
        public async Task UpdateStatus_WhenCall_ReturnCorrectResult(int code, string msg)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = new ResultInfo { Code = code, Message = msg };
            var expectedMsg = expectedResult.IsSuccessful ? ViewResources.UpdateSuccess : expectedResult.Message;
            _service.UpdateStatusAsync(metaModel).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var result = await controller.UpdateStatus(metaModel);
            var data = result.Value as IResultInfo;

            //
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedResult, data);
            Assert.AreEqual(expectedMsg, data.Message);
        }

        private DcsBetLimitController CreateController()
            => new DcsBetLimitController(_contextService, _resourceService, _service);
    }
}
