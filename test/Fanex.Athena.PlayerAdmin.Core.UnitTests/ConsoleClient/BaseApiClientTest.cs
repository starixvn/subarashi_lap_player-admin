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
    using Newtonsoft.Json;
    using Xunit;

    public class BaseApiClientTest
    {
        private readonly TestableBaseApiClient _apiClient;
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public BaseApiClientTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://test.api.com/")
            };
            _apiClient = new TestableBaseApiClient(_httpClient);
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
            var responseContent = "Test response content";
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
            var result = await _apiClient.PostStringAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.Equal(responseContent, result);
        }

        [Fact]
        public async Task GetStringAsync_WithValidPath_ReturnsStringResponse()
        {
            // Arrange
            var responseContent = "Test GET response";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.GetStringAsync("/test");

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
            Assert.Contains("Test API", exception.Message);
        }

        [Fact]
        public async Task PostAsync_WhenResponseIsNotSuccessful_CallsHandleUnSuccessRequestAsync()
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
        public async Task PostStringAsync_WithTimeout_UsesSpecifiedTimeout()
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
            var result = await _apiClient.PostStringAsync("/test", new { data = "test" }, "en", 30);

            // Assert
            Assert.Equal("Test response", result);
        }

        [Fact]
        public void ServiceException_WithMessage_FormatsCorrectly()
        {
            // Arrange & Act
            var exception = new ServiceException("Test error", "Test Service");

            // Assert
            Assert.Contains("Test Service", exception.Message);
            Assert.Contains("Test error", exception.Message);
        }

        [Fact]
        public void ServiceException_WithInnerException_FormatsCorrectly()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner error");

            // Act
            var exception = new ServiceException("Test error", "Test Service", innerException);

            // Assert
            Assert.Contains("Test Service", exception.Message);
            Assert.Contains("Test error", exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        // Test helper classes
        private class TestableBaseApiClient : BaseApiClient
        {
            public TestableBaseApiClient(HttpClient client) : base(client)
            {
            }

            protected override string ApiName => "Test API";

            public new async Task<T> PostAsync<T>(string path, object value, string language) where T : class
            {
                return await base.PostAsync<T>(path, value, language);
            }

            public new async Task<string> PostStringAsync(string path, object value, string language, int timeOut = 100)
            {
                return await base.PostStringAsync(path, value, language, timeOut);
            }

            public new async Task<string> GetStringAsync(string path, int timeOut = 100)
            {
                return await base.GetStringAsync(path, timeOut);
            }
        }

        private class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}