using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using MediatR;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class SportsBookBetLimitServiceTests
    {
        private readonly Mock<IDcsGameBetLimitRepository> _mockBetSettingRepository;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly Fixture _fixture;

        public SportsBookBetLimitServiceTests()
        {
            _mockBetSettingRepository = new Mock<IDcsGameBetLimitRepository>();
            _mockPublisher = new Mock<IPublisher>();
            _fixture = new Fixture();
        }

        private SportsBookBetLimitService CreateService()
        {
            return new SportsBookBetLimitService(_mockBetSettingRepository.Object, _mockPublisher.Object);
        }

        [Fact]
        public async Task GetSettingAsync_WithShowMixedSportTrue_ShouldReturnAllSettings()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var isShowMixedSport = true;

            var mockSettings = new List<PlayerBetLimit>
            {
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType },
                new PlayerBetLimit { SportType = SportType.OtherSports.ToString(), BetType = BetType.Parlay },
                new PlayerBetLimit { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType }
            };

            _mockBetSettingRepository
                .Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook))
                .ReturnsAsync(mockSettings);

            // Act
            var result = await service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.SportType == SportType.OtherSports.ToString() && x.BetType == BetType.Parlay);
            _mockBetSettingRepository.Verify(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_WithShowMixedSportFalse_ShouldFilterOutMixedSports()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var isShowMixedSport = false;

            var mockSettings = new List<PlayerBetLimit>
            {
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType },
                new PlayerBetLimit { SportType = SportType.OtherSports.ToString(), BetType = BetType.Parlay },
                new PlayerBetLimit { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType }
            };

            _mockBetSettingRepository
                .Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook))
                .ReturnsAsync(mockSettings);

            // Act
            var result = await service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.DoesNotContain(result, x => x.SportType == SportType.OtherSports.ToString() && x.BetType == BetType.Parlay);
            Assert.Contains(result, x => x.SportType == SportType.Soccer.ToString());
            Assert.Contains(result, x => x.SportType == SportType.BasketBall.ToString());
        }

        [Fact]
        public async Task GetSettingAsync_ShouldReturnOrderedResults()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var isShowMixedSport = true;

            var mockSettings = new List<PlayerBetLimit>
            {
                new PlayerBetLimit { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType },
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType },
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.Parlay }
            };

            _mockBetSettingRepository
                .Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook))
                .ReturnsAsync(mockSettings);

            // Act
            var result = await service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(3, resultList.Count);
            // Soccer should come first (order 0), then Soccer Parlay (order 2), then Basketball (order 4)
            Assert.Equal($"{SportType.Soccer}_{BetType.DefaultBetType}", resultList[0].Id);
            Assert.Equal($"{SportType.Soccer}_{BetType.Parlay}", resultList[1].Id);
            Assert.Equal($"{SportType.BasketBall}_{BetType.DefaultBetType}", resultList[2].Id);
        }

        [Fact]
        public async Task GetSettingAsync_WithCompleteOrderingTest_ShouldReturnCorrectOrder()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var isShowMixedSport = true;

            // Create settings in random order to test sorting
            var mockSettings = new List<PlayerBetLimit>
            {
                new PlayerBetLimit { SportType = SportType.OtherSports.ToString(), BetType = BetType.Parlay }, // Should be last (14)
                new PlayerBetLimit { SportType = SportType.Tennis.ToString(), BetType = BetType.DefaultBetType }, // Order 7
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType }, // Should be first (0)
                new PlayerBetLimit { SportType = SportType.SabaSoccer.ToString(), BetType = BetType.DefaultBetType }, // Order 1
                new PlayerBetLimit { SportType = SportType.Soccer.ToString(), BetType = BetType.Parlay }, // Order 2
                new PlayerBetLimit { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType }, // Order 4
            };

            _mockBetSettingRepository
                .Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook))
                .ReturnsAsync(mockSettings);

            // Act
            var result = await service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            var resultList = result.ToList();
            Assert.Equal(6, resultList.Count);
            
            // Verify the expected order based on SBBetLimitComparer
            Assert.Equal($"{SportType.Soccer}_{BetType.DefaultBetType}", resultList[0].Id); // Order 0
            Assert.Equal($"{SportType.SabaSoccer}_{BetType.DefaultBetType}", resultList[1].Id); // Order 1
            Assert.Equal($"{SportType.Soccer}_{BetType.Parlay}", resultList[2].Id); // Order 2
            Assert.Equal($"{SportType.BasketBall}_{BetType.DefaultBetType}", resultList[3].Id); // Order 4
            Assert.Equal($"{SportType.Tennis}_{BetType.DefaultBetType}", resultList[4].Id); // Order 7
            Assert.Equal($"{SportType.OtherSports}_{BetType.Parlay}", resultList[5].Id); // Order 14
        }

        [Fact]
        public async Task UpdateSettingAsync_WithSuccessfulUpdate_ShouldPublishLogEventOnce()
        {
            // Arrange
            var service = CreateService();
            var isShowMixedSport = _fixture.Create<bool>();

            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>(),
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        Id = $"{SportType.Soccer}_{BetType.DefaultBetType}",
                        SportType = SportType.Soccer.ToString(),
                        BetType = BetType.DefaultBetType,
                        MinBet = 10,
                        MaxBet = 1000,
                        OldMinBet = 5,
                        OldMaxBet = 500,
                        IsUpdated = true
                    }
                }
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockBetSettingRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            Assert.Equal(0, result.Code);
            Assert.Equal("Success", result.Message);
            _mockBetSettingRepository.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_WithFailedUpdate_ShouldPublishLogEventAndReturnEarly()
        {
            // Arrange
            var service = CreateService();
            var isShowMixedSport = _fixture.Create<bool>();

            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>(),
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        Id = $"{SportType.Soccer}_{BetType.DefaultBetType}",
                        SportType = SportType.Soccer.ToString(),
                        BetType = BetType.DefaultBetType,
                        MinBet = 10,
                        MaxBet = 1000,
                        OldMinBet = 5,
                        OldMaxBet = 500,
                        IsUpdated = true // Must have updated settings to generate log event
                    }
                }
            };

            var failureResult = new ResultInfo { Code = 1, Message = "Error occurred" };

            _mockBetSettingRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(failureResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            Assert.Equal(1, result.Code);
            Assert.Equal("Error occurred", result.Message);
            _mockBetSettingRepository.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_WithNoUpdatedSettings_ShouldNotPublishLogEvent()
        {
            // Arrange
            var service = CreateService();
            var isShowMixedSport = _fixture.Create<bool>();

            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>(),
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        Id = $"{SportType.Soccer}_{BetType.DefaultBetType}",
                        SportType = SportType.Soccer.ToString(),
                        BetType = BetType.DefaultBetType,
                        IsUpdated = false // No updates
                    }
                }
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockBetSettingRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            Assert.Equal(0, result.Code);
            _mockBetSettingRepository.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            // Should not publish event when no log event is created (no updated settings)
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Never);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateSettingAsync_ShouldPassIsShowMixedSportParameter(bool isShowMixedSport)
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                Setting = new List<PlayerBetLimitMetaModel>()
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };
            _mockBetSettingRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            await service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            // The isShowMixedSport parameter is used in the service logic but not passed to repository
            // This test verifies the method accepts the parameter without throwing
            _mockBetSettingRepository.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_WithEmptyResults_ShouldReturnEmptyCollection()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var isShowMixedSport = _fixture.Create<bool>();

            _mockBetSettingRepository
                .Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook))
                .ReturnsAsync(new List<PlayerBetLimit>());

            // Act
            var result = await service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateSettingAsync_WithMultipleUpdatedSettings_ShouldCreateCorrectLogEvent()
        {
            // Arrange
            var service = CreateService();
            var isShowMixedSport = _fixture.Create<bool>();

            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>(),
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        Id = $"{SportType.Soccer}_{BetType.DefaultBetType}",
                        SportType = SportType.Soccer.ToString(),
                        BetType = BetType.DefaultBetType,
                        MinBet = 10,
                        MaxBet = 1000,
                        MaxPerMatch = 5000,
                        MaxPayoutPerMatch = 10000,
                        OldMinBet = 5,
                        OldMaxBet = 500,
                        OldMaxPerMatch = 2500,
                        OldMaxPayoutPerMatch = 5000,
                        IsUpdated = true
                    },
                    new PlayerBetLimitMetaModel
                    {
                        Id = $"{SportType.BasketBall}_{BetType.DefaultBetType}",
                        SportType = SportType.BasketBall.ToString(),
                        BetType = BetType.DefaultBetType,
                        MinBet = 20,
                        OldMinBet = 10,
                        IsUpdated = true
                    }
                }
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockBetSettingRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            PlayerUpdatedBetLimitEvent capturedEvent = null;
            _mockPublisher
                .Setup(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default))
                .Callback<object, System.Threading.CancellationToken>((evt, _) => capturedEvent = evt as PlayerUpdatedBetLimitEvent);

            // Act
            await service.UpdateSettingAsync(metaModel, isShowMixedSport);

            // Assert
            Assert.NotNull(capturedEvent);
            Assert.Equal(metaModel.CustId, capturedEvent.CustID);
            Assert.Equal(metaModel.UserId, capturedEvent.UserId);
            Assert.Equal(metaModel.SiteName, capturedEvent.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, capturedEvent.LicenseeCustId);
            Assert.NotNull(capturedEvent.Detail);
            Assert.True(capturedEvent.Detail.Any()); // Should have log details for updated settings
        }

        [Fact]
        public async Task GetSettingAsync_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var service = CreateService();
            var custId = 12345;
            var isShowMixedSport = true;

            _mockBetSettingRepository
                .Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<PlayerBetLimit>());

            // Act
            await service.GetSettingAsync(custId, isShowMixedSport);

            // Assert
            _mockBetSettingRepository.Verify(
                x => x.GetSettingAsync(custId, ProductTypes.Sportsbook), 
                Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = 12345,
                Setting = new List<PlayerBetLimitMetaModel>()
            };

            _mockBetSettingRepository
                .Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>()))
                .ReturnsAsync(new ResultInfo { Code = 0 });

            // Act
            await service.UpdateSettingAsync(metaModel, true);

            // Assert
            _mockBetSettingRepository.Verify(
                x => x.UpdateSettingAsync(metaModel), 
                Times.Once);
        }
    }
} 