using System;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.HealthCheck
{
    public class MemoryHealthCheckTests
    {
        private readonly Mock<IOptionsMonitor<MemoryCheckOptions>> _optionsMonitorMock;
        private readonly MemoryHealthCheck _healthCheck;

        public MemoryHealthCheckTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<MemoryCheckOptions>>();
            _healthCheck = new MemoryHealthCheck(_optionsMonitorMock.Object);
        }

        [Fact]
        public async Task CheckHealthAsync_WithLowMemoryUsage_ReturnsHealthy()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 1024 }; // 1GB
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Contains("Reports degraded status if allocated bytes >= 1024 mbs.", result.Description);
            Assert.NotNull(result.Data);
            Assert.True(result.Data.ContainsKey("AllocatedMBs"));
            Assert.True(result.Data.ContainsKey("Gen0Collections"));
            Assert.True(result.Data.ContainsKey("Gen1Collections"));
            Assert.True(result.Data.ContainsKey("Gen2Collections"));
        }

        [Fact]
        public async Task CheckHealthAsync_WithHighMemoryUsage_ReturnsDegraded()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 1 }; // 1MB (very low threshold)
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            Assert.Equal(HealthStatus.Unhealthy, result.Status);
            Assert.Contains("Reports degraded status if allocated bytes >= 1 mbs.", result.Description);
        }

        [Fact]
        public async Task CheckHealthAsync_WithCustomFailureStatus_ReturnsCustomStatus()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 1 }; // 1MB (very low threshold)
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Degraded, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            Assert.Equal(HealthStatus.Degraded, result.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_IncludesMemoryData()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 1024 };
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            Assert.True(result.Data.ContainsKey("AllocatedMBs"));
            Assert.True(result.Data.ContainsKey("Gen0Collections"));
            Assert.True(result.Data.ContainsKey("Gen1Collections"));
            Assert.True(result.Data.ContainsKey("Gen2Collections"));

            // Verify data types
            Assert.IsType<long>(result.Data["AllocatedMBs"]);
            Assert.IsType<int>(result.Data["Gen0Collections"]);
            Assert.IsType<int>(result.Data["Gen1Collections"]);
            Assert.IsType<int>(result.Data["Gen2Collections"]);
        }

        [Fact]
        public async Task CheckHealthAsync_WithZeroThreshold_ReturnsUnhealthy()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 0 };
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            // With zero threshold, any memory usage will exceed it, so it should be unhealthy
            Assert.Equal(HealthStatus.Unhealthy, result.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_WithVeryHighThreshold_AlwaysReturnsHealthy()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = long.MaxValue };
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_WithDifferentRegistrationNames_UsesCorrectOptions()
        {
            // Arrange
            var options1 = new MemoryCheckOptions { Threshold = 1024 };
            var options2 = new MemoryCheckOptions { Threshold = 1 };

            _optionsMonitorMock.Setup(x => x.Get("check1")).Returns(options1);
            _optionsMonitorMock.Setup(x => x.Get("check2")).Returns(options2);

            var context1 = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("check1", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            var context2 = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("check2", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result1 = await _healthCheck.CheckHealthAsync(context1, CancellationToken.None);
            var result2 = await _healthCheck.CheckHealthAsync(context2, CancellationToken.None);

            // Assert
            Assert.Equal(HealthStatus.Healthy, result1.Status);
            Assert.Equal(HealthStatus.Unhealthy, result2.Status);
        }

        [Fact]
        public async Task CheckHealthAsync_WithNullRegistration_ThrowsException()
        {
            // Arrange
            var context = new HealthCheckContext { Registration = null };

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _healthCheck.CheckHealthAsync(context, CancellationToken.None));
        }

        [Fact]
        public async Task CheckHealthAsync_AllocatedMemoryIsPositive()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 1024 };
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            var allocatedMBs = (long)result.Data["AllocatedMBs"];
            Assert.True(allocatedMBs >= 0);
        }

        [Fact]
        public async Task CheckHealthAsync_GCCollectionCountsAreNonNegative()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 1024 };
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            var gen0Collections = (int)result.Data["Gen0Collections"];
            var gen1Collections = (int)result.Data["Gen1Collections"];
            var gen2Collections = (int)result.Data["Gen2Collections"];

            Assert.True(gen0Collections >= 0);
            Assert.True(gen1Collections >= 0);
            Assert.True(gen2Collections >= 0);
        }

        [Fact]
        public async Task CheckHealthAsync_DescriptionContainsThreshold()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 2048 };
            _optionsMonitorMock.Setup(x => x.Get(It.IsAny<string>())).Returns(options);

            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("test", _healthCheck, HealthStatus.Unhealthy, new string[0])
            };

            // Act
            var result = await _healthCheck.CheckHealthAsync(context, CancellationToken.None);

            // Assert
            Assert.Contains("2048", result.Description);
        }
    }

    public class MemoryCheckOptionsTests
    {
        [Fact]
        public void MemoryCheckOptions_DefaultThreshold_IsOneGB()
        {
            // Act
            var options = new MemoryCheckOptions();

            // Assert
            Assert.Equal(1024L, options.Threshold);
        }

        [Fact]
        public void MemoryCheckOptions_CanSetCustomThreshold()
        {
            // Arrange
            var options = new MemoryCheckOptions { Threshold = 512L };

            // Assert
            Assert.Equal(512L, options.Threshold);
        }
    }
} 