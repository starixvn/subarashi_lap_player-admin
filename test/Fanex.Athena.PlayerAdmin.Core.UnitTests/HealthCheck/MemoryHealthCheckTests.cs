namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.HealthCheck
{
    using Core.HealthCheck;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Options;

    public class MemoryHealthCheckTests
    {
        [Fact]
        public async Task CheckHealthAsync_ReturnsHealthy_WhenAllocatedBelowThreshold()
        {
            var monitor = new TestOptionsMonitor(new MemoryCheckOptions { Threshold = long.MaxValue });
            var check = new MemoryHealthCheck(monitor);
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("Test", check, HealthStatus.Unhealthy, null)
            };

            var result = await check.CheckHealthAsync(context, CancellationToken.None);

            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.True(result.Data.ContainsKey("AllocatedMBs"));
        }

        [Fact]
        public async Task CheckHealthAsync_ReturnsFailureStatus_WhenAllocatedAboveThreshold()
        {
            var monitor = new TestOptionsMonitor(new MemoryCheckOptions { Threshold = 0 });
            var check = new MemoryHealthCheck(monitor);
            var context = new HealthCheckContext
            {
                Registration = new HealthCheckRegistration("Test", check, HealthStatus.Degraded, null)
            };

            var result = await check.CheckHealthAsync(context, CancellationToken.None);

            Assert.Equal(HealthStatus.Degraded, result.Status);
        }

        private sealed class TestOptionsMonitor : IOptionsMonitor<MemoryCheckOptions>
        {
            public TestOptionsMonitor(MemoryCheckOptions options)
            {
                CurrentValue = options;
            }

            public MemoryCheckOptions CurrentValue { get; }

            public MemoryCheckOptions Get(string name) => CurrentValue;
            public IDisposable OnChange(Action<MemoryCheckOptions, string> listener) => new Noop();

            private sealed class Noop : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }
    }
}