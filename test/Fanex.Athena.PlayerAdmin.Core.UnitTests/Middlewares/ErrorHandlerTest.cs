namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Middlewares
{
    using System.Net;
    using AutoFixture;
    using Core.Middlewares;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class ErrorHandlerTest
    {
        private readonly Mock<RequestDelegate> _mockNext;
        private readonly Mock<ILogger<ErrorHandler>> _mockLogger;
        private readonly ErrorHandler _errorHandler;
        private readonly DefaultHttpContext _httpContext;
        private readonly Fixture _fixture;

        public ErrorHandlerTest()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<ErrorHandler>>();
            _errorHandler = new ErrorHandler(_mockNext.Object, _mockLogger.Object);
            _httpContext = new DefaultHttpContext();
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Invoke_WhenNoException_CallsNextMiddleware()
        {
            // Arrange
            _mockNext.Setup(x => x.Invoke(_httpContext))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            // Act
            await _errorHandler.Invoke(_httpContext);

            // Assert
            _mockNext.Verify(x => x.Invoke(_httpContext), Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenExceptionThrown_HandlesErrorCorrectly()
        {
            // Arrange
            var testException = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(_httpContext))
                    .ThrowsAsync(testException);

            // Act
            await _errorHandler.Invoke(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenExceptionThrown_LogsErrorMessage()
        {
            // Arrange
            var testException = new InvalidOperationException("Test exception");
            _mockNext.Setup(x => x.Invoke(_httpContext))
                    .ThrowsAsync(testException);

            // Act
            await _errorHandler.Invoke(_httpContext);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    testException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenExceptionThrown_SetsInternalServerErrorStatus()
        {
            // Arrange
            var testException = new Exception("Test exception");
            _mockNext.Setup(x => x.Invoke(_httpContext))
                    .ThrowsAsync(testException);

            // Act
            await _errorHandler.Invoke(_httpContext);

            // Assert
            Assert.Equal(500, _httpContext.Response.StatusCode);
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenExceptionThrown_ClearsResponse()
        {
            // Arrange
            var testException = new Exception("Test exception");
            _mockNext.Setup(x => x.Invoke(_httpContext))
                    .ThrowsAsync(testException);

            // Pre-populate response to verify it gets cleared
            _httpContext.Response.StatusCode = 200;
            _httpContext.Response.Headers["Test-Header"] = "Test-Value";

            // Act
            await _errorHandler.Invoke(_httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
            Assert.False(_httpContext.Response.Headers.ContainsKey("Test-Header"));
        }

        [Fact]
        public async Task Invoke_WithDifferentExceptionTypes_HandlesAllCorrectly()
        {
            // Arrange & Act & Assert - ArgumentException
            var argumentException = new ArgumentException("Invalid argument");
            _mockNext.Setup(x => x.Invoke(_httpContext)).ThrowsAsync(argumentException);


            await _errorHandler.Invoke(_httpContext);
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);

            // Reset for next test
            _httpContext.Response.Clear();

            // Arrange & Act & Assert - NullReferenceException

            var nullRefException = new NullReferenceException("Null reference");
            _mockNext.Setup(x => x.Invoke(_httpContext)).ThrowsAsync(nullRefException);


            await _errorHandler.Invoke(_httpContext);
            Assert.Equal((int)HttpStatusCode.InternalServerError, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WhenExceptionWithInnerException_LogsCorrectly()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner exception");
            var outerException = new Exception("Outer exception", innerException);
            _mockNext.Setup(x => x.Invoke(_httpContext))
                    .ThrowsAsync(outerException);

            // Act
            await _errorHandler.Invoke(_httpContext);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    outerException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WhenRequestContextHasValues_LogsWithContext()
        {
            // Arrange
            _httpContext.Request.Host = new HostString("test.example.com");
            _httpContext.Request.Path = "/api/test";
            _httpContext.Connection.RemoteIpAddress = IPAddress.Loopback;

            var testException = new Exception("Test exception with context");
            _mockNext.Setup(x => x.Invoke(_httpContext))
                    .ThrowsAsync(testException);

            // Act
            await _errorHandler.Invoke(_httpContext);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("test.example.com")),
                    testException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }


        [Fact]
        public async Task Invoke_MultipleCalls_HandlesEachIndependently()
        {
            // Arrange
            var firstException = new InvalidOperationException("First exception");
            var secondException = new ArgumentException("Second exception");

            // First call throws exception
            _mockNext.SetupSequence(x => x.Invoke(It.IsAny<HttpContext>()))
                    .ThrowsAsync(firstException)
                    .ThrowsAsync(secondException);

            var firstContext = new DefaultHttpContext();
            var secondContext = new DefaultHttpContext();

            // Act
            await _errorHandler.Invoke(firstContext);
            await _errorHandler.Invoke(secondContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, firstContext.Response.StatusCode);
            Assert.Equal((int)HttpStatusCode.InternalServerError, secondContext.Response.StatusCode);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Exactly(2));
        }
    }
}