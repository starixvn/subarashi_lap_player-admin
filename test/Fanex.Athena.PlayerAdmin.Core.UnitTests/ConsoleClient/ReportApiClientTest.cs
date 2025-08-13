namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    using System.Net;
    using System.Text;
    using Core.ConsoleClient;
    using Core.Services;
    using Moq;
    using Moq.Protected;

    public class ReportApiClientTest
    {
        private readonly ReportApiClient _apiClient;
        private readonly HttpClient _httpClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public ReportApiClientTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockContextService = new Mock<IContextService>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.api.com/")
            };
            _apiClient = new ReportApiClient(_httpClient, _mockContextService.Object);
        }

        [Fact]
        public async Task PostStringAsync_WithValidData_ReturnsStringResponse()
        {
            // Arrange
            var responseContent = "Test string response";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            _mockContextService.Setup(x => x.SiteDBServer).Returns("TestDBServer");
            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync("test-token");

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.PostStringAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.Equal(responseContent, result);
        }

        [Fact]
        public async Task PostStringAsync_SetsCorrectHeaders()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            _mockContextService.Setup(x => x.SiteDBServer).Returns("TestDBServer");
            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync("test-token");

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) => capturedRequest = request)
                .ReturnsAsync(responseMessage);

            // Act
            await _apiClient.PostStringAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.True(capturedRequest.Headers.Contains("X-IgnoreSharding"));
            Assert.Equal("True", capturedRequest.Headers.GetValues("X-IgnoreSharding").FirstOrDefault());
            Assert.NotNull(capturedRequest.Headers.Authorization);
            Assert.Equal("Bearer", capturedRequest.Headers.Authorization.Scheme);
            Assert.Equal("test-token", capturedRequest.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task PostStringAsync_WithCustomTimeout_UsesSpecifiedTimeout()
        {
            // Arrange
            var responseContent = "Test response";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            _mockContextService.Setup(x => x.SiteDBServer).Returns("TestDBServer");
            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync("test-token");

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.PostStringAsync("/test", new { data = "test" }, "en", 30);

            // Assert
            Assert.Equal(responseContent, result);
        }

        [Fact]
        public async Task PostStringAsync_WhenContextServiceThrows_PropagatesException()
        {
            // Arrange
            _mockContextService.Setup(x => x.AccessToken).ThrowsAsync(new InvalidOperationException("Context error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _apiClient.PostStringAsync("/test", new { data = "test" }, "en"));
        }

        [Fact]
        public async Task PostStringAsync_WhenAccessTokenIsNull_SetsNullAuthorizationHeader()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync((string)null);

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) => capturedRequest = request)
                .ReturnsAsync(responseMessage);

            // Act
            await _apiClient.PostStringAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.NotNull(capturedRequest.Headers.Authorization);
            Assert.Equal("Bearer", capturedRequest.Headers.Authorization.Scheme);
            Assert.Null(capturedRequest.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task PostStringAsync_WhenHttpRequestFails_ThrowsServiceException()
        {
            // Arrange
            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync("test-token");

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostStringAsync("/test", new { data = "test" }, "en"));

            Assert.Contains("Network error", exception.Message);
            Assert.Contains("Report", exception.Message);
        }

        [Fact]
        public async Task PostStringAsync_WithNullValue_SendsRequestSuccessfully()
        {
            // Arrange
            var responseContent = "Test response";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync("test-token");

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.PostStringAsync("/test", null, "en");

            // Assert
            Assert.Equal(responseContent, result);
        }

        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            // Arrange & Act
            var client = new ReportApiClient(_httpClient, _mockContextService.Object);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void Constructor_WithValidParameters_CreatesInstance()
        {
            // Act & Assert
            var client = new ReportApiClient(_httpClient, _mockContextService.Object);
            Assert.NotNull(client);
        }

        [Fact]
        public async Task PostStringAsync_WhenResponseIsNotSuccessful_ThrowsServiceException()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad request", Encoding.UTF8, "application/json")
            };

            _mockContextService.Setup(x => x.AccessToken).ReturnsAsync("test-token");

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostStringAsync("/test", new { data = "test" }, "en"));
        }
    }
}