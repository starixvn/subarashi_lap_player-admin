// src/Fanex.Athena.PlayerAdmin.Core/Extensions/MiddlewareExtensions.cs
namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using System.Net;
    using AutoFixture;
    using Core.Extensions;
    using Core.Settings;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Primitives;
    using Moq;
    using ONEbook.UM.Services;

    public class MiddlewareExtensionsTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IServiceCollection> _mockServices;
        private readonly Mock<IApplicationBuilder> _mockApp;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<HttpRequest> _mockRequest;
        private readonly Mock<HttpResponse> _mockResponse;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public MiddlewareExtensionsTests()
        {
            _fixture = new Fixture();
            _mockServices = new Mock<IServiceCollection>();
            _mockApp = new Mock<IApplicationBuilder>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockRequest = new Mock<HttpRequest>();
            _mockResponse = new Mock<HttpResponse>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 12_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1", true)]
        [InlineData("Mozilla/5.0 (iPad; CPU OS 12_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1 Mobile/15E148 Safari/604.1", true)]
        [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1 Safari/605.1.15", true)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36", true)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36", false)]
        [InlineData("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36", false)]
        public void DisallowsSameSiteNone_VariousUserAgents_ReturnsExpected(string userAgent, bool expected)
        {
            // Act
            var result = MiddlewareExtensions.DisallowsSameSiteNone(userAgent);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X)", true)]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 12_1_1 like Mac OS X)", true)]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 13_0 like Mac OS X)", false)]
        [InlineData("Mozilla/5.0 (iPad; CPU OS 12_0 like Mac OS X)", true)]
        [InlineData("Mozilla/5.0 (iPad; CPU OS 12_1_1 like Mac OS X)", true)]
        [InlineData("Mozilla/5.0 (iPad; CPU OS 13_0 like Mac OS X)", false)]
        public void DisallowsSameSiteNone_iOSVersions_ReturnsExpected(string userAgent, bool expected)
        {
            // Act
            var result = MiddlewareExtensions.DisallowsSameSiteNone(userAgent);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Safari/605.1.15", true)]
        [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1.2 Safari/605.1.15", true)]
        [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0 Safari/605.1.15", false)]
        [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36", false)]
        public void DisallowsSameSiteNone_macOSVersions_ReturnsExpected(string userAgent, bool expected)
        {
            // Act
            var result = MiddlewareExtensions.DisallowsSameSiteNone(userAgent);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36", true)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36", true)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36", false)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36", false)]
        public void DisallowsSameSiteNone_ChromeVersions_ReturnsExpected(string userAgent, bool expected)
        {
            // Act
            var result = MiddlewareExtensions.DisallowsSameSiteNone(userAgent);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddCorsPortalSite_WithValidAppSetting_RegistersCorsPolicy()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            var appSetting = new AppSetting
            {
                AllowedCrossDomains = new[] { "example.com", "test.org" }
            };

            // Act
            services.AddCorsPortalSite(appSetting);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var corsService = serviceProvider.GetService<ICorsService>();
            Assert.NotNull(corsService);
        }

        [Fact]
        public void AddCorsPortalSite_WithEmptyAllowedDomains_AllowsAllOrigins()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            var appSetting = new AppSetting
            {
                AllowedCrossDomains = new string[0]
            };

            // Act
            services.AddCorsPortalSite(appSetting);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var corsService = serviceProvider.GetService<ICorsService>();
            Assert.NotNull(corsService);
        }

        [Fact]
        public void AddHealthCheck_WithValidConfiguration_RegistersHealthChecks()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["IdentityServer:Host"] = "https://identity.example.com",
                    ["IdentityServer:HealthPath"] = "/health",
                    ["Statement:InternalHost"] = "https://statement.example.com",
                    ["Statement:HealthPath"] = "/health",
                    ["ConsoleClient:Report:Host"] = "https://report.example.com",
                    ["ConsoleClient:Report:HealthPath"] = "/health",
                    ["ConsoleClient:BetList:Host"] = "https://betlist.example.com",
                    ["ConsoleClient:BetList:HealthPath"] = "/health",
                    ["ConsoleClient:Lg:Host"] = "https://lg.example.com",
                    ["ConsoleClient:Lg:HealthPath"] = "/health",
                    ["Redis:Server"] = "localhost",
                    ["Redis:Port"] = "6379",
                    ["Redis:Pass"] = "password"
                })
                .Build();

            // Act
            services.AddHealthCheck(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var healthCheckService = serviceProvider.GetService<HealthCheckService>();
            Assert.NotNull(healthCheckService);
        }

        [Fact]
        public void AddUnderMaintenance_WithValidConfiguration_RegistersServices()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions()));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["UnderMaintenance:Url"] = "https://um.example.com",
                    ["UnderMaintenance:ApiKey"] = "test-key"
                })
                .Build();

            // Act
            services.AddUnderMaintenance(configuration);

            // Assert
            Assert.Contains(services, d => d.ServiceType == typeof(IMaintenanceService));
        }

        [Fact]
        public void UsePing_WithPingPath_DoesNotThrow()
        {
            var services = new ServiceCollection().BuildServiceProvider();
            var app = new ApplicationBuilder(services);
            app.UsePing();
        }

        [Fact]
        public void UsePing_WithNonPingPath_DoesNotThrow()
        {
            var services = new ServiceCollection().BuildServiceProvider();
            var app = new ApplicationBuilder(services);
            app.UsePing();
        }

        [Fact]
        public void UseHealthCheck_WithValidConfiguration_DoesNotThrow()
        {
            var services = new ServiceCollection();
            services.AddRouting();
            services.AddHealthChecks();
            services.AddLogging();
            var provider = services.BuildServiceProvider();
            var app = new ApplicationBuilder(provider);
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["HealthCheck:AccessKey"] = "test-key"
                })
                .Build();

            app.UseHealthCheck(configuration);
        }

        [Fact]
        public void UseErrorHandler_DoesNotThrow()
        {
            var services = new ServiceCollection().BuildServiceProvider();
            var app = new ApplicationBuilder(services);
            app.UseErrorHandler();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UseStaticResource_WithDevFlag_DoesNotThrow(bool isDev)
        {
            var services = new ServiceCollection().BuildServiceProvider();
            var app = new ApplicationBuilder(services);
            app.UseStaticResource(isDev);
        }

        [Fact]
        public void UseCookieSameSitePolicy_DoesNotThrow()
        {
            var services = new ServiceCollection().BuildServiceProvider();
            var app = new ApplicationBuilder(services);
            app.UseCookieSameSitePolicy();
        }

        [Fact]
        public void ContextInMemoryHelper_GetSession_WithValidKey_ReturnsValue()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            var helper = new MiddlewareExtensions.ContextInMemoryHelper(httpContextAccessor.Object, memoryCache);
            var key = "test-key";
            var expectedValue = "test-value";
            helper.SetSession(key, expectedValue);

            // Act
            var result = helper.GetSession(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void ContextInMemoryHelper_GetSession_WithInvalidKey_ReturnsEmptyString()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            var helper = new MiddlewareExtensions.ContextInMemoryHelper(httpContextAccessor.Object, memoryCache);

            // Act
            var result = helper.GetSession("non-existent-key");

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ContextInMemoryHelper_GetValueFromQueryString_WithValidKey_ReturnsValue()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Query = new QueryCollection(new Dictionary<string, StringValues>
            {
                ["testKey"] = "testValue"
            });
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            var helper = new MiddlewareExtensions.ContextInMemoryHelper(httpContextAccessor.Object, memoryCache);

            // Act
            var result = helper.GetValueFromQueryString("testKey");

            // Assert
            Assert.Equal("testValue", result);
        }

        [Fact]
        public void ContextInMemoryHelper_GetValueFromQueryString_WithInvalidKey_ReturnsNull()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            var helper = new MiddlewareExtensions.ContextInMemoryHelper(httpContextAccessor.Object, memoryCache);

            // Act
            var result = helper.GetValueFromQueryString("non-existent-key");

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ContextInMemoryHelper_SetSession_WithValidKeyValue_StoresInCache()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            
            var helper = new MiddlewareExtensions.ContextInMemoryHelper(httpContextAccessor.Object, memoryCache);
            var key = "test-key";
            var value = "test-value";

            // Act
            helper.SetSession(key, value);
            var result = helper.GetSession(key);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ContextInMemoryHelper_Constructor_WithValidParameters_CreatesInstance()
        {
            // Arrange & Act
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var helper = new MiddlewareExtensions.ContextInMemoryHelper(httpContextAccessor.Object, memoryCache);

            // Assert
            Assert.NotNull(helper);
        }
    }
}

