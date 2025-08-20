namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Services
{
    using System.Net;
    using Core.Settings;
    using MetaModel;
    using Microsoft.Extensions.Options;
    using Moq;
    using Moq.Protected;
    using MVC.Services;
    using Newtonsoft.Json;

    public class PlayerStatementServiceTest
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IOptions<StatementSetting>> _mockOptions;
        private readonly PlayerStatementService _service;
        private readonly StatementSetting _statementSetting;

        public PlayerStatementServiceTest()
        {
            _statementSetting = new StatementSetting
            {
                ExternalHost = "https://test.example.com/",
                User = "testuser",
                Password = "testpassword"
            };

            _mockOptions = new Mock<IOptions<StatementSetting>>();
            _mockOptions.Setup(x => x.Value).Returns(_statementSetting);

            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.example.com/")
            };

            _service = new PlayerStatementService(_mockOptions.Object, _httpClient);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullOptions_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new PlayerStatementService(null, _httpClient));
        }

        [Fact]
        public async Task GetUrl_WithValidMetaModel_ReturnsCorrectUrl()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel
            {
                ViewType = 1,
                CustId = 123,
                FromDate = new DateTime(2023, 1, 1),
                ToDate = new DateTime(2023, 12, 31),
                Language = "en",
                SiteName = "testsite",
                IsShowIpVN = false
            };

            var tokenResponse = new
            {
                access_token = "test-token-123"
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(tokenResponse))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.GetUrl(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("https://test.example.com/", result);
            Assert.Contains("Statement/Summary", result);
            Assert.Contains("vt=1", result);
            Assert.Contains("cid=123", result);
            Assert.Contains("fd=01-01-2023", result);
            Assert.Contains("td=12-31-2023", result);
            Assert.Contains("ps=1000", result);
            Assert.Contains("sb=True", result);
            Assert.Contains("lang=en", result);
            Assert.Contains("access_token=test-token-123", result);
            Assert.Contains("isDesktopViewOnly=true", result);
        }

        [Fact]
        public async Task GetUrl_WithDifferentDateFormat_FormatsDateCorrectly()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel
            {
                ViewType = 2,
                CustId = 456,
                FromDate = new DateTime(2023, 6, 15),
                ToDate = new DateTime(2023, 7, 20),
                Language = "zh",
                SiteName = "testsite2",
                IsShowIpVN = true
            };

            var tokenResponse = new
            {
                access_token = "test-token-456"
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(tokenResponse))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.GetUrl(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("fd=06-15-2023", result);
            Assert.Contains("td=07-20-2023", result);
            Assert.Contains("vt=2", result);
            Assert.Contains("cid=456", result);
            Assert.Contains("lang=zh", result);
        }

        [Fact]
        public async Task GetUrl_HttpClientThrowsException_PropagatesException()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel
            {
                SiteName = "testsite",
                IsShowIpVN = false
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetUrl(metaModel));
        }

        [Fact]
        public async Task GetUrl_HttpResponseNotOk_ReturnsUrlWithNullToken()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel
            {
                ViewType = 1,
                CustId = 123,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                Language = "en",
                SiteName = "testsite",
                IsShowIpVN = false
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetUrl(metaModel));
        }

        [Fact]
        public async Task GetUrl_ValidatesAuthenticationRequest()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel
            {
                SiteName = "mysite",
                IsShowIpVN = true
            };

            var tokenResponse = new
            {
                access_token = "test-token"
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(tokenResponse))
            };

            HttpRequestMessage capturedRequest = null;
            string capturedRequestBody = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>(async (request, token) =>
                {
                    capturedRequest = request;
                    if (request.Content != null)
                    {
                        capturedRequestBody = await request.Content.ReadAsStringAsync();
                    }
                })
                .ReturnsAsync(httpResponse);

            // Act
            await _service.GetUrl(metaModel);

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Post, capturedRequest.Method);
            Assert.Equal("https://test.example.com/api/auth", capturedRequest.RequestUri.ToString());
            Assert.Equal("application/json", capturedRequest.Content.Headers.ContentType.MediaType);

            Assert.NotNull(capturedRequestBody);
            Assert.Contains("mysite@testuser", capturedRequestBody);
            Assert.Contains("testpassword", capturedRequestBody);
            Assert.Contains("\"IsShowIpvn\":true", capturedRequestBody);
        }

        [Fact]
        public async Task GetUrl_WithNullMetaModel_ThrowsNullReferenceException()
        {
            // Arrange
            PlayerStatementMetaModel metaModel = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.GetUrl(metaModel));
        }

        [Fact]
        public async Task GetUrl_WithEmptyToken_ReturnsUrlWithEmptyToken()
        {
            // Arrange
            var metaModel = new PlayerStatementMetaModel
            {
                ViewType = 1,
                CustId = 123,
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                Language = "en",
                SiteName = "testsite",
                IsShowIpVN = false
            };

            var tokenResponse = new
            {
                access_token = ""
            };

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(tokenResponse))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _service.GetUrl(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("access_token=", result);
        }
    }
}