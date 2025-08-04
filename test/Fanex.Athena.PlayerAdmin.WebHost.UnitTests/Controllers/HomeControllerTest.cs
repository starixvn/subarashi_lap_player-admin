namespace Fanex.Athena.PlayerAdmin.WebHost.UnitTests.Controllers
{
    using System.Diagnostics;
    using Core.Attributes;
    using Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using WebHost.Controllers;
    using WebHost.Models;

    public class HomeControllerTest
    {
        private readonly HomeController _controller;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IWebHostEnvironment> _mockHostingEnvironment;

        public HomeControllerTest()
        {
            _controller = new HomeController();
            _mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            _mockContextService = new Mock<IContextService>();

            // Setup HttpContext for the controller
            var httpContext = new DefaultHttpContext();
            httpContext.TraceIdentifier = "test-trace-id";
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Constructor_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void Index_InDevelopmentEnvironment_ReturnsView()
        {
            // Arrange
            _mockHostingEnvironment.Setup(x => x.EnvironmentName).Returns("Development");

            // Act
            var result = _controller.Index(_mockHostingEnvironment.Object);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void Index_InNonDevelopmentEnvironment_RedirectsToAccessDenied()
        {
            // Arrange
            _mockHostingEnvironment.Setup(x => x.EnvironmentName).Returns("Production");

            // Act
            var result = _controller.Index(_mockHostingEnvironment.Object);

            // Assert
            Assert.NotNull(result);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AccessDenied", redirectResult.ActionName);
        }

        [Fact]
        public void Index_CallsEnvironmentNameOnHostingEnvironment()
        {
            // Arrange
            _mockHostingEnvironment.Setup(x => x.EnvironmentName).Returns("Development");

            // Act
            _controller.Index(_mockHostingEnvironment.Object);

            // Assert
            _mockHostingEnvironment.Verify(x => x.EnvironmentName, Times.AtLeastOnce);
        }

        [Fact]
        public void UserInfo_WithValidContextService_ReturnsJsonWithUserData()
        {
            // Arrange
            const int expectedUserId = 123;
            const string expectedUserName = "testuser";
            const int expectedUserLevel = 5;
            const bool expectedIsMaster = true;
            const int expectedWhitelabel = 456;

            _mockContextService.Setup(x => x.UserId).Returns(expectedUserId);
            _mockContextService.Setup(x => x.Username).Returns(expectedUserName);
            _mockContextService.Setup(x => x.UserLevel).Returns(expectedUserLevel);
            _mockContextService.Setup(x => x.IsMasterLicensee).Returns(expectedIsMaster);
            _mockContextService.Setup(x => x.WhiteLabelID).Returns(expectedWhitelabel);

            // Act
            var result = _controller.UserInfo(_mockContextService.Object);

            // Assert
            Assert.NotNull(result);
            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.NotNull(jsonResult.Value);

            // Use reflection to check the anonymous object properties
            var resultValue = jsonResult.Value;
            var userIdProperty = resultValue.GetType().GetProperty("UserId");
            var userNameProperty = resultValue.GetType().GetProperty("UserName");
            var userLevelProperty = resultValue.GetType().GetProperty("UserLevel");
            var isMasterProperty = resultValue.GetType().GetProperty("IsMaster");
            var whitelabelProperty = resultValue.GetType().GetProperty("Whitelabel");

            Assert.NotNull(userIdProperty);
            Assert.NotNull(userNameProperty);
            Assert.NotNull(userLevelProperty);
            Assert.NotNull(isMasterProperty);
            Assert.NotNull(whitelabelProperty);

            Assert.Equal(expectedUserId, userIdProperty.GetValue(resultValue));
            Assert.Equal(expectedUserName, userNameProperty.GetValue(resultValue));
            Assert.Equal(expectedUserLevel, userLevelProperty.GetValue(resultValue));
            Assert.Equal(expectedIsMaster, isMasterProperty.GetValue(resultValue));
            Assert.Equal(expectedWhitelabel, whitelabelProperty.GetValue(resultValue));
        }

        [Fact]
        public void UserInfo_AccessesAllContextServiceProperties()
        {
            // Arrange
            _mockContextService.Setup(x => x.UserId).Returns(1);
            _mockContextService.Setup(x => x.Username).Returns("test");
            _mockContextService.Setup(x => x.UserLevel).Returns(1);
            _mockContextService.Setup(x => x.IsMasterLicensee).Returns(false);
            _mockContextService.Setup(x => x.WhiteLabelID).Returns(1);

            // Act
            _controller.UserInfo(_mockContextService.Object);

            // Assert
            _mockContextService.Verify(x => x.UserId, Times.Once);
            _mockContextService.Verify(x => x.Username, Times.Once);
            _mockContextService.Verify(x => x.UserLevel, Times.Once);
            _mockContextService.Verify(x => x.IsMasterLicensee, Times.Once);
            _mockContextService.Verify(x => x.WhiteLabelID, Times.Once);
        }

        [Fact]
        public void Error_WithDefaultCode_ReturnsViewWithErrorModel()
        {
            // Act
            var result = _controller.Error();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(500, model.Code);
            Assert.Equal("test-trace-id", model.RequestId);
        }

        [Fact]
        public void Error_WithCustomCode_ReturnsViewWithCustomCode()
        {
            // Arrange
            const int customCode = 404;

            // Act
            var result = _controller.Error(customCode);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(customCode, model.Code);
            Assert.Equal("test-trace-id", model.RequestId);
        }

        [Fact]
        public void Error_WithActivityCurrent_UsesActivityId()
        {
            // Arrange
            const int code = 400;
            using var activity = new Activity("test-activity");
            activity.Start();

            // Act
            var result = _controller.Error(code);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(code, model.Code);
            Assert.Equal(activity.Id, model.RequestId);

            activity.Stop();
        }

        [Fact]
        public void AccessDenied_ReturnsView()
        {
            // Act
            var result = _controller.AccessDenied();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void Index_HasCookieAuthorizeAttribute()
        {
            // Arrange
            var method = typeof(HomeController).GetMethod("Index");

            // Assert
            Assert.NotNull(method);
            var attributes = method.GetCustomAttributes(typeof(CookieAuthorizeAttribute), false);
            Assert.NotEmpty(attributes);
        }

        [Fact]
        public void UserInfo_HasCookieAuthorizeAttribute()
        {
            // Arrange
            var method = typeof(HomeController).GetMethod("UserInfo");

            // Assert
            Assert.NotNull(method);
            var cookieAuthorizeAttributes = method.GetCustomAttributes(typeof(CookieAuthorizeAttribute), false);
            Assert.NotEmpty(cookieAuthorizeAttributes);

            var httpGetAttributes = method.GetCustomAttributes(typeof(HttpGetAttribute), false);
            Assert.NotEmpty(httpGetAttributes);
        }

        [Fact]
        public void Error_HasAllowAnonymousAttribute()
        {
            // Arrange
            var method = typeof(HomeController).GetMethod("Error");

            // Assert
            Assert.NotNull(method);
            var allowAnonymousAttributes = method.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            Assert.NotEmpty(allowAnonymousAttributes);

            var responseCacheAttributes = method.GetCustomAttributes(typeof(ResponseCacheAttribute), false);
            Assert.NotEmpty(responseCacheAttributes);
        }

        [Fact]
        public void AccessDenied_HasAllowAnonymousAttribute()
        {
            // Arrange
            var method = typeof(HomeController).GetMethod("AccessDenied");

            // Assert
            Assert.NotNull(method);
            var allowAnonymousAttributes = method.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
            Assert.NotEmpty(allowAnonymousAttributes);

            var responseCacheAttributes = method.GetCustomAttributes(typeof(ResponseCacheAttribute), false);
            Assert.NotEmpty(responseCacheAttributes);
        }

        [Fact]
        public void Controller_InheritsFromController()
        {
            // Assert
            Assert.True(typeof(Controller).IsAssignableFrom(typeof(HomeController)));
        }

        [Fact]
        public void Controller_HasCorrectNamespace()
        {
            // Assert
            Assert.Equal("Fanex.Athena.PlayerAdmin.WebHost.Controllers", typeof(HomeController).Namespace);
        }
    }
}