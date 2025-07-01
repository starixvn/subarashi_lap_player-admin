using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.ConsoleClient
{
    public class BaseApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly TestApiClient _testApiClient;

        public BaseApiClientTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/")
            };
            _testApiClient = new TestApiClient(_httpClient);
        }

        [Fact]
        public async Task PostAsync_SuccessfulRequest_ReturnsDeserializedObject()
        {
            // Arrange
            var expectedResponse = new TestResponse { Data = "Success" };
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse)),
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(responseMessage);

            // Act
            var result = await _testApiClient.PostAsync<TestResponse>("testpath", new { }, "en-US");

            // Assert
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task PostStringAsync_SuccessfulRequest_ReturnsString()
        {
            // Arrange
            var expectedResponse = "Success";
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse),
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(responseMessage);

            // Act
            var result = await _testApiClient.PostStringAsync("testpath", new { }, "en-US");

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task PostAsync_UnsuccessfulRequest_ThrowsHttpRequestException()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
            };

            _handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(responseMessage);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ServiceException>(() => _testApiClient.PostAsync<TestResponse>("testpath", new { }, "en-US"));
            Assert.IsType<HttpRequestException>(exception.InnerException);
        }
    }

    public class TestApiClient : BaseApiClient
    {
        protected override string ApiName => "TestApi";

        public TestApiClient(HttpClient client) : base(client)
        {
        }
    }

    public class TestResponse
    {
        public string Data { get; set; }
    }
} 