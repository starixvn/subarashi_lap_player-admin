using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator.Events;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class ExchangeChargeCommServiceTests
    {
        private readonly Mock<ILgApiClient> _mockLgApiClient;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly Mock<IOptions<AppSetting>> _mockAppSettingOptions;
        private readonly Fixture _fixture;
        private readonly AppSetting _appSetting;

        public ExchangeChargeCommServiceTests()
        {
            _mockLgApiClient = new Mock<ILgApiClient>();
            _mockPublisher = new Mock<IPublisher>();
            _mockAppSettingOptions = new Mock<IOptions<AppSetting>>();
            _fixture = new Fixture();
            
            _appSetting = new AppSetting
            {
                Env = _fixture.Create<string>()
            };
            
            _mockAppSettingOptions.Setup(x => x.Value).Returns(_appSetting);
        }

        private ExchangeChargeCommService CreateService()
        {
            return new ExchangeChargeCommService(_mockLgApiClient.Object, _mockPublisher.Object, _mockAppSettingOptions.Object);
        }

        [Fact]
        public async Task GetChargeCommission_WithSuccessfulApiCall_ShouldReturnSuccessResult()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var language = _fixture.Create<string>();
            var groupMode = _fixture.Create<int>();
            var commission = _fixture.Create<double>();

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = 1, // Success status
                    Commission = commission
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/GetCommission",
                    It.Is<object>(obj => 
                        obj.GetType().GetProperty("custId").GetValue(obj).Equals(custId) &&
                        obj.GetType().GetProperty("environment").GetValue(obj).Equals(_appSetting.Env) &&
                        obj.GetType().GetProperty("groupMode").GetValue(obj).Equals(groupMode)),
                    language))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.GetChargeCommission(custId, language, groupMode);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(0, result.Code);
            Assert.Equal(ViewResources.UpdateSuccess, result.Message);
            Assert.Equal(commission, result.SpecificExtendedData);
        }

        [Fact]
        public async Task GetChargeCommission_WithFailedApiStatus_ShouldReturnErrorResult()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var language = _fixture.Create<string>();
            var groupMode = _fixture.Create<int>();
            var commission = _fixture.Create<double>();
            var failedStatus = 2; // Non-success status

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = failedStatus,
                    Commission = commission
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/GetCommission",
                    It.IsAny<object>(),
                    language))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.GetChargeCommission(custId, language, groupMode);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(failedStatus, result.Code);
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
            Assert.Null(result.SpecificExtendedData);
        }

        [Fact]
        public async Task GetChargeCommission_WithNullApiData_ShouldReturnApiProcessingError()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var language = _fixture.Create<string>();
            var groupMode = _fixture.Create<int>();

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = null
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/GetCommission",
                    It.IsAny<object>(),
                    language))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.GetChargeCommission(custId, language, groupMode);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(102, result.Code); // ApiProcessingErrorCode
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
            Assert.Null(result.SpecificExtendedData);
        }

        [Theory]
        [InlineData("12Bet", 0)] // 12Bet should return GroupMode 0
        [InlineData("OtherSite", 1)] // Other sites should return GroupMode 1
        [InlineData("12bet", 0)] // Case insensitive
        [InlineData("", 1)] // Empty string should return GroupMode 1
        public async Task GetChargeCommission_ShouldUseCorrectGroupModeBasedOnSiteName(string siteName, int expectedGroupMode)
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var language = _fixture.Create<string>();
            var actualGroupMode = ExchangeProductHelper.GetGroupMode(siteName);

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = 1,
                    Commission = _fixture.Create<double>()
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/GetCommission",
                    It.IsAny<object>(),
                    language))
                .ReturnsAsync(apiResponse);

            // Act
            await service.GetChargeCommission(custId, language, actualGroupMode);

            // Assert
            Assert.Equal(expectedGroupMode, actualGroupMode);
            _mockLgApiClient.Verify(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                "/Exchange/GetCommission",
                It.Is<object>(obj => obj.GetType().GetProperty("groupMode").GetValue(obj).Equals(expectedGroupMode)),
                language), Times.Once);
        }

        [Fact]
        public async Task UpdateChargeCommission_WithSuccessfulUpdate_ShouldPublishEventAndReturnSuccess()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerUpdateChargeCommMetaModel
            {
                CustId = _fixture.Create<int>(),
                Comm = _fixture.Create<double>(),
                Language = _fixture.Create<string>(),
                SiteName = _fixture.Create<string>(),
                UserId = _fixture.Create<int>(),
                LicenseeCustId = _fixture.Create<int>(),
                OldComm = _fixture.Create<double>()
            };

            var commission = _fixture.Create<double>();
            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = 1, // Success status
                    Commission = commission
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/UpdateCommission",
                    It.Is<object>(obj =>
                        obj.GetType().GetProperty("custId").GetValue(obj).Equals(metaModel.CustId) &&
                        obj.GetType().GetProperty("commission").GetValue(obj).Equals(metaModel.Comm) &&
                        obj.GetType().GetProperty("environment").GetValue(obj).Equals(_appSetting.Env) &&
                        obj.GetType().GetProperty("groupMode").GetValue(obj).Equals(ExchangeProductHelper.GetGroupMode(metaModel.SiteName))),
                    metaModel.Language))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(0, result.Code);
            Assert.Equal(ViewResources.UpdateSuccess, result.Message);
            
            // Verify event was published
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateChargeCommission_WithFailedUpdate_ShouldNotPublishEventAndReturnError()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerUpdateChargeCommMetaModel
            {
                CustId = _fixture.Create<int>(),
                Comm = _fixture.Create<double>(),
                Language = _fixture.Create<string>(),
                SiteName = _fixture.Create<string>()
            };

            var commission = _fixture.Create<double>();
            var failedStatus = 2; // Non-success status
            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = failedStatus,
                    Commission = commission
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/UpdateCommission",
                    It.IsAny<object>(),
                    metaModel.Language))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(failedStatus, result.Code);
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
            
            // Verify event was not published
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), default), Times.Never);
        }

        [Fact]
        public async Task UpdateChargeCommission_WithNullApiData_ShouldNotPublishEventAndReturnError()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerUpdateChargeCommMetaModel
            {
                CustId = _fixture.Create<int>(),
                Comm = _fixture.Create<double>(),
                Language = _fixture.Create<string>(),
                SiteName = _fixture.Create<string>()
            };

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = null
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/UpdateCommission",
                    It.IsAny<object>(),
                    metaModel.Language))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(102, result.Code); // ApiProcessingErrorCode
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
            
            // Verify event was not published
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), default), Times.Never);
        }

        [Fact]
        public async Task UpdateChargeCommission_WithZeroApiStatus_ShouldUseApiProcessingErrorCode()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerUpdateChargeCommMetaModel
            {
                CustId = _fixture.Create<int>(),
                Comm = _fixture.Create<double>(),
                Language = _fixture.Create<string>(),
                SiteName = _fixture.Create<string>()
            };

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = 0, // Zero status should be converted to ApiProcessingErrorCode
                    Commission = _fixture.Create<double>()
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/UpdateCommission",
                    It.IsAny<object>(),
                    metaModel.Language))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(102, result.Code); // ApiProcessingErrorCode should be used when status is 0
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
        }

        [Fact]
        public async Task UpdateChargeCommission_ShouldPublishCorrectLogEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerUpdateChargeCommMetaModel
            {
                CustId = _fixture.Create<int>(),
                Comm = 5.5,
                Language = _fixture.Create<string>(),
                SiteName = _fixture.Create<string>(),
                UserId = _fixture.Create<int>(),
                LicenseeCustId = _fixture.Create<int>(),
                OldComm = 4.0
            };

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = 1,
                    Commission = _fixture.Create<double>()
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    "/Exchange/UpdateCommission",
                    It.IsAny<object>(),
                    metaModel.Language))
                .ReturnsAsync(apiResponse);

            PlayerUpdatedChargeCommEvent capturedEvent = null;
            _mockPublisher
                .Setup(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), default))
                .Callback<object, System.Threading.CancellationToken>((evt, _) => capturedEvent = evt as PlayerUpdatedChargeCommEvent);

            // Act
            await service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.NotNull(capturedEvent);
            Assert.Equal(metaModel.CustId, capturedEvent.CustID);
            Assert.Equal(metaModel.UserId, capturedEvent.UserId);
            Assert.Equal(metaModel.SiteName, capturedEvent.SiteName);
            Assert.Equal(metaModel.LicenseeCustId, capturedEvent.LicenseeCustId);
            Assert.NotNull(capturedEvent.Detail);
            Assert.Equal(ChargeCommLogHelper.Success, capturedEvent.Detail.Status);
            Assert.Equal(ProductTypes.Exchange.ToString(), capturedEvent.Detail.ProductId);
            Assert.Equal("4%", capturedEvent.Detail.Old);
            Assert.Equal("5.5%", capturedEvent.Detail.New);
        }

        [Fact]
        public async Task GetChargeCommission_CallsApiClientWithCorrectParameters()
        {
            // Arrange
            var service = CreateService();
            var custId = 12345;
            var language = "en";
            var groupMode = 1;

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = 1,
                    Commission = 5.0
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()))
                .ReturnsAsync(apiResponse);

            // Act
            await service.GetChargeCommission(custId, language, groupMode);

            // Assert
            _mockLgApiClient.Verify(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                "/Exchange/GetCommission",
                It.Is<object>(obj =>
                    obj.GetType().GetProperty("custId").GetValue(obj).Equals(custId) &&
                    obj.GetType().GetProperty("environment").GetValue(obj).Equals(_appSetting.Env) &&
                    obj.GetType().GetProperty("groupMode").GetValue(obj).Equals(groupMode)),
                language), Times.Once);
        }

        [Fact]
        public async Task UpdateChargeCommission_CallsApiClientWithCorrectParameters()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerUpdateChargeCommMetaModel
            {
                CustId = 12345,
                Comm = 5.5,
                Language = "en",
                SiteName = "TestSite"
            };

            var apiResponse = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData
                {
                    Status = 1,
                    Commission = 5.5
                }
            };

            _mockLgApiClient
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    It.IsAny<string>()))
                .ReturnsAsync(apiResponse);

            // Act
            await service.UpdateChargeCommission(metaModel);

            // Assert
            _mockLgApiClient.Verify(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                "/Exchange/UpdateCommission",
                It.Is<object>(obj =>
                    obj.GetType().GetProperty("custId").GetValue(obj).Equals(metaModel.CustId) &&
                    obj.GetType().GetProperty("commission").GetValue(obj).Equals(metaModel.Comm) &&
                    obj.GetType().GetProperty("environment").GetValue(obj).Equals(_appSetting.Env) &&
                    obj.GetType().GetProperty("groupMode").GetValue(obj).Equals(ExchangeProductHelper.GetGroupMode(metaModel.SiteName))),
                metaModel.Language), Times.Once);
        }
    }
} 