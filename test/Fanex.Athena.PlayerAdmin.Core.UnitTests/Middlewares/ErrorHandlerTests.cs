using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Middlewares
{
    public class ErrorHandlerTests
    {
        private readonly Mock<ILogger<ErrorHandler>> _loggerMock;
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly DefaultHttpContext _httpContext;

        public ErrorHandlerTests()
        {
            _loggerMock = new Mock<ILogger<ErrorHandler>>();
            _nextMock = new Mock<RequestDelegate>();
            _httpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task Invoke_NoException_CallsNextMiddleware()
        {
            // Arrange
            var nextCalled = false;
            RequestDelegate next = ctx => 
            { 
                nextCalled = true; 
                return Task.CompletedTask; 
            };
            var handler = new ErrorHandler(next, _loggerMock.Object);

            // Act
            await handler.Invoke(_httpContext);

            // Assert
            Assert.True(nextCalled);
            _loggerMock.Verify(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Fact]
        public async Task Invoke_WithException_LogsErrorAndSetsResponse()
        {
            // Arrange
            var exception = new InvalidOperationException("Test exception");
            RequestDelegate next = ctx => throw exception;
            var handler = new ErrorHandler(next, _loggerMock.Object);

            // Act
            await handler.Invoke(_httpContext);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
            
            Assert.Equal(500, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WithException_ClearsResponse()
        {
            // Arrange
            var exception = new ArgumentException("Test exception");
            RequestDelegate next = ctx => throw exception;
            var handler = new ErrorHandler(next, _loggerMock.Object);

            // Set some content to verify it gets cleared
            _httpContext.Response.Body = new System.IO.MemoryStream();
            await _httpContext.Response.WriteAsync("Some content");

            // Act
            await handler.Invoke(_httpContext);

            // Assert
            _httpContext.Response.Body.Position = 0;
            var content = await new System.IO.StreamReader(_httpContext.Response.Body).ReadToEndAsync();
            Assert.Empty(content);
        }

        [Fact]
        public async Task Invoke_WithDifferentExceptionTypes_HandlesCorrectly()
        {
            // Arrange
            var exceptions = new Exception[]
            {
                new ArgumentNullException("param"),
                new InvalidOperationException("Invalid operation"),
                new NotSupportedException("Not supported"),
                new TimeoutException("Timeout")
            };

            foreach (var exception in exceptions)
            {
                // Arrange for each exception
                RequestDelegate next = ctx => throw exception;
                var handler = new ErrorHandler(next, _loggerMock.Object);
                var context = new DefaultHttpContext();

                // Act
                await handler.Invoke(context);

                // Assert
                _loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Critical,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        exception,
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
                
                Assert.Equal(500, context.Response.StatusCode);
            }
        }

        [Fact]
        public async Task Invoke_WithAggregateException_HandlesCorrectly()
        {
            // Arrange
            var innerException1 = new ArgumentException("Inner exception 1");
            var innerException2 = new InvalidOperationException("Inner exception 2");
            var aggregateException = new AggregateException(innerException1, innerException2);
            
            RequestDelegate next = ctx => throw aggregateException;
            var handler = new ErrorHandler(next, _loggerMock.Object);

            // Act
            await handler.Invoke(_httpContext);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    aggregateException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
            
            Assert.Equal(500, _httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_WithException_LogsCorrectMessage()
        {
            // Arrange
            var exception = new InvalidOperationException("Test exception message");
            RequestDelegate next = ctx => throw exception;
            var handler = new ErrorHandler(next, _loggerMock.Object);

            // Act
            await handler.Invoke(_httpContext);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Test exception message")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_WithException_IncludesRequestInfoInLog()
        {
            // Arrange
            var exception = new Exception("Test exception");
            RequestDelegate next = ctx => throw exception;
            var handler = new ErrorHandler(next, _loggerMock.Object);

            // Set up request info
            _httpContext.Request.Host = new Microsoft.AspNetCore.Http.HostString("localhost", 5000);
            _httpContext.Request.Path = "/test/path";

            // Act
            await handler.Invoke(_httpContext);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("REQUEST INFO")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
} 