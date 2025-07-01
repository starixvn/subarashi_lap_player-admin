using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.HealthCheck;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.HealthCheck
{
    public class WhoAmIHealthCheckTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IWebHostEnvironment> _envMock;
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<ConnectionInfo> _connectionInfoMock;
        private readonly Mock<HttpRequest> _httpRequestMock;
        private readonly WhoAmIHealthCheck _healthCheck;

        public WhoAmIHealthCheckTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _envMock = new Mock<IWebHostEnvironment>();
            _httpContextMock = new Mock<HttpContext>();
            _connectionInfoMock = new Mock<ConnectionInfo>();
            _httpRequestMock = new Mock<HttpRequest>();

            // Setup mocks
            _envMock.Setup(x => x.EnvironmentName).Returns("TestEnvironment");
            
            _connectionInfoMock.Setup(x => x.LocalIpAddress).Returns(IPAddress.Parse("127.0.0.1"));
            _connectionInfoMock.Setup(x => x.LocalPort).Returns(5000);
            _connectionInfoMock.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse("192.168.1.1"));
            
            _httpRequestMock.Setup(x => x.Host).Returns(new HostString("localhost", 5000));
            _httpRequestMock.Setup(x => x.Scheme).Returns("https");
            
            _httpContextMock.Setup(x => x.Connection).Returns(_connectionInfoMock.Object);
            _httpContextMock.Setup(x => x.Request).Returns(_httpRequestMock.Object);
            
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(_httpContextMock.Object);

            _healthCheck = new WhoAmIHealthCheck(_httpContextAccessorMock.Object, _envMock.Object);
        }

        [Fact]
        public async Task CheckHealthAsync_ReturnsHealthyResult()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Equal("Server Info", result.Description);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesEnvironmentInfo()
        {
            // Arrange
            _envMock.Setup(x => x.EnvironmentName).Returns("Production");

            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Env"));
            Assert.Equal("Production", result.Data["Env"]);
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesHostName()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Host Name"));
            Assert.NotNull(result.Data["Host Name"]);
            Assert.NotEmpty(result.Data["Host Name"].ToString());
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesLocalIpAddress()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Local Ip"));
            Assert.Equal("127.0.0.1", result.Data["Local Ip"]);
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesLocalPort()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Local Port"));
            Assert.Equal(5000, result.Data["Local Port"]);
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesClientIpAddress()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Client Ip"));
            Assert.Equal("192.168.1.1", result.Data["Client Ip"]);
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesServerTime()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Server Time"));
            Assert.NotNull(result.Data["Server Time"]);
            
            // Verify it's a valid date
            var serverTime = result.Data["Server Time"].ToString();
            Assert.True(DateTime.TryParse(serverTime, out _));
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesLastUpdated()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Last Upadted"));
            Assert.NotNull(result.Data["Last Upadted"]);
            
            // Verify it's a valid date
            var lastUpdated = result.Data["Last Upadted"].ToString();
            Assert.True(DateTime.TryParse(lastUpdated, out _));
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesHost()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Host"));
            Assert.Equal("localhost:5000", result.Data["Host"].ToString());
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesSchema()
        {
            // Act
            var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("Schema"));
            Assert.Equal("https", result.Data["Schema"]);
        }

        [Fact]
        public async Task CheckHealthAsync_WithNullHttpContext_ThrowsException()
        {
            // Arrange
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None));
        }

        [Fact]
        public async Task CheckHealthAsync_WithNullConnection_ThrowsException()
        {
            // Arrange
            _httpContextMock.Setup(x => x.Connection).Returns((ConnectionInfo)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None));
        }

        [Fact]
        public async Task CheckHealthAsync_WithNullRequest_ThrowsException()
        {
            // Arrange
            _httpContextMock.Setup(x => x.Request).Returns((HttpRequest)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _healthCheck.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None));
        }
    }
} 