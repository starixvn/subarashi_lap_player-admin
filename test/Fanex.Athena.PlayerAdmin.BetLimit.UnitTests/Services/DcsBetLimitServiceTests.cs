

using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class DcsBetLimitServiceTests
    {
        private IDcsBetLimitRepository _repository;
        private INewDcsBetLimitRepository _newRepository;
        private IDcsGameBetLimitRepository _gameBetLimitRepository;
        private IPublisher _publisher;

        [SetUp]
        public void Setup() 
        {
            _gameBetLimitRepository = Substitute.For<IDcsGameBetLimitRepository>();
            _repository = Substitute.For<IDcsBetLimitRepository>();
            _newRepository = Substitute.For<INewDcsBetLimitRepository>();
            _publisher= Substitute.For<IPublisher>();
        }

        [TestCase(ProductTypes.Sportsbook)]
        [TestCase(ProductTypes.NumberGame)]
        public async Task GetSettingAsync_WhenProductIsSportBookOrNumberGame_CallGameBetLimitRepo(int productId)
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var service = CreateService();

            //Act
            await service.GetSettingAsync(custId, productId);

            //Assert
            await _gameBetLimitRepository.Received(1).GetSettingAsync(custId, productId);
        }

        [Test]
        public async Task GetSettingAsync_WhenProductIsVS3_CallNewRepo()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var productId = ProductTypes.VS3;

            var service = CreateService();

            //Act
            await service.GetSettingAsync(custId, productId);

            //Assert
            await _newRepository.Received(1).GetSettingAsync(custId, productId);
        }

        [Test]
        public async Task GetSettingAsync_WhenProducrIsAllBet_CallDCSRepo()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var productId = ProductTypes.AllBet;

            var service = CreateService();

            //Act
            await service.GetSettingAsync(custId, productId);

            //Assert
            await _repository.Received(1).GetSettingAsync(custId, productId);
        }

        [Test]
        public async Task GetStatusAsync_WhenCallWithNotSupportProduct_ReturnNull()
        {
            //Arrange
            var notSupportProduct = ProductTypes.Sportsbook;
            var custId = FakeHelper.Create<int>();

            var service = CreateService();

            //Act
            var result = await service.GetStatusAsync(custId, notSupportProduct);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetStatusAsync_WhenCallWithSupportedProduct_CallRelevantMethod()
        {
            //Arrange
            var notSupportProduct = ProductTypes.Lottery;
            var custId = FakeHelper.Create<int>();

            var service = CreateService();

            //Act
            await service.GetStatusAsync(custId, notSupportProduct);

            //Assert
            await _repository.Received(1).GetStatusAsync(custId, notSupportProduct);
        }

        [TestCase(ProductTypes.Sportsbook)]
        [TestCase(ProductTypes.NumberGame)]
        public async Task UpdateSettingAsync_WhenProductIsSportBookOrNumberGame_ReturnCorrecResult(int prodcuctId)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = prodcuctId;
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _gameBetLimitRepository.UpdateSettingAsync(metaModel).Returns(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateSettingAsync(metaModel);

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task UpdateSettingAsync_WhenProductIsVS3_ReturnCorrecResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = ProductTypes.VS3;
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _newRepository.UpdateSettingAsync(metaModel).Returns(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateSettingAsync(metaModel);

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [TestCase(ProductTypes.Lottery)]
        [TestCase(ProductTypes.AllBet)]
        public async Task UpdateSettingAsync_WhenProductIsNormalDCS_ReturnCorrecResult(int productId)
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = productId;
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _repository.UpdateSettingAsync(metaModel).Returns(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateSettingAsync(metaModel);

            //Assert
            Assert.AreSame(expectedResult, result);
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = FakeHelper.Create<ResultInfo>();

            _repository.UpdateStatusAsync(metaModel).Returns(expectedResult);

            var service = CreateService();

            //Act
            var result = await service.UpdateStatusAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreSame(expectedResult, result);
                _publisher.Received(1).Publish(Arg.Any<PlayerUpdatedProductStatusEvent>());
            });
        }

        private DcsBetLimitService CreateService()
            => new DcsBetLimitService(_repository, _newRepository, _gameBetLimitRepository, _publisher);
    }
}
