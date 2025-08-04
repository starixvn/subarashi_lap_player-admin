namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ConsoleClient;
    using Core.Services;
    using Moq;
    using Moq.Protected;
    using Xunit;

    public class BetListApiClientTest
    {
        private readonly BetListApiClient _apiClient;
        private readonly HttpClient _httpClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public BetListApiClientTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockContextService = new Mock<IContextService>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.api.com/")
            };
            _apiClient = new BetListApiClient(_httpClient, _mockContextService.Object);
        }

        [Fact]
        public async Task PostAsync_WithValidData_ReturnsHttpResponseMessage()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
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
            var result = await _apiClient.PostAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_SetsCorrectHeaders()
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
            await _apiClient.PostAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.True(capturedRequest.Headers.Contains("X-SiteDBServer"));
            Assert.Equal("TestDBServer", capturedRequest.Headers.GetValues("X-SiteDBServer").FirstOrDefault());
            Assert.NotNull(capturedRequest.Headers.Authorization);
            Assert.Equal("Bearer", capturedRequest.Headers.Authorization.Scheme);
            Assert.Equal("test-token", capturedRequest.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task PostAsync_WithCustomTimeout_UsesSpecifiedTimeout()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
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
            var result = await _apiClient.PostAsync("/test", new { data = "test" }, "en", 30);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_WhenContextServiceThrows_PropagatesException()
        {
            // Arrange
            _mockContextService.Setup(x => x.SiteDBServer).Throws(new InvalidOperationException("Context error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
        }

        [Fact]
        public async Task PostAsync_WhenAccessTokenIsNull_SetsNullAuthorizationHeader()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            _mockContextService.Setup(x => x.SiteDBServer).Returns("TestDBServer");
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
            await _apiClient.PostAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.NotNull(capturedRequest.Headers.Authorization);
            Assert.Equal("Bearer", capturedRequest.Headers.Authorization.Scheme);
            Assert.Null(capturedRequest.Headers.Authorization.Parameter);
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
        public async Task PostStringAsync_WhenHttpRequestFails_ThrowsServiceException()
        {
            // Arrange
            _mockContextService.Setup(x => x.SiteDBServer).Returns("TestDBServer");
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
            Assert.Contains("Bet List", exception.Message);
        }

        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            // Arrange & Act
            var client = new BetListApiClient(_httpClient, _mockContextService.Object);

            // Assert
            Assert.NotNull(client);
        }
    }
}