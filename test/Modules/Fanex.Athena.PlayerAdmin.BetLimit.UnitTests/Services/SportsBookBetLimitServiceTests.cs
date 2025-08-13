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

    public class SportsBookBetLimitServiceTests
    {
        private readonly Mock<IDcsGameBetLimitRepository> _betSettingMock;
        private readonly IFixture _fixture;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly SportsBookBetLimitService _service;

        public SportsBookBetLimitServiceTests()
        {
            _fixture = new Fixture();
            _betSettingMock = new Mock<IDcsGameBetLimitRepository>();
            _publisherMock = new Mock<IPublisher>();

            _service = new SportsBookBetLimitService(_betSettingMock.Object, _publisherMock.Object);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldReturnAllSettings_WhenIsShowMixedSportIsTrue()
        {
            // Arrange
            const int custId = 123;
            const bool isShowMixedSport = true;
            var sbSettings = _fixture.CreateMany<PlayerBetLimit>(5).ToList();

            _betSettingMock.Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook)).ReturnsAsync(sbSettings);

            // Act
            var result = await _service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
            _betSettingMock.Verify(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldFilterMixParlaySports_WhenIsShowMixedSportIsFalse()
        {
            // Arrange
            const int custId = 123;
            const bool isShowMixedSport = false;
            var sbSettings = new List<PlayerBetLimit>
            {
                new() { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType },
                new() { SportType = SportType.OtherSports.ToString(), BetType = BetType.Parlay }, // Should be filtered
                new() { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType }
            };

            _betSettingMock.Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook)).ReturnsAsync(sbSettings);

            // Act
            var result = await _service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.DoesNotContain(result, x => x.SportType == SportType.OtherSports.ToString() && x.BetType == BetType.Parlay);
            _betSettingMock.Verify(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldReturnOrderedResults_WhenCalled()
        {
            // Arrange
            const int custId = 123;
            const bool isShowMixedSport = true;
            var sbSettings = new List<PlayerBetLimit>
            {
                new() { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType },
                new() { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType },
                new() { SportType = SportType.Tennis.ToString(), BetType = BetType.DefaultBetType }
            };

            _betSettingMock.Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook)).ReturnsAsync(sbSettings);

            // Act
            var result = await _service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            Assert.NotNull(result);
            // Results should be ordered according to SBBetLimitComparer
            Assert.Equal(SportType.Soccer.ToString(), result.First().SportType);
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldReturnResult_WhenRepositoryReturnsResult()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            const bool isShowMixedSport = true;
            var expectedResult = new ResultInfo { Code = 0 };

            _betSettingMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            Assert.Equal(expectedResult, result);
            _betSettingMock.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldPublishEvent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();

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

            const bool isShowMixedSport = true;
            var expectedResult = new ResultInfo { Code = 0 };

            _betSettingMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldPublishEvent_WhenUpdateFails()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();

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

            const bool isShowMixedSport = true;
            var expectedResult = new ResultInfo { Code = 1 };

            _betSettingMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    public class SBBetLimitComparerTests
    {
        private readonly SBBetLimitComparer _comparer;
        private readonly IFixture _fixture;

        public SBBetLimitComparerTests()
        {
            _fixture = new Fixture();
            _comparer = new SBBetLimitComparer();
        }

        [Fact]
        public void Compare_ShouldReturn1_WhenFirstItemIsNull()
        {
            // Arrange
            PlayerBetLimit x = null;
            var y = _fixture.Create<PlayerBetLimit>();

            // Act
            var result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_ShouldReturn1_WhenSecondItemIsNull()
        {
            // Arrange
            var x = _fixture.Create<PlayerBetLimit>();
            PlayerBetLimit y = null;

            // Act
            var result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_ShouldReturn1_WhenBothItemsAreNull()
        {
            // Arrange
            PlayerBetLimit x = null;
            PlayerBetLimit y = null;

            // Act
            var result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_ShouldReturn1_WhenFirstItemIdNotInOrderer()
        {
            // Arrange
            var x = new PlayerBetLimit { SportType = "999", BetType = BetType.DefaultBetType };
            var y = new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType };

            // Act
            var result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_ShouldReturn1_WhenSecondItemIdNotInOrderer()
        {
            // Arrange
            var x = new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType };
            var y = new PlayerBetLimit { SportType = "999", BetType = BetType.DefaultBetType };

            // Act
            var result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void Compare_ShouldReturnCorrectOrder_WhenBothItemsAreValid()
        {
            // Arrange
            var x = new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType }; // Order 0
            var y = new PlayerBetLimit { SportType = SportType.SabaSoccer.ToString(), BetType = BetType.DefaultBetType }; // Order 1

            // Act
            var result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(-1, result); // x comes before y
        }

        [Fact]
        public void Compare_ShouldReturnZero_WhenBothItemsHaveSameOrder()
        {
            // Arrange
            var x = new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType };
            var y = new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType };

            // Act
            var result = _comparer.Compare(x, y);

            // Assert
            Assert.Equal(0, result);
        }
    }
}