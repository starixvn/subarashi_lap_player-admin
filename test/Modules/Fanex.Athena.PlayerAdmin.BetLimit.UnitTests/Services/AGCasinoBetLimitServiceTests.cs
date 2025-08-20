namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    using System.Net;
    using AutoFixture;
    using BetLimit.DTO;
    using BetLimit.Mediator;
    using BetLimit.Repositories.Interfaces;
    using BetLimit.Services;
    using Core.Models;
    using Core.Settings;
    using MediatR;
    using MetaModel;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;

    public class AGCasinoBetLimitServiceTests
    {
        private readonly IOptions<BetLimitApiSetting> _appSetting;
        private readonly IFixture _fixture;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<AGCasinoBetLimitService>> _loggerMock;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly Mock<IAGCasinoBetLimitRepository> _repositoryMock;
        private readonly AGCasinoBetLimitService _service;

        public AGCasinoBetLimitServiceTests()
        {
            _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<AGCasinoBetLimitService>>();
            _publisherMock = new Mock<IPublisher>();
            _appSetting = Options.Create(new BetLimitApiSetting { AGCasino = "http://test-api.com/agcasino" });
            _repositoryMock = new Mock<IAGCasinoBetLimitRepository>();
            _httpClient = new HttpClient(new TestHttpMessageHandler());

            _service = new AGCasinoBetLimitService(_loggerMock.Object, _appSetting, _repositoryMock.Object, _httpClient, _publisherMock.Object);
        }

        [Fact]
        public async Task GetGroupBettingsAsync_ShouldReturnEmptyList_WhenHttpRequestFails()
        {
            // Arrange
            const string siteName = "TestSite";
            const int currency = 1;

            var httpClient = new HttpClient(new TestHttpMessageHandler(HttpStatusCode.InternalServerError));
            var service = new AGCasinoBetLimitService(_loggerMock.Object, _appSetting, _repositoryMock.Object, httpClient, _publisherMock.Object);

            // Act
            var result = await service.GetGroupBettingsAsync(siteName, currency);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGroupBettingsAsync_ShouldReturnEmptyList_WhenInvalidXmlReturned()
        {
            // Arrange
            const string siteName = "TestSite";
            const int currency = 1;

            var httpClient = new HttpClient(new TestHttpMessageHandler(HttpStatusCode.OK, "Invalid XML"));
            var service = new AGCasinoBetLimitService(_loggerMock.Object, _appSetting, _repositoryMock.Object, httpClient, _publisherMock.Object);

            // Act
            var result = await service.GetGroupBettingsAsync(siteName, currency);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetGroupBettingsAsync_ShouldParseValidXml_WhenValidXmlReturned()
        {
            // Arrange
            const string siteName = "TestSite";
            const int currency = 1;
            const string validXml = @"<?xml version=""1.0""?>
                <root>
                    <Game GameType=""Baccarat"" Name=""Baccarat Game"">
                        <Group GroupType=""VIP"">
                            <Min>10</Min>
                            <Max>1000</Max>
                        </Group>
                    </Game>
                </root>";

            var httpClient = new HttpClient(new TestHttpMessageHandler(HttpStatusCode.OK, validXml));
            var service = new AGCasinoBetLimitService(_loggerMock.Object, _appSetting, _repositoryMock.Object, httpClient, _publisherMock.Object);

            // Act
            var result = await service.GetGroupBettingsAsync(siteName, currency);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var game = result.First();
            Assert.Equal("Baccarat", game.GameType);
            Assert.Equal("Baccarat Game", game.Name);
            Assert.Single(game.Groups);
            var group = game.Groups.First();
            Assert.Equal("VIP", group.GroupType);
            Assert.Equal(10, group.Min);
            Assert.Equal(1000, group.Max);
        }

        [Fact]
        public async Task GetBetSettingAsync_ShouldReturnBetSetting_WhenCalled()
        {
            // Arrange
            const int custId = 123;
            var expectedBetSetting = _fixture.Create<AgCasinoBetSetting>();

            _repositoryMock.Setup(x => x.GetBetSettingAsync(custId)).ReturnsAsync(expectedBetSetting);

            // Act
            var result = await _service.GetBetSettingAsync(custId);

            // Assert
            Assert.Equal(expectedBetSetting, result);
            _repositoryMock.Verify(x => x.GetBetSettingAsync(custId), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldReturnResult_WhenRepositoryReturnsResult()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
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
        public async Task UpdateSettingAsync_ShouldPublishEvent_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            var expectedResult = new ResultInfo { Code = 0 };

            _repositoryMock.Setup(x => x.UpdateSettingAsync(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UpdateSettingAsync(metaModel);

            // Assert
            _publisherMock.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), It.IsAny<CancellationToken>()), Times.Once);
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
    }

    // Test HTTP message handler for mocking HTTP responses
    public class TestHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _content;
        private readonly HttpStatusCode _statusCode;

        public TestHttpMessageHandler(HttpStatusCode statusCode = HttpStatusCode.OK, string content = "")
        {
            _statusCode = statusCode;
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_content)
            };
            return Task.FromResult(response);
        }
    }
}