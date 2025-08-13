namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    using AutoFixture;
    using BetLimit.DTO;
    using BetLimit.Mediator.Events;
    using BetLimit.Services;
    using Core.ConsoleClient;
    using Core.Models;
    using Core.Resources;
    using Core.Settings;
    using MediatR;
    using MetaModel;
    using Microsoft.Extensions.Options;
    using Moq;

    public class ExchangeChargeCommServiceTests
    {
        private readonly IOptions<AppSetting> _appSetting;
        private readonly IFixture _fixture;
        private readonly Mock<ILgApiClient> _lgApiClientMock;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly ExchangeChargeCommService _service;

        public ExchangeChargeCommServiceTests()
        {
            _fixture = new Fixture();
            _lgApiClientMock = new Mock<ILgApiClient>();
            _publisherMock = new Mock<IPublisher>();
            _appSetting = Options.Create(new AppSetting { Env = "TEST" });


            _service = new ExchangeChargeCommService(_lgApiClientMock.Object, _publisherMock.Object, _appSetting);
        }

        [Fact]
        public async Task GetChargeCommission_ShouldReturnSuccessResult_WhenApiDataIsValid()
        {
            // Arrange
            const int custId = 123;
            const string language = "en";
            const int groupMode = 1;
            const double expectedCommission = 5.5;


            var apiData = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData { Status = 1, Commission = expectedCommission }
            };


            _lgApiClientMock.Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(It.IsAny<string>(), It.IsAny<object>(), language)).ReturnsAsync(apiData);

            // Act
            var result = await _service.GetChargeCommission(custId, language, groupMode);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal(0, result.Code);
            Assert.Equal(expectedCommission, result.SpecificExtendedData);
            Assert.Equal(ViewResources.UpdateSuccess, result.Message);
        }

        [Fact]
        public async Task GetChargeCommission_ShouldReturnErrorResult_WhenApiDataIsNull()
        {
            // Arrange
            const int custId = 123;
            const string language = "en";
            const int groupMode = 1;


            var apiData = new ApiData<ExchangeCommissionData> { Data = null };


            _lgApiClientMock.Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(It.IsAny<string>(), It.IsAny<object>(), language)).ReturnsAsync(apiData);

            // Act
            var result = await _service.GetChargeCommission(custId, language, groupMode);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccessful);
            Assert.Equal(102, result.Code);
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
        }

        [Fact]
        public async Task GetChargeCommission_ShouldReturnErrorResult_WhenStatusIsNotSuccess()
        {
            // Arrange
            const int custId = 123;
            const string language = "en";
            const int groupMode = 1;
            const double commission = 5.5;


            var apiData = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData { Status = 0, Commission = commission }
            };


            _lgApiClientMock.Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(It.IsAny<string>(), It.IsAny<object>(), language)).ReturnsAsync(apiData);

            // Act
            var result = await _service.GetChargeCommission(custId, language, groupMode);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccessful);
            Assert.Equal(102, result.Code);
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
        }

        [Fact]
        public async Task GetChargeCommission_ShouldCallApiWithCorrectParameters_WhenCalled()
        {
            // Arrange
            const int custId = 123;
            const string language = "en";
            const int groupMode = 1;
            const string environment = "TEST";


            var apiData = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData { Status = 1, Commission = 5.5 }
            };


            _lgApiClientMock.Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>("/Exchange/GetCommission", It.IsAny<object>(), language))
                .ReturnsAsync(apiData);

            // Act
            var result = await _service.GetChargeCommission(custId, language, groupMode);

            // Assert
            _lgApiClientMock.Verify(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                "/Exchange/GetCommission",
                It.Is<object>(x => HasCorrectProperties(x, custId, environment, groupMode)),
                language), Times.Once);
        }

        [Fact]
        public async Task UpdateChargeCommission_ShouldReturnSuccessResult_WhenApiDataIsValid()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.SiteName = "TestSite";
            metaModel.Comm = 5.0;


            var apiData = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData { Status = 1, Commission = metaModel.Comm }
            };

            _lgApiClientMock
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(It.IsAny<string>(), It.IsAny<object>(), metaModel.Language))
                .ReturnsAsync(apiData);

            // Act
            var result = await _service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccessful);
            Assert.Equal(0, result.Code);
            Assert.Equal(ViewResources.UpdateSuccess, result.Message);
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateChargeCommission_ShouldReturnErrorResult_WhenApiDataIsNull()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.SiteName = "TestSite";


            var apiData = new ApiData<ExchangeCommissionData> { Data = null };

            _lgApiClientMock
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(It.IsAny<string>(), It.IsAny<object>(), metaModel.Language))
                .ReturnsAsync(apiData);

            // Act
            var result = await _service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccessful);
            Assert.Equal(102, result.Code);
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateChargeCommission_ShouldReturnErrorResult_WhenStatusIsNotSuccess()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.SiteName = "TestSite";
            metaModel.Comm = 5.0;


            var apiData = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData { Status = 0, Commission = metaModel.Comm }
            };

            _lgApiClientMock
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(It.IsAny<string>(), It.IsAny<object>(), metaModel.Language))
                .ReturnsAsync(apiData);

            // Act
            var result = await _service.UpdateChargeCommission(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccessful);
            Assert.Equal(102, result.Code);
            Assert.Equal(ViewResources.ErrorProcessingMsg, result.Message);
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateChargeCommission_ShouldPublishEvent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.SiteName = "TestSite";
            metaModel.Comm = 5.0;


            var apiData = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData { Status = 1, Commission = metaModel.Comm }
            };

            _lgApiClientMock
                .Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>(It.IsAny<string>(), It.IsAny<object>(), metaModel.Language))
                .ReturnsAsync(apiData);

            // Act
            var result = await _service.UpdateChargeCommission(metaModel);

            // Assert
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedChargeCommEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("12Bet", 0)]
        [InlineData("OtherSite", 1)]
        [InlineData("testsite", 1)]
        public async Task UpdateChargeCommission_ShouldUseCorrectGroupMode_WhenSiteNameProvided(string siteName, int expectedGroupMode)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.SiteName = siteName;
            metaModel.Comm = 5.0;


            var apiData = new ApiData<ExchangeCommissionData>
            {
                Data = new ExchangeCommissionData { Status = 1, Commission = metaModel.Comm }
            };


            _lgApiClientMock.Setup(x => x.PostAsync<ApiData<ExchangeCommissionData>>("/Exchange/UpdateCommission", It.IsAny<object>(), metaModel.Language))
                .ReturnsAsync(apiData);

            // Act
            var result = await _service.UpdateChargeCommission(metaModel);

            // Assert
            _lgApiClientMock.Verify(x => x.PostAsync<ApiData<ExchangeCommissionData>>(
                "/Exchange/UpdateCommission",
                It.Is<object>(x => HasCorrectUpdateProperties(x, metaModel.CustId, metaModel.Comm, "TEST", expectedGroupMode)),
                metaModel.Language), Times.Once);
        }

        private static bool HasCorrectProperties(object requestObject, int custId, string environment, int groupMode)
        {
            var type = requestObject.GetType();
            var custIdProp = type.GetProperty("custId")?.GetValue(requestObject);
            var environmentProp = type.GetProperty("environment")?.GetValue(requestObject);
            var groupModeProp = type.GetProperty("groupMode")?.GetValue(requestObject);


            return custIdProp?.Equals(custId) == true &&
                   environmentProp?.Equals(environment) == true &&
                   groupModeProp?.Equals(groupMode) == true;
        }

        private static bool HasCorrectUpdateProperties(object requestObject, int custId, double commission, string environment, int groupMode)
        {
            var type = requestObject.GetType();
            var custIdProp = type.GetProperty("custId")?.GetValue(requestObject);
            var commissionProp = type.GetProperty("commission")?.GetValue(requestObject);
            var environmentProp = type.GetProperty("environment")?.GetValue(requestObject);
            var groupModeProp = type.GetProperty("groupMode")?.GetValue(requestObject);


            return custIdProp?.Equals(custId) == true &&
                   commissionProp?.Equals(commission) == true &&
                   environmentProp?.Equals(environment) == true &&
                   groupModeProp?.Equals(groupMode) == true;
        }
    }
}