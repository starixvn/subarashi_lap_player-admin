using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Models;
using MediatR;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class ThirdPartyBetLimitServiceTests
    {
        private readonly Mock<IThirdPartyBetLimitRepository> _mockRepository;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly Fixture _fixture;

        public ThirdPartyBetLimitServiceTests()
        {
            _mockRepository = new Mock<IThirdPartyBetLimitRepository>();
            _mockPublisher = new Mock<IPublisher>();
            _fixture = new Fixture();
        }

        private ThirdPartyBetLimitService CreateService()
        {
            return new ThirdPartyBetLimitService(_mockRepository.Object, _mockPublisher.Object);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldCallRepositoryAndReturnResult()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var productId = _fixture.Create<int>();
            
            var expectedSetting = new ThirdPartyBetSetting
            {
                IsEnabled = _fixture.Create<bool>(),
                IsLineEnabled = _fixture.Create<bool>(),
                CurrencyId = _fixture.Create<int>()
            };

            _mockRepository
                .Setup(x => x.GetSettingAsync(custId, productId))
                .ReturnsAsync(expectedSetting);

            // Act
            var result = await service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedSetting, result);
            Assert.Equal(expectedSetting.IsEnabled, result.IsEnabled);
            Assert.Equal(expectedSetting.IsLineEnabled, result.IsLineEnabled);
            Assert.Equal(expectedSetting.CurrencyId, result.CurrencyId);
            
            _mockRepository.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_WithSpecificValues_ShouldReturnCorrectData()
        {
            // Arrange
            var service = CreateService();
            var custId = 12345;
            var productId = 67890;
            
            var expectedSetting = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = false,
                CurrencyId = 101
            };

            _mockRepository
                .Setup(x => x.GetSettingAsync(custId, productId))
                .ReturnsAsync(expectedSetting);

            // Act
            var result = await service.GetSettingAsync(custId, productId);

            // Assert
            Assert.True(result.IsEnabled);
            Assert.False(result.IsLineEnabled);
            Assert.Equal(101, result.CurrencyId);
            
            _mockRepository.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WithSuccessfulUpdate_ShouldPublishEventAndReturnResult()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = _fixture.Create<bool>(),
                OldStatus = _fixture.Create<bool>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>(),
                Ip = _fixture.Create<string>(),
                Language = _fixture.Create<string>()
            };

            var successResult = new ResultInfo
            {
                Code = 0,
                Message = _fixture.Create<string>()
            };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Equal(successResult.Code, result.Code);
            Assert.Equal(successResult.Message, result.Message);
            
            _mockRepository.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WithFailedUpdate_ShouldStillPublishEventAndReturnResult()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = _fixture.Create<bool>(),
                OldStatus = _fixture.Create<bool>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>(),
                Ip = _fixture.Create<string>(),
                Language = _fixture.Create<string>()
            };

            var failureResult = new ResultInfo
            {
                Code = 1,
                Message = "Update failed"
            };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(failureResult);

            // Act
            var result = await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Equal(1, result.Code);
            Assert.Equal("Update failed", result.Message);
            
            _mockRepository.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldPublishCorrectLogEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = 12345,
                UserId = 67890,
                ProductId = 111,
                Status = true,
                OldStatus = false,
                SiteName = "TestSite",
                LicenseeCustId = 99999,
                Ip = "192.168.1.1",
                Language = "en"
            };

            var successResult = new ResultInfo
            {
                Code = 0,
                Message = "Success"
            };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(successResult);

            PlayerUpdatedProductStatusEvent capturedEvent = null;
            _mockPublisher
                .Setup(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default))
                .Callback<object, System.Threading.CancellationToken>((evt, _) => capturedEvent = evt as PlayerUpdatedProductStatusEvent);

            // Act
            await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.NotNull(capturedEvent);
            Assert.Equal(metaModel.CustId, capturedEvent.CustID);
            Assert.Equal(metaModel.UserId, capturedEvent.UserId);
            Assert.Equal(metaModel.SiteName, capturedEvent.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, capturedEvent.LicenseeCustId);
            Assert.NotNull(capturedEvent.Detail);
            Assert.Equal(ProductStatusLogHelper.Success, capturedEvent.Detail.Status);
            Assert.Equal(metaModel.ProductId.ToString(), capturedEvent.Detail.ProductId);
            Assert.Equal("Disable", capturedEvent.Detail.Old); // OldStatus = false -> "Disable"
            Assert.Equal("Enable", capturedEvent.Detail.New);  // Status = true -> "Enable"
        }

        [Fact]
        public async Task UpdateStatusAsync_WithFailedResult_ShouldPublishFailureLogEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = false,
                OldStatus = true,
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var failureResult = new ResultInfo
            {
                Code = 1,
                Message = "Error occurred"
            };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(failureResult);

            PlayerUpdatedProductStatusEvent capturedEvent = null;
            _mockPublisher
                .Setup(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default))
                .Callback<object, System.Threading.CancellationToken>((evt, _) => capturedEvent = evt as PlayerUpdatedProductStatusEvent);

            // Act
            await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.NotNull(capturedEvent);
            Assert.Equal(ProductStatusLogHelper.False, capturedEvent.Detail.Status);
            Assert.Equal("Enable", capturedEvent.Detail.Old);   // OldStatus = true -> "Enable"
            Assert.Equal("Disable", capturedEvent.Detail.New);  // Status = false -> "Disable"
        }

        [Theory]
        [InlineData(true, false, "Disable", "Enable")]
        [InlineData(false, true, "Enable", "Disable")]
        [InlineData(true, true, "Enable", "Enable")]
        [InlineData(false, false, "Disable", "Disable")]
        public async Task UpdateStatusAsync_ShouldMapStatusCorrectly(bool status, bool oldStatus, string expectedOld, string expectedNew)
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = status,
                OldStatus = oldStatus,
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var result = new ResultInfo { Code = 0, Message = "Success" };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(result);

            PlayerUpdatedProductStatusEvent capturedEvent = null;
            _mockPublisher
                .Setup(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default))
                .Callback<object, System.Threading.CancellationToken>((evt, _) => capturedEvent = evt as PlayerUpdatedProductStatusEvent);

            // Act
            await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.NotNull(capturedEvent);
            Assert.Equal(expectedOld, capturedEvent.Detail.Old);
            Assert.Equal(expectedNew, capturedEvent.Detail.New);
        }

        [Fact]
        public async Task GetSettingAsync_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var service = CreateService();
            var custId = 123;
            var productId = 456;

            var setting = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = true,
                CurrencyId = 1
            };

            _mockRepository
                .Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(setting);

            // Act
            await service.GetSettingAsync(custId, productId);

            // Assert
            _mockRepository.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_CallsRepositoryWithCorrectParameters()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = 123,
                UserId = 456,
                ProductId = 789,
                Status = true,
                OldStatus = false
            };

            var result = new ResultInfo { Code = 0 };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>()))
                .ReturnsAsync(result);

            // Act
            await service.UpdateStatusAsync(metaModel);

            // Assert
            _mockRepository.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_AlwaysPublishesEventRegardlessOfResult()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = _fixture.Create<bool>(),
                OldStatus = _fixture.Create<bool>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var result = new ResultInfo { Code = 999, Message = "Any result" };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(result);

            // Act
            await service.UpdateStatusAsync(metaModel);

            // Assert
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldReturnOriginalResultFromRepository()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = _fixture.Create<bool>(),
                OldStatus = _fixture.Create<bool>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var originalResult = new ResultInfo
            {
                Code = 42,
                Message = "Original message from repository"
            };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(originalResult);

            // Act
            var result = await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Same(originalResult, result);
            Assert.Equal(42, result.Code);
            Assert.Equal("Original message from repository", result.Message);
        }
    }
} 