namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using System.Net;
    using Core.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Moq;

    public class HttpRequestExtensionTest
    {
        [Fact]
        public void IsAjaxRequest_ReturnsTrue_WhenXRequestedWithHeaderIsXMLHttpRequest()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary
            {
                { "X-Requested-With", "XMLHttpRequest" }
            };
            mockRequest.Setup(x => x.Headers).Returns(headers);

            // Act
            var result = mockRequest.Object.IsAjaxRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAjaxRequest_ReturnsTrue_WhenXRequestedWithHeaderIsCaseInsensitive()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary
            {
                { "X-Requested-With", "xmlhttprequest" }
            };
            mockRequest.Setup(x => x.Headers).Returns(headers);

            // Act
            var result = mockRequest.Object.IsAjaxRequest();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsAjaxRequest_ReturnsFalse_WhenXRequestedWithHeaderIsMissing()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary();
            mockRequest.Setup(x => x.Headers).Returns(headers);

            // Act
            var result = mockRequest.Object.IsAjaxRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAjaxRequest_ReturnsFalse_WhenXRequestedWithHeaderHasWrongValue()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary
            {
                { "X-Requested-With", "SomeOtherValue" }
            };
            mockRequest.Setup(x => x.Headers).Returns(headers);

            // Act
            var result = mockRequest.Object.IsAjaxRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsAjaxRequest_ReturnsFalse_WhenXRequestedWithHeaderIsEmpty()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary
            {
                { "X-Requested-With", "" }
            };
            mockRequest.Setup(x => x.Headers).Returns(headers);

            // Act
            var result = mockRequest.Object.IsAjaxRequest();

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("/styles/main.css", true)]
        [InlineData("/scripts/app.js", true)]
        [InlineData("/images/logo.png", true)]
        [InlineData("/favicon.ico", true)]
        [InlineData("/file.txt", true)]
        public void IsStaticRequest_ReturnsTrue_WhenPathContainsDot(string path, bool expected)
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Path).Returns(new PathString(path));

            // Act
            var result = mockRequest.Object.IsStaticRequest();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("/home/index", false)]
        [InlineData("/api/users", false)]
        [InlineData("/", false)]
        [InlineData("/admin", false)]
        [InlineData("", false)]
        public void IsStaticRequest_ReturnsFalse_WhenPathDoesNotContainDot(string path, bool expected)
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Path).Returns(new PathString(path));

            // Act
            var result = mockRequest.Object.IsStaticRequest();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsStaticRequest_ReturnsFalse_WhenPathIsNull()
        {
            // Arrange
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Path).Returns(new PathString(null));

            // Act
            var result = mockRequest.Object.IsStaticRequest();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HandleInvalidRequest_SetsJsonResult_WhenRequestIsAjax()
        {
            // Arrange
            var errorMsg = "Test error message";
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary
            {
                { "X-Requested-With", "XMLHttpRequest" }
            };
            mockRequest.Setup(x => x.Headers).Returns(headers);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

            var actionContext = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = mockHttpContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<object>().Object
            );

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.IsType<JsonResult>(actionContext.Result);
            var jsonResult = (JsonResult)actionContext.Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, jsonResult.StatusCode);
        }

        [Fact]
        public void HandleInvalidRequest_SetsRedirectResult_WhenRequestIsNotAjax()
        {
            // Arrange
            var errorMsg = "Test error message";
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary();
            mockRequest.Setup(x => x.Headers).Returns(headers);

            var mockResponse = new Mock<HttpResponse>();
            var responseHeaders = new HeaderDictionary();
            mockResponse.Setup(x => x.Headers).Returns(responseHeaders);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);

            var actionContext = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = mockHttpContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<object>().Object
            );

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            Assert.IsType<RedirectResult>(actionContext.Result);
            var redirectResult = (RedirectResult)actionContext.Result;
            Assert.Equal("/Home/Error?code=400", redirectResult.Url);
            Assert.True(responseHeaders.ContainsKey("error-msg"));
            Assert.Equal(errorMsg, responseHeaders["error-msg"]);
        }

        [Fact]
        public void HandleInvalidRequest_JsonResultHasCorrectStructure_WhenRequestIsAjax()
        {
            // Arrange
            var errorMsg = "Validation failed";
            var mockRequest = new Mock<HttpRequest>();
            var headers = new HeaderDictionary
            {
                { "X-Requested-With", "XMLHttpRequest" }
            };
            mockRequest.Setup(x => x.Headers).Returns(headers);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

            var actionContext = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = mockHttpContext.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<object>().Object
            );

            // Act
            actionContext.HandleInvalidRequest(errorMsg);

            // Assert
            var jsonResult = (JsonResult)actionContext.Result;
            var value = jsonResult.Value;

            // Use reflection to check the anonymous object properties

            var valueType = value.GetType();
            var messageProperty = valueType.GetProperty("Message");
            var isSuccessProperty = valueType.GetProperty("IsSuccess");
            var codeProperty = valueType.GetProperty("Code");

            Assert.Equal(errorMsg, messageProperty.GetValue(value));
            Assert.False((bool)isSuccessProperty.GetValue(value));
            Assert.Equal(1, codeProperty.GetValue(value));
        }
    }
}