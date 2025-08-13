namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    using AutoFixture;
    using BetLimit.DTO;
    using BetLimit.Mediator;
    using BetLimit.Repositories;
    using BetLimit.Services;
    using Core.Models;
    using MediatR;
    using MetaModel;
    using Moq;

    public class ThirdPartyBetLimitServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly Mock<IThirdPartyBetLimitRepository> _repositoryMock;
        private readonly ThirdPartyBetLimitService _service;

        public ThirdPartyBetLimitServiceTests()
        {
            _fixture = new Fixture();
            _repositoryMock = new Mock<IThirdPartyBetLimitRepository>();
            _publisherMock = new Mock<IPublisher>();

            _service = new ThirdPartyBetLimitService(_repositoryMock.Object, _publisherMock.Object);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldReturnBetSetting_WhenCalled()
        {
            // Arrange
            const int custId = 123;
            const int productId = 456;
            var expectedBetSetting = _fixture.Create<ThirdPartyBetSetting>();

            _repositoryMock.Setup(x => x.GetSettingAsync(custId, productId)).ReturnsAsync(expectedBetSetting);

            // Act
            var result = await _service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedBetSetting, result);
            _repositoryMock.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldReturnResult_WhenRepositoryReturnsResult()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = new ResultInfo { Code = 0 };

            _repositoryMock.Setup(x => x.UpdateStatusAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Equal(expectedResult, result);
            _repositoryMock.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldPublishEvent_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = new ResultInfo { Code = 0 };

            _repositoryMock.Setup(x => x.UpdateStatusAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateStatusAsync(metaModel);

            // Assert
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldPublishEventEvenWhenUpdateFails_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = new ResultInfo { Code = 1, Message = "Update failed" };

            _repositoryMock.Setup(x => x.UpdateStatusAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Equal(expectedResult, result);
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldCallRepositoryWithCorrectParameters_WhenCalled()
        {
            // Arrange
            const int custId = 123;
            const int productId = 456;
            var expectedBetSetting = _fixture.Create<ThirdPartyBetSetting>();

            _repositoryMock.Setup(x => x.GetSettingAsync(custId, productId)).ReturnsAsync(expectedBetSetting);

            // Act
            var result = await _service.GetSettingAsync(custId, productId);

            // Assert
            _repositoryMock.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldCallRepositoryWithCorrectParameters_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedResult = new ResultInfo { Code = 0 };

            _repositoryMock.Setup(x => x.UpdateStatusAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateStatusAsync(metaModel);

            // Assert
            _repositoryMock.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
        }
    }
}