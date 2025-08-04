namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ConsoleClient;
    using Moq;
    using Moq.Protected;
    using Xunit;

    public class CTSApiClientTest
    {
        private readonly CTSApiClient _apiClient;
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public CTSApiClientTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.api.com/")
            };
            _apiClient = new CTSApiClient(_httpClient);
        }

        [Fact]
        public async Task PostAsync_WithValidData_ReturnsHttpResponseMessage()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

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
        public async Task PostAsync_WithCustomTimeout_UsesSpecifiedTimeout()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

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
        public async Task PostAsync_WhenHttpRequestFails_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));

            Assert.Contains("Network error", exception.Message);
            Assert.Contains("CTS API", exception.Message);
        }

        [Fact]
        public async Task PostAsync_WithNullPath_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new ArgumentNullException("path"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync(null, new { data = "test" }, "en"));

            Assert.Contains("CTS API", exception.Message);
        }

        [Fact]
        public async Task PostAsync_WithEmptyPath_SendsRequestToCorrectEndpoint()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) => capturedRequest = request)
                .ReturnsAsync(responseMessage);

            // Act
            await _apiClient.PostAsync("", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Post, capturedRequest.Method);
            Assert.Equal("https://test.api.com/", capturedRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task PostAsync_WithValidPath_SendsRequestToCorrectEndpoint()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) => capturedRequest = request)
                .ReturnsAsync(responseMessage);

            // Act
            await _apiClient.PostAsync("/api/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Post, capturedRequest.Method);
            Assert.Equal("https://test.api.com//api/test", capturedRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task PostAsync_WithNullValue_SendsRequestSuccessfully()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.PostAsync("/test", null, "en");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_WithDifferentLanguages_SendsRequestSuccessfully()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result1 = await _apiClient.PostAsync("/test", new { data = "test" }, "en");
            var result2 = await _apiClient.PostAsync("/test", new { data = "test" }, "zh-CN");
            var result3 = await _apiClient.PostAsync("/test", new { data = "test" }, null);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.Equal(HttpStatusCode.OK, result1.StatusCode);
            Assert.Equal(HttpStatusCode.OK, result2.StatusCode);
            Assert.Equal(HttpStatusCode.OK, result3.StatusCode);
        }

        [Fact]
        public void Constructor_WithValidHttpClient_InitializesCorrectly()
        {
            // Arrange & Act
            var client = new CTSApiClient(_httpClient);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void Constructor_WithValidHttpClient_CreatesInstance()
        {
            // Act & Assert
            var client = new CTSApiClient(_httpClient);
            Assert.NotNull(client);
        }

        [Fact]
        public async Task PostAsync_WhenResponseIsNotSuccessful_ThrowsServiceException()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad request", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
        }
    }
}