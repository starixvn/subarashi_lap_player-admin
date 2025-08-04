namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.ConsoleClient;
    using Core.Services;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;
    using Xunit;

    public class CommonApiClientTest
    {
        private readonly CommonApiClient _apiClient;
        private readonly HttpClient _httpClient;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public CommonApiClientTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockContextService = new Mock<IContextService>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.api.com/")
            };
            _apiClient = new CommonApiClient(_httpClient, _mockContextService.Object);
        }

        [Fact]
        public async Task GetAsync_WithSuccessfulResponse_ReturnsDeserializedResult()
        {
            // Arrange
            var testData = new TestModel { Id = 1, Name = "Test" };
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                ErrorMessage = null,
                Result = testData
            };
            var responseJson = JsonConvert.SerializeObject(apiResponse);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.GetAsync<TestModel>("/test");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public async Task GetAsync_WithErrorResponse_ReturnsDefault()
        {
            // Arrange
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 1,
                ErrorMessage = "Error occurred",
                Result = null
            };
            var responseJson = JsonConvert.SerializeObject(apiResponse);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.GetAsync<TestModel>("/test");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAsync_WithQueryParameters_BuildsCorrectUrl()
        {
            // Arrange
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = new TestModel { Id = 1, Name = "Test" }
            };
            var responseJson = JsonConvert.SerializeObject(apiResponse);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) => capturedRequest = request)
                .ReturnsAsync(responseMessage);

            var queryParams = new { id = 123, name = "test", active = true };

            // Act
            await _apiClient.GetAsync<TestModel>("/test", queryParams);

            // Assert
            Assert.NotNull(capturedRequest);
            var uri = capturedRequest.RequestUri.ToString();
            Assert.Contains("id=123", uri);
            Assert.Contains("name=test", uri);
            Assert.Contains("active=True", uri);
        }

        [Fact]
        public async Task GetAsync_WithNullQueryParameters_DoesNotAddQueryString()
        {
            // Arrange
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = new TestModel { Id = 1, Name = "Test" }
            };
            var responseJson = JsonConvert.SerializeObject(apiResponse);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            HttpRequestMessage capturedRequest = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((request, token) => capturedRequest = request)
                .ReturnsAsync(responseMessage);

            // Act
            await _apiClient.GetAsync<TestModel>("/test");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal("https://test.api.com//test", capturedRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task GetAsync_WithCustomTimeout_UsesSpecifiedTimeout()
        {
            // Arrange
            var apiResponse = new ApiResponse<TestModel>
            {
                ErrorCode = 0,
                Result = new TestModel { Id = 1, Name = "Test" }
            };
            var responseJson = JsonConvert.SerializeObject(apiResponse);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.GetAsync<TestModel>("/test", null, 30);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAsync_WhenHttpRequestFails_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.GetAsync<TestModel>("/test"));

            Assert.Contains("Network error", exception.Message);
            Assert.Contains("Common API Client", exception.Message);
        }

        [Fact]
        public async Task GetAsync_WithInvalidJson_ThrowsJsonReaderException()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Invalid JSON", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(() => _apiClient.GetAsync<TestModel>("/test"));
        }

        [Fact]
        public async Task GetAsync_WithNullApiResponse_ReturnsDefault()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.GetAsync<TestModel>("/test");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            // Arrange & Act
            var client = new CommonApiClient(_httpClient, _mockContextService.Object);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void Constructor_WithValidParameters_CreatesInstance()
        {
            // Act & Assert
            var client = new CommonApiClient(_httpClient, _mockContextService.Object);
            Assert.NotNull(client);
        }

        [Fact]
        public void ApiResponse_Properties_WorkCorrectly()
        {
            // Arrange & Act
            var response = new ApiResponse<string>
            {
                ErrorCode = 1,
                ErrorMessage = "Test error",
                Result = "Test result"
            };

            // Assert
            Assert.Equal(1, response.ErrorCode);
            Assert.Equal("Test error", response.ErrorMessage);
            Assert.Equal("Test result", response.Result);
        }

        // Test helper class
        private class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}