namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    using AutoFixture;
    using BetLimit.DTO;
    using BetLimit.Mediator;
    using BetLimit.Repositories.Interfaces;
    using BetLimit.Services;
    using Core.Constants;
    using Core.Models;
    using MediatR;
    using MetaModel;
    using Moq;

    public class DcsBetLimitServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IDcsGameBetLimitRepository> _gameBetLimitRepositoryMock;
        private readonly Mock<INewDcsBetLimitRepository> _newRepositoryMock;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly Mock<IDcsBetLimitRepository> _repositoryMock;
        private readonly DcsBetLimitService _service;

        public DcsBetLimitServiceTests()
        {
            _fixture = new Fixture();
            _repositoryMock = new Mock<IDcsBetLimitRepository>();
            _newRepositoryMock = new Mock<INewDcsBetLimitRepository>();
            _gameBetLimitRepositoryMock = new Mock<IDcsGameBetLimitRepository>();
            _publisherMock = new Mock<IPublisher>();

            _service = new DcsBetLimitService(_repositoryMock.Object, _newRepositoryMock.Object, _gameBetLimitRepositoryMock.Object, _publisherMock.Object);
        }

        [Theory]
        [InlineData(ProductTypes.Sportsbook)]
        [InlineData(ProductTypes.NumberGame)]
        public async Task GetSettingAsync_ShouldUseGameBetLimitRepository_WhenSportsbookOrNumberGameProduct(int productId)
        {
            // Arrange
            const int custId = 123;
            var expectedData = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _gameBetLimitRepositoryMock.Setup(x => x.GetSettingAsync(custId, productId)).ReturnsAsync(expectedData);

            // Act
            var result = await _service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedData, result);
            _gameBetLimitRepositoryMock.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldUseNewRepository_WhenVS3Product()
        {
            // Arrange
            const int custId = 123;
            const int productId = ProductTypes.VS3;
            var expectedData = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _newRepositoryMock.Setup(x => x.GetSettingAsync(custId, productId)).ReturnsAsync(expectedData);

            // Act
            var result = await _service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedData, result);
            _newRepositoryMock.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Theory]
        [InlineData(ProductTypes.Lottery)]
        [InlineData(ProductTypes.I4D)]
        [InlineData(ProductTypes.SLT)]
        public async Task GetSettingAsync_ShouldUseRepository_WhenOtherProducts(int productId)
        {
            // Arrange
            const int custId = 123;
            var expectedData = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _repositoryMock.Setup(x => x.GetSettingAsync(custId, productId)).ReturnsAsync(expectedData);

            // Act
            var result = await _service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedData, result);
            _repositoryMock.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Theory]
        [InlineData(ProductTypes.Lottery)]
        [InlineData(ProductTypes.I4D)]
        [InlineData(ProductTypes.Exchange)]
        public async Task GetStatusAsync_ShouldReturnStatus_WhenProductHasControlStatus(int productId)
        {
            // Arrange
            const int custId = 123;
            var expectedStatus = new PlayerBetLimitProductSetting { IsDisabled = true, SportType = 220, BetType = 1021 };

            _repositoryMock.Setup(x => x.GetStatusAsync(custId, productId)).ReturnsAsync(expectedStatus);

            // Act
            var result = await _service.GetStatusAsync(custId, productId);

            // Assert
            Assert.Equal(expectedStatus, result);
            _repositoryMock.Verify(x => x.GetStatusAsync(custId, productId), Times.Once);
        }

        [Theory]
        [InlineData(ProductTypes.Sportsbook)]
        [InlineData(ProductTypes.Keno)]
        public async Task GetStatusAsync_ShouldReturnEmptySetting_WhenProductHasNoControlStatus(int productId)
        {
            // Arrange
            const int custId = 123;

            // Act
            var result = await _service.GetStatusAsync(custId, productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.ProductId);
            Assert.Equal(0, result.SportType);
            Assert.Equal(0, result.BetType);
            Assert.Null(result.IsDisabled);
            _repositoryMock.Verify(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(ProductTypes.Sportsbook)]
        [InlineData(ProductTypes.NumberGame)]
        public async Task UpdateSettingAsync_ShouldUseGameBetLimitRepository_WhenSportsbookOrNumberGameProduct(int productId)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = productId;

            // Setup the setting with proper test data to ensure event is published
            var setting = new PlayerBetLimitMetaModel
            {
                Id = "1_1",
                IsUpdated = true,
                MinBet = 10,
                OldMinBet = 5, // Different from MinBet to trigger log event
                MaxBet = 1000,
                OldMaxBet = 1000
            };
            metaModel.Setting = new[] { setting };

            var expectedResult = new ResultInfo { Code = 0 };

            _gameBetLimitRepositoryMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(expectedResult, result);
            _gameBetLimitRepositoryMock.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldUseNewRepository_WhenVS3Product()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = ProductTypes.VS3;

            // Setup the setting with proper test data to ensure event is published
            var setting = new PlayerBetLimitMetaModel
            {
                Id = "1_1",
                IsUpdated = true,
                MinBet = 10,
                OldMinBet = 5, // Different from MinBet to trigger log event
                MaxBet = 1000,
                OldMaxBet = 1000
            };
            metaModel.Setting = new[] { setting };

            var expectedResult = new ResultInfo { Code = 0 };

            _newRepositoryMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(expectedResult, result);
            _newRepositoryMock.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(ProductTypes.Lottery)]
        [InlineData(ProductTypes.I4D)]
        [InlineData(ProductTypes.SLT)]
        public async Task UpdateSettingAsync_ShouldUseRepository_WhenOtherProducts(int productId)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = productId;

            // Setup the setting with proper test data to ensure event is published
            var setting = new PlayerBetLimitMetaModel
            {
                Id = "1_1",
                IsUpdated = true,
                MinBet = 10,
                OldMinBet = 5, // Different from MinBet to trigger log event
                MaxBet = 1000,
                OldMaxBet = 1000
            };
            metaModel.Setting = new[] { setting };

            var expectedResult = new ResultInfo { Code = 0 };

            _repositoryMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(expectedResult, result);
            _repositoryMock.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldNotPublishEvent_WhenUpdateFails()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = ProductTypes.Lottery;
            var expectedResult = new ResultInfo { Code = 1 };

            _repositoryMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(expectedResult, result);
            _publisherMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
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
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(ProductTypes.Lottery)]
        [InlineData(ProductTypes.Keno)]
        public void GetBetSettingAction_ShouldReturnLotteryActions_WhenLotteryOrKenoProduct(int productId)
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(11, result); // UpdatingMinBet
            Assert.Contains(12, result); // UpdatingMaxBet
            Assert.Contains(35, result); // UpdatingMaxBetPerDraw
        }

        [Fact]
        public void GetBetSettingAction_ShouldReturnNumberGameActions_WhenNumberGameProduct()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.NumberGame);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.Contains(11, result); // UpdatingMinBet
            Assert.Contains(12, result); // UpdatingMaxBet
            Assert.Contains(13, result); // UpdatingMaxPerMatch
            Assert.Contains(14, result); // UpdatingMaxPerBall
        }

        [Fact]
        public void GetBetSettingAction_ShouldReturnSportsbookActions_WhenSportsbookProduct()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.Sportsbook);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
            Assert.Contains(11, result); // UpdatingMinBet
            Assert.Contains(12, result); // UpdatingMaxBet
            Assert.Contains(13, result); // UpdatingMaxPerMatch
            Assert.Contains(2047, result); // UpdatingMaxPayoutPerMatch
        }

        [Fact]
        public void GetBetSettingAction_ShouldReturnVS3Actions_WhenVS3Product()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.VS3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(11, result); // UpdatingMinBet
            Assert.Contains(12, result); // UpdatingMaxBet
        }

        [Theory]
        [InlineData(ProductTypes.I4D)]
        [InlineData(ProductTypes.SLT)]
        public void GetBetSettingAction_ShouldReturnDefaultActions_WhenOtherProducts(int productId)
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(11, result); // UpdatingMinBet
            Assert.Contains(12, result); // UpdatingMaxBet
            Assert.Contains(13, result); // UpdatingMaxPerMatch
        }
    }
}