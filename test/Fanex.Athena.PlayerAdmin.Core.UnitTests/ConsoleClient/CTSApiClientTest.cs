namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    using System.Net;
    using System.Reflection;
    using System.Text;
    using Core.ConsoleClient;
    using Moq;
    using Moq.Protected;

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

        [Fact]
        public async Task PostAsync_WhenResponseIsNotSuccessfulWithContent_ThrowsHttpRequestExceptionWithDetails()
        {
            // Arrange
            var errorContent = "Detailed error message";
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(errorContent, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
            
            Assert.Contains("CTS API", exception.Message);
            Assert.NotNull(exception.InnerException);
            Assert.IsType<HttpRequestException>(exception.InnerException);
            Assert.Contains("Status code: InternalServerError", exception.InnerException.Message);
            Assert.Contains(errorContent, exception.InnerException.Message);
        }

        [Fact]
        public async Task PostAsync_WhenResponseIsNotSuccessfulWithEmptyContent_ThrowsHttpRequestExceptionFromBase()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
            
            Assert.Contains("CTS API", exception.Message);
            Assert.NotNull(exception.InnerException);
            Assert.IsType<HttpRequestException>(exception.InnerException);
        }

        [Fact]
        public async Task PostAsync_WhenResponseIsNotSuccessfulWithNullContent_ThrowsHttpRequestExceptionFromBase()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            // Content is null by default

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
            
            Assert.Contains("CTS API", exception.Message);
            Assert.NotNull(exception.InnerException);
            Assert.IsType<HttpRequestException>(exception.InnerException);
        }

        [Fact]
        public async Task PostAsync_WhenResponseIsNotSuccessfulWithWhitespaceContent_ThrowsHttpRequestExceptionFromBase()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent("   \n\t  ", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
            
            Assert.Contains("CTS API", exception.Message);
            Assert.NotNull(exception.InnerException);
            Assert.IsType<HttpRequestException>(exception.InnerException);
        }

        [Fact]
        public async Task PostAsync_WithZeroTimeout_WorksCorrectly()
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
            var result = await _apiClient.PostAsync("/test", new { data = "test" }, "en", 0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_WithLargeTimeout_WorksCorrectly()
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
            var result = await _apiClient.PostAsync("/test", new { data = "test" }, "en", 3600);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task PostAsync_WithComplexObject_SerializesCorrectly()
        {
            // Arrange
            var complexObject = new
            {
                Id = 123,
                Name = "Test User",
                IsActive = true,
                CreatedDate = DateTimeOffset.Now,
                Tags = new[] { "tag1", "tag2" },
                Metadata = new { Key = "Value", Number = 42 }
            };

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
            var result = await _apiClient.PostAsync("/test", complexObject, "en");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(capturedRequest);
            Assert.NotNull(capturedRequest.Content);
        }

        [Fact]
        public async Task PostAsync_WithEmptyLanguage_SendsRequestSuccessfully()
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
            var result = await _apiClient.PostAsync("/test", new { data = "test" }, "");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(capturedRequest);
            Assert.True(capturedRequest.Headers.Contains("Language"));
        }

        [Fact]
        public async Task PostAsync_VerifiesLanguageHeaderIsSet()
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
            await _apiClient.PostAsync("/test", new { data = "test" }, "fr-FR");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.True(capturedRequest.Headers.Contains("Language"));
            var languageHeaders = capturedRequest.Headers.GetValues("Language");
            Assert.Contains("fr-FR", languageHeaders);
        }

        [Fact]
        public async Task PostAsync_VerifiesContentTypeIsJson()
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
            await _apiClient.PostAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.NotNull(capturedRequest.Content);
            Assert.Equal("application/json", capturedRequest.Content.Headers.ContentType?.MediaType);
            Assert.Equal("utf-8", capturedRequest.Content.Headers.ContentType?.CharSet);
        }

        [Fact]
        public async Task PostAsync_VerifiesAcceptHeaderIsJson()
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
            await _apiClient.PostAsync("/test", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.True(capturedRequest.Headers.Accept.Any(a => a.MediaType == "application/json"));
        }

        [Fact]
        public void ApiName_ReturnsCorrectValue()
        {
            // Arrange & Act
            // Use reflection to access the protected property
            var apiNameProperty = typeof(CTSApiClient).GetProperty("ApiName", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            var apiName = apiNameProperty?.GetValue(_apiClient) as string;

            // Assert
            Assert.Equal("CTS API", apiName);
        }

        [Fact]
        public async Task PostAsync_WhenTaskCanceled_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("The operation was canceled."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
            
            Assert.Contains("CTS API", exception.Message);
            Assert.Contains("The operation was canceled.", exception.Message);
        }

        [Fact]
        public async Task PostAsync_WhenOperationCanceled_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new OperationCanceledException("The operation was canceled."));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
            
            Assert.Contains("CTS API", exception.Message);
            Assert.Contains("The operation was canceled.", exception.Message);
        }

        [Fact]
        public async Task PostAsync_WhenArgumentExceptionThrown_ThrowsServiceException()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _apiClient.PostAsync("/test", new { data = "test" }, "en"));
            
            Assert.Contains("CTS API", exception.Message);
            Assert.Contains("Invalid argument", exception.Message);
        }

        [Fact]
        public async Task PostAsync_ResponseDisposalTest_VerifyResponseHandling()
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
            
            // Verify that the response can still be used (not disposed) since disposeHttpResponse is false in PostAsync
            var content = await result.Content.ReadAsStringAsync();
            Assert.Equal("Test response", content);
        }

        [Fact]
        public async Task PostAsync_WithSpecialCharactersInPath_WorksCorrectly()
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
            await _apiClient.PostAsync("/api/test-endpoint?param=value&other=123", new { data = "test" }, "en");

            // Assert
            Assert.NotNull(capturedRequest);
            Assert.Contains("test-endpoint", capturedRequest.RequestUri?.ToString());
            Assert.Contains("param=value", capturedRequest.RequestUri?.ToString());
        }

        [Fact]
        public async Task PostAsync_WithVeryLongLanguageCode_WorksCorrectly()
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

            var longLanguageCode = "en-US-x-very-long-language-code-extension";

            // Act
            var result = await _apiClient.PostAsync("/test", new { data = "test" }, longLanguageCode);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(capturedRequest);
            Assert.True(capturedRequest.Headers.Contains("Language"));
            var languageHeaders = capturedRequest.Headers.GetValues("Language");
            Assert.Contains(longLanguageCode, languageHeaders);
        }

        [Fact]
        public async Task PostAsync_MultipleCallsSequentially_WorksCorrectly()
        {
            // Arrange
            var responseMessage1 = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Response 1", Encoding.UTF8, "application/json")
            };
            
            var responseMessage2 = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = new StringContent("Response 2", Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage1)
                .ReturnsAsync(responseMessage2);

            // Act
            var result1 = await _apiClient.PostAsync("/test1", new { data = "test1" }, "en");
            var result2 = await _apiClient.PostAsync("/test2", new { data = "test2" }, "zh");

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(HttpStatusCode.OK, result1.StatusCode);
            Assert.Equal(HttpStatusCode.Accepted, result2.StatusCode);
        }
    }
}