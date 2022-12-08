

using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerListControllerTests
    {
        private IPlayerListService _playerListService;

        [SetUp]
        public void Setup() 
            => _playerListService = Substitute.For<IPlayerListService>();

        [Test]
        public async Task Search_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var model = FakeHelper.Create<PlayerSearchingMetaModel>();
            var expectedResult = FakeHelper.Create<PlayerListResultViewModel>();
            _playerListService.Search(model).Returns(expectedResult);
            var controller = CreateController();

            //Act
            var result = await controller.Search(model);

            //Assert
            Assert.AreSame(expectedResult, result.Value);
        }

        [Test]
        public async Task GetPlayerListById_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var model = FakeHelper.Create<PlayerListByIdMetaModel>();
            var expectedResult = FakeHelper.Create<PlayerListResultViewModel>();
            _playerListService.GetPlayerListById(model).Returns(expectedResult);
            var controller = CreateController();

            //Act
            var result = await controller.GetPlayerListById(model);

            //Assert
            Assert.AreSame(expectedResult, result.Value);
        }

        [Test]
        public async Task ArchivedSearch_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var model = FakeHelper.Create<PlayerArchivedSearchingMetaModel>();
            var expectedResult = FakeHelper.Create<PlayerListResultViewModel>();
            _playerListService.ArchivedSearch(model).Returns(expectedResult);
            var controller = CreateController();

            //Act
            var result = await controller.ArchivedSearch(model);

            //Assert
            Assert.AreSame(expectedResult, result.Value);
        }

        private PlayerListController CreateController()
            => new PlayerListController(_playerListService);
    }
}
