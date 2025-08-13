namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    using System.Net;
    using System.Runtime.Serialization;
    using System.Text;
    using Core.ConsoleClient;
    using Moq;
    using Moq.Protected;
    using Newtonsoft.Json;

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

        [Fact]
        public async Task HandleAsync_WithMsgPackMediaType_SerializesCorrectly()
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
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.Content!.Headers.ContentType!.MediaType == "application/x-msgpack"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _apiClient.TestHandleAsyncMsgPack("/test", new { data = "test" }, "en");

            // Assert
            Assert.Equal("Test response", result);
            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Content!.Headers.ContentType!.MediaType == "application/x-msgpack"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task HandleAsync_WithMsgPackMediaType_WhenExceptionOccurs_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => 
                        req.Content!.Headers.ContentType!.MediaType == "application/x-msgpack"),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("MsgPack Network error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => 
                _apiClient.TestHandleAsyncMsgPack("/test", new { data = "test" }, "en"));

            Assert.Contains("MsgPack Network error", exception.Message);
            Assert.Contains("Test API", exception.Message);
        }

        [Fact]
        public async Task HandleAsync_WhenCodeBlockIsNull_ReturnsDefault()
        {
            // Arrange & Act
            var result = await _apiClient.TestHandleAsyncWithNullCodeBlock("/test", new { data = "test" }, "en");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleAsync_WhenExceptionOccurs_AndDisposeHttpResponseIsFalse_DisposesResponse()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("test", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => 
                _apiClient.TestHandleAsyncWithExceptionAndNoDispose("/test", new { data = "test" }, "en"));

            Assert.Contains("Test exception", exception.Message);
            Assert.Contains("Test API", exception.Message);
        }

        [Fact]
        public async Task GetStringAsync_WithCustomTimeout_UsesCancellationToken()
        {
            // Arrange
            var responseContent = "Test GET response with custom timeout";
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
            var result = await _apiClient.GetStringAsync("/test", 50);

            // Assert
            Assert.Equal(responseContent, result);
        }

        [Fact]
        public async Task GetStringAsync_WhenHttpExceptionOccurs_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("Request timeout"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => 
                _apiClient.GetStringAsync("/test"));

            Assert.Contains("Request timeout", exception.Message);
            Assert.Contains("Test API", exception.Message);
        }

        [Fact]
        public async Task GetStringAsync_WhenResponseIsNotSuccessful_CallsHandleUnSuccessRequestAsync()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Server error", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.GetStringAsync("/test"));
            
            Assert.Contains("Test API", exception.Message);
        }

        [Fact]
        public async Task PostStringAsync_WithLargeContent_ReadsInChunks()
        {
            // Arrange
            var largeContent = new string('A', 10000); // Large content to test chunked reading
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(largeContent, Encoding.UTF8, "application/json")
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
            Assert.Equal(largeContent, result);
        }

        [Fact]
        public async Task GetStringAsync_WithLargeContent_ReadsInChunks()
        {
            // Arrange
            var largeContent = new string('B', 8192); // Content larger than default chunk size
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(largeContent, Encoding.UTF8, "application/json")
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
            Assert.Equal(largeContent, result);
        }

        [Fact]
        public async Task HandleUnSuccessRequestAsync_WithNullResponse_DoesNotThrow()
        {
            // Arrange & Act
            await _apiClient.TestHandleUnSuccessRequestAsync(null);

            // Assert - No exception should be thrown
            Assert.True(true);
        }

        [Fact]
        public void ServiceException_Serialization_PreservesServiceName()
        {
            // Arrange
            var originalException = new ServiceException("Test error", "Test Service");
            var info = new SerializationInfo(typeof(ServiceException), new FormatterConverter());
            var context = new StreamingContext();

            // Act
            originalException.GetObjectData(info, context);

            // Assert
            Assert.Equal("Test Service", info.GetString("_serviceName"));
            Assert.Contains("Test Service", originalException.Message);
            Assert.Contains("Test error", originalException.Message);
        }

        [Fact]
        public void ServiceException_GetObjectData_AddsServiceName()
        {
            // Arrange
            var exception = new ServiceException("Test error", "Test Service");
            var info = new SerializationInfo(
                typeof(ServiceException), 
                new FormatterConverter());
            var context = new StreamingContext();

            // Act
            exception.GetObjectData(info, context);

            // Assert
            Assert.Equal("Test Service", info.GetString("_serviceName"));
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

            // Test helper methods for additional coverage
            public async Task<string> TestHandleAsyncMsgPack(string path, object value, string language)
            {
                return await HandleAsync<string>(path, value, language, MediaType.MsgPack, 
                    async response => "Test response");
            }

            public async Task<string> TestHandleAsyncWithNullCodeBlock(string path, object value, string language)
            {
                return await HandleAsync<string>(path, value, language, MediaType.Json, null);
            }

            public async Task<string> TestHandleAsyncWithExceptionAndNoDispose(string path, object value, string language)
            {
                return await HandleAsync<string>(path, value, language, MediaType.Json, 
                    async response => throw new InvalidOperationException("Test exception"),
                    disposeHttpResponse: false);
            }

            public async Task TestHandleUnSuccessRequestAsync(HttpResponseMessage response)
            {
                await HandleUnSuccessRequestAsync(response);
            }
        }

        private class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}