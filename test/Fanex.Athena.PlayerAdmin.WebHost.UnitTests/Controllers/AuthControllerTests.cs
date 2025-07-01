using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.WebHost.Controllers;
using Fanex.Athena.PlayerAdmin.WebHost.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net.Http;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.WebHost.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IOptionsSnapshot<IdentityServerSetting>> _mockSettings;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly AuthController _controller;
        private readonly IdentityServerSetting _settings;

        public AuthControllerTests()
        {
            _mockSettings = new Mock<IOptionsSnapshot<IdentityServerSetting>>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClient = new Mock<HttpClient>();

            _settings = new IdentityServerSetting
            {
                Host = "https://test-identity-server.com",
                ExternalApiScope = "external_scope",
                InternalApiScope = "internal_scope",
                GrantType = "test_grant_type",
                TokenEndpoint = "connect/token",
                UserInfoEndpoint = "connect/userinfo",
                CredentialClient = new CredentialsSetting
                {
                    ClientId = "test_client_id",
                    ClientSecret = "test_client_secret"
                }
            };

            _mockSettings.Setup(x => x.Value).Returns(_settings);
            _mockHttpClientFactory.Setup(x => x.CreateClient("Auth")).Returns(_mockHttpClient.Object);

            _controller = new AuthController(_mockSettings.Object, _mockHttpClientFactory.Object);

            // Setup HttpContext
            var httpContext = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesInstance()
        {
            // Act
            var controller = new AuthController(_mockSettings.Object, _mockHttpClientFactory.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void RedirectToLAP_WithValidUrl_RedirectsToLAP()
        {
            // Arrange
            var lapUrl = "test-lap.com";
            var lapVer = "v1";

            // Act
            var result = _controller.RedirectToLAP(lapUrl, lapVer);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("http://test-lap.com/v1/12bet/Auth/Login?returnUrl=/PlayerAdminContainer/Index", redirectResult.Url);
        }

        [Fact]
        public void RedirectToLAP_WithEmptyVersion_RedirectsToLAPWithoutVersion()
        {
            // Arrange
            var lapUrl = "test-lap.com";
            var lapVer = "";

            // Act
            var result = _controller.RedirectToLAP(lapUrl, lapVer);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("http://test-lap.com//12bet/Auth/Login?returnUrl=/PlayerAdminContainer/Index", redirectResult.Url);
        }

        [Fact]
        public void RedirectToLAP_WithNullVersion_RedirectsToLAPWithoutVersion()
        {
            // Arrange
            var lapUrl = "test-lap.com";

            // Act
            var result = _controller.RedirectToLAP(lapUrl);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("http://test-lap.com//12bet/Auth/Login?returnUrl=/PlayerAdminContainer/Index", redirectResult.Url);
        }

        [Fact]
        public void RedirectToLAP_WithEmptyUrl_RedirectsToLAPWithEmptyUrl()
        {
            // Arrange
            var lapUrl = "";
            var lapVer = "v1";

            // Act
            var result = _controller.RedirectToLAP(lapUrl, lapVer);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("http:///v1/12bet/Auth/Login?returnUrl=/PlayerAdminContainer/Index", redirectResult.Url);
        }

        [Fact]
        public void RedirectToLAP_WithNullUrl_RedirectsToLAPWithNullUrl()
        {
            // Arrange
            string lapUrl = null;
            var lapVer = "v1";

            // Act
            var result = _controller.RedirectToLAP(lapUrl, lapVer);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("http:///v1/12bet/Auth/Login?returnUrl=/PlayerAdminContainer/Index", redirectResult.Url);
        }

        [Fact]
        public void RedirectToLAP_WithSpecialCharacters_RedirectsToLAPWithEncodedUrl()
        {
            // Arrange
            var lapUrl = "test-lap.com/path with spaces";
            var lapVer = "v1";

            // Act
            var result = _controller.RedirectToLAP(lapUrl, lapVer);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("http://test-lap.com/path with spaces/v1/12bet/Auth/Login?returnUrl=/PlayerAdminContainer/Index", redirectResult.Url);
        }
    }
} 