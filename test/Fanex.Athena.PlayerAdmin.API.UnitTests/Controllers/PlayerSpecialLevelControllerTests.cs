using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.Utilities;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NPOI.Util;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerSpecialLevelControllerTests
    {
        private IPlayerSpecialLevelService _playerSpecialLevelService;
        private ILicenseeService _licenseeService;
        private IContextService _contextService;
        private ILogger<PlayerSpecialLevelController> _logger;

        [SetUp]
        public void SetUp()
        {
            _playerSpecialLevelService = Substitute.For<IPlayerSpecialLevelService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _contextService = Substitute.For<IContextService>();
            _logger = Substitute.For<ILogger<PlayerSpecialLevelController>>();
        }

        [Test]
        public async Task Update_WhenIsFakeLicensee_ReturnFunctionDeniedResult()
        {
            //Arrange
            var model = FakeHelper.Create<SpecialLevelUpdatingMetaModel>();
            _contextService.IsFakeLicensee.Returns(true);

            var controller = CreateController();

            //Act
            var result = await controller.Update(model) as ObjectResult;
            var value = result.Value as ResultViewModel;

            //Assert
            Assert.NotNull(value);
            Assert.IsFalse(value.Success);
            Assert.AreEqual(ResourceKeyConstants.FunctionDenied, value.ErrorMessage);
        }

        [Test]
        public async Task Update_WhenServiceWorkNormal_ReturnCorrectResult()
        {
            //Arrange
            var model = FakeHelper.Create<SpecialLevelUpdatingMetaModel>();
            _contextService.IsFakeLicensee.Returns(false);
            var expectedResult = FakeHelper.Create<ResultViewModel>();
            _playerSpecialLevelService.Update(model).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var result = await controller.Update(model) as ObjectResult;
            var value = result.Value as ResultViewModel;

            //Assert
            Assert.NotNull(value);
            Assert.AreSame(expectedResult, value);
        }

        [Test]
        public async Task Update_WhenServiceThrowException_ReturnFunctionDeniedResult()
        {
            //Arrange
            var model = FakeHelper.Create<SpecialLevelUpdatingMetaModel>();
            _contextService.IsFakeLicensee.Returns(false);
            _playerSpecialLevelService.Update(model).Throws(new System.Exception("test exception"));

            var controller = CreateController();

            //Act
            var result = await controller.Update(model) as ObjectResult;
            var value = result.Value as ResultViewModel;

            //Assert
            Assert.NotNull(value);
            Assert.IsFalse(value.Success);
            Assert.AreEqual(ResourceKeyConstants.FunctionDenied, value.ErrorMessage);
        }

        private PlayerSpecialLevelController CreateController()
            => new PlayerSpecialLevelController(_playerSpecialLevelService, _licenseeService, _contextService, _logger);
    }
}
