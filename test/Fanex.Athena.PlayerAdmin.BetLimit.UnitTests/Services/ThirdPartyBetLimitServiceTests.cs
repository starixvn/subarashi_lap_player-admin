

using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using MediatR;
using NPOI.OpenXmlFormats.Shared;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class ThirdPartyBetLimitServiceTests
    {
        private IThirdPartyBetLimitRepository _repository;
        private IPublisher _publisher;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<IThirdPartyBetLimitRepository>();
            _publisher= Substitute.For<IPublisher>();
        }

        [Test]
        public async Task GetSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var productId = FakeHelper.Create<int>();
            var expecterdResult = FakeHelper.Create<ThirdPartyBetSetting>();
            _repository.GetSettingAsync(custId, productId).Returns(expecterdResult);

            var service = CreateService();

            //Act
            var result = await service.GetSettingAsync(custId, productId);

            //Assert
            Assert.AreSame(expecterdResult, result);
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_ResultCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _repository.UpdateStatusAsync(metaModel).Returns(expectedResult);

            var service = CreateService();
            
            //Act
            var result = await service.UpdateStatusAsync(metaModel);

            //Assert
            Assert.AreSame(expectedResult, result);
            await _publisher.Received(1).Publish(Arg.Any<PlayerUpdatedProductStatusEvent>());
        }

        private ThirdPartyBetLimitService CreateService()
            => new ThirdPartyBetLimitService( _repository, _publisher );
    }
}
