namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    using System.Net;
    using System.Text;
    using Core.ConsoleClient;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;

    public class LgApiClientTest
    {
        private readonly LgApiClient _apiClient;
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public LgApiClientTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.api.com/")
            };
            _apiClient = new LgApiClient(_httpClient);
        }

        [Fact]
        public async Task PostAsync_WithValidData_ReturnsDeserializedObject()
        {
            // Arrange
            var testObject = new TestModel { Id = 1, Name = "Test" };
            var responseJson = JsonConvert.SerializeObject(testObject);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.PostAsync<TestModel>("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
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
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync<TestModel>("/test", new { data = "test" }, "en"));

            Assert.Contains("Network error", exception.Message);
            Assert.Contains("LG", exception.Message);
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
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync<TestModel>(null, new { data = "test" }, "en"));

            Assert.Contains("LG", exception.Message);
        }

        [Fact]
        public async Task PostStringAsync_WithEmptyPath_SendsRequestToCorrectEndpoint()
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
            await _apiClient.PostStringAsync("", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Post, capturedRequest.Method);
            Assert.Equal("https://test.api.com/", capturedRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task PostStringAsync_WithValidPath_SendsRequestToCorrectEndpoint()
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
            await _apiClient.PostStringAsync("/api/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Equal(HttpMethod.Post, capturedRequest.Method);
            Assert.Equal("https://test.api.com//api/test", capturedRequest.RequestUri.ToString());
        }

        [Fact]
        public async Task PostAsync_WithInvalidJsonResponse_ThrowsServiceException()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Invalid JSON", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync<TestModel>("/test", null, "en"));
        }

        [Fact]
        public async Task PostStringAsync_WithDifferentLanguages_SendsRequestSuccessfully()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Test response", Encoding.UTF8, "application/json")
                });

            // Act
            var result1 = await _apiClient.PostStringAsync("/test", new { data = "test" }, "en");
            var result2 = await _apiClient.PostStringAsync("/test", new { data = "test" }, "zh-CN");
            var result3 = await _apiClient.PostStringAsync("/test", new { data = "test" }, null);

            // Assert
            Assert.Equal("Test response", result1);
            Assert.Equal("Test response", result2);
            Assert.Equal("Test response", result3);
        }

        [Fact]
        public void Constructor_WithValidHttpClient_InitializesCorrectly()
        {
            // Arrange & Act
            var client = new LgApiClient(_httpClient);

            // Assert
            Assert.NotNull(client);
        }

        [Fact]
        public void Constructor_WithValidHttpClient_CreatesInstance()
        {
            // Act & Assert
            var client = new LgApiClient(_httpClient);
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
            await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync<TestModel>("/test", new { data = "test" }, "en"));
        }

        [Fact]
        public async Task PostStringAsync_WithComplexObject_SerializesCorrectly()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Test response", Encoding.UTF8, "application/json")
            };

            var complexObject = new
            {
                Id = 123,
                Name = "Test Object",
                Properties = new { Prop1 = "Value1", Prop2 = 456 },
                Items = new[] { "Item1", "Item2", "Item3" }
            };

            string capturedContent = null;
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>(async (request, token) =>
                {
                    if (request.Content != null)
                    {
                        capturedContent = await request.Content.ReadAsStringAsync();
                    }
                })
                .ReturnsAsync(responseMessage);

            // Act
            await _apiClient.PostStringAsync("/test", complexObject, "en");

            // Assert
            Assert.NotNull(capturedContent);
            Assert.Contains("123", capturedContent);
            Assert.Contains("Test Object", capturedContent);
            Assert.Contains("Value1", capturedContent);
        }

        // Test helper class
        private class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}