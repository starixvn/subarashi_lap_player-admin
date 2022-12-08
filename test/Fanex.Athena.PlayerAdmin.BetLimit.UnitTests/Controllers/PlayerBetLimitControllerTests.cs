

using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class PlayerBetLimitControllerTests
    {
        private IPlayerBetLimitRepository _playerBetLimitRepository;
        private IContextService _contextService;

        [SetUp]
        public void SetUp()
        {
            _playerBetLimitRepository = Substitute.For<IPlayerBetLimitRepository>();
            _contextService = Substitute.For<IContextService>();
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var productList = new List<PlayerBetLimitProduct>
            {
                new PlayerBetLimitProduct { ProductId = 1, ProductCode = FakeHelper.Create<string>()},
                new PlayerBetLimitProduct {ProductId = 2, ProductCode = ProductCode.MXG}
            };
            _playerBetLimitRepository.GetEnableBetLimitProducts(_contextService.SiteName).Returns(productList);

            var controller = CreateController();

            //Act
            var result = await controller.Index(_contextService, FakeHelper.Create<int>(), FakeHelper.Create<int>(), FakeHelper.Create<int>());
            var model = result.Model as PlayerBetLimitProductViewModel;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.NotNull(model);
                Assert.AreEqual(1, model.ProductId);
                Assert.AreEqual(2, model.ProductList.Keys.Count);
            });
        }

        private PlayerBetLimitController CreateController()
            => new PlayerBetLimitController(_playerBetLimitRepository);
    }
}
