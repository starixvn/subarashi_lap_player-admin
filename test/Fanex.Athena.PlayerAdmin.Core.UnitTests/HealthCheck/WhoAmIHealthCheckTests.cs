namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.HealthCheck
{
    using System.Net;
    using Core.HealthCheck;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.FileProviders;

    public class WhoAmIHealthCheckTests
    {
        [Fact]
        public async Task CheckHealthAsync_ReturnsHealthyAndContainsExpectedKeys()
        {
            var context = new DefaultHttpContext();
            context.Connection.LocalIpAddress = IPAddress.Loopback;
            context.Connection.RemoteIpAddress = IPAddress.Loopback;
            context.Connection.LocalPort = 5000;
            context.Request.Scheme = "http";
            context.Request.Host = new HostString("localhost", 5000);

            var accessor = new HttpContextAccessor { HttpContext = context };
            var env = new TestEnv();
            var check = new WhoAmIHealthCheck(accessor, env);

            var hcContext = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("WhoAmI", check, HealthStatus.Unhealthy, null)
            };

            var result = await check.CheckHealthAsync(hcContext);
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Contains("Env", result.Data.Keys);
            Assert.Contains("Host", result.Data.Keys);
            Assert.Contains("Schema", result.Data.Keys);
        }

        private sealed class TestEnv : IWebHostEnvironment
        {
            public string ApplicationName { get; set; } = "app";
            public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
            public string WebRootPath { get; set; } = string.Empty;
            public string EnvironmentName { get; set; } = "Test";
            public string ContentRootPath { get; set; } = string.Empty;
            public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
        }
    }
}