
using Fanex.Athena.PlayerAdmin.Commission.Controllers;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Controllers
{
    public partial class PlayerCommissionControllerTests
    {
        private IContextService _contextService;
        private ILicenseeService _licenseeService;
        private IPlayerCommissionService _playerCommService;

        [SetUp]
        public void Setup() 
        {
            _contextService = Substitute.For<IContextService>();
            _licenseeService= Substitute.For<ILicenseeService>();
            _playerCommService = Substitute.For<IPlayerCommissionService>();
        }

        [Test]
        public async Task PostMultipleMoveGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<MultipleMoveGroupQuery>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _playerCommService.MultipleMoveGroup(query).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var jsonResult = await controller.PostMultipleMoveGroup(query);
            var result = jsonResult.Value as IResultInfo;

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task PostMultipleUpdateGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<MultipleUpdateGroupQuery>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _playerCommService.MultipleUpdateGroup(query).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var jsonResult = await controller.PostMultipleUpdateGroup(query);
            var result = jsonResult.Value as IResultInfo;

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task PostMultipleRemoveFromGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<MultipleRemoveFromGroupQuery>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _playerCommService.MultipleRemoveFromGroup(query).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var jsonResult = await controller.PostMultipleRemoveFromGroup(query);
            var result = jsonResult.Value as IResultInfo;

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task MultipleMoveGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<MultipleMoveGroupQuery>();
            var expectedResult = FakeHelper.CreateMany<PlayerCommissionViewModel>();
            _playerCommService.GetAllCommGroups(query).Returns(expectedResult);

            var controller = CreateController();

            //Act
            var viewResult = await controller.MultipleMoveGroup(query);
            var result = viewResult.Model as MultipleMoveGroupViewModel;

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedResult.Count(), result.Groups.Count());
        }

        [Test]
        public async Task MultipleRemoveFromGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<MultipleRemoveFromGroupQuery>();

            var controller = CreateController();

            //Act
            var viewResult = await controller.MultipleRemoveFromGroup(query);
            var result = viewResult.Model as MultipleRemoveFromGroupViewModel;

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task MultipleUpdateGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<MultipleUpdateGroupQuery>();

            var controller = CreateController();

            //Act
            var viewResult = await controller.MultipleUpdateGroup(query);
            var result = viewResult.Model as MultipleUpdateGroupViewModel;

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task SingleMoveGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<SingleMoveGroupQuery>();
            var groups = FakeHelper.CreateMany<PlayerCommissionViewModel>();
            var currentGroup = FakeHelper.Create<PlayerCommissionViewModel>();

            _playerCommService.GetAllCommGroups(query).Returns(groups);
            _playerCommService.GetPlayerCommissionGroup(query).Returns(currentGroup);
            _contextService.IsInRoles(Roles.SuperAdmin, Roles.CommissionGroup_Write).Returns(true);

            var controller = CreateController();

            //Act
            var viewResult = await controller.SingleMoveGroup(query);
            var result = viewResult.Model as SingleMoveGroupViewModel;

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task SingleRemoveFromGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<SingleRemoveFromGroupQuery>();

            var controller = CreateController();

            //Act
            var viewResult = await controller.SingleRemoveFromGroup(query);
            var result = viewResult.Model as MultipleRemoveFromGroupViewModel;

            //Assert
            Assert.NotNull(result);
        }

        [Test]
        public async Task SingleUpdateGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            FakeGetProductPermission();
            var query = FakeHelper.Create<SingleUpdateGroupQuery>();
            var groups = FakeHelper.CreateMany<PlayerCommissionViewModel>();
            var currentGroup = FakeHelper.Create<PlayerCommissionViewModel>();

            _playerCommService.GetAllCommGroups(query).Returns(groups);
            _playerCommService.GetPlayerCommissionGroup(query).Returns(currentGroup);

            var controller = CreateController();

            //Act
            var viewResult = await controller.SingleUpdateGroup(query);
            var result = viewResult.Model as SingleMoveGroupViewModel;

            //Assert
            Assert.NotNull(result);
        }

        private void FakeGetProductPermission()
        {
            var products = FakeHelper.CreateMany<Product>().ToList();
            var supportedProduct = FakeHelper.CreateMany<string>().ToArray();
            _licenseeService.GetProductAccess(default, default).ReturnsForAnyArgs(new ProductAccess(products));
            _licenseeService.GetAllowedProductsByFunction(default).ReturnsForAnyArgs(supportedProduct);
        }

        private PlayerCommissionController CreateController()
            => new PlayerCommissionController(_contextService, _licenseeService, _playerCommService);
    }
}
