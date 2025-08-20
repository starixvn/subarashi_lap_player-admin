namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Extensions
{
    using BatchUpdate.Repositories;
    using BatchUpdate.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class DependencyInjectionExtensionTest
    {
        [Fact]
        public void AddBatchUpdate_RegistersAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddBatchUpdate();

            // Assert
            Assert.Same(services, result); // Verify method chaining

            // Verify service registrations
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerBatchUpdateService) &&
                                           s.ImplementationType == typeof(PlayerBatchUpdateService));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerBatchUpdateRepository) &&
                                           s.ImplementationType == typeof(PlayerBatchUpdateRepository));
        }

        [Fact]
        public void AddBatchUpdate_RegistersCorrectServiceLifetimes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddBatchUpdate();

            // Assert - All services should be registered as Transient
            var playerBatchUpdateServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerBatchUpdateService));
            Assert.NotNull(playerBatchUpdateServiceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, playerBatchUpdateServiceDescriptor.Lifetime);

            var playerBatchUpdateRepositoryDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerBatchUpdateRepository));
            Assert.NotNull(playerBatchUpdateRepositoryDescriptor);
            Assert.Equal(ServiceLifetime.Transient, playerBatchUpdateRepositoryDescriptor.Lifetime);
        }

        [Fact]
        public void AddBatchUpdate_RegistersCorrectImplementations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddBatchUpdate();

            // Assert - Verify correct interface to implementation mappings
            var playerBatchUpdateServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerBatchUpdateService));
            Assert.NotNull(playerBatchUpdateServiceDescriptor);
            Assert.Equal(typeof(PlayerBatchUpdateService), playerBatchUpdateServiceDescriptor.ImplementationType);

            var playerBatchUpdateRepositoryDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerBatchUpdateRepository));
            Assert.NotNull(playerBatchUpdateRepositoryDescriptor);
            Assert.Equal(typeof(PlayerBatchUpdateRepository), playerBatchUpdateRepositoryDescriptor.ImplementationType);
        }

        [Fact]
        public void AddBatchUpdate_EnablesMethodChaining()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddBatchUpdate();

            // Assert
            Assert.Same(services, result);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddBatchUpdate_CanBeCalledMultipleTimes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert - Should not throw
            services.AddBatchUpdate();
            services.AddBatchUpdate();

            // Should handle duplicate registrations gracefully
            Assert.True(services.Count >= 2); // At least 2 services registered (could be more with duplicates)

            // Verify services are still registered correctly after multiple calls
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerBatchUpdateService));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerBatchUpdateRepository));
        }

        [Fact]
        public void AddBatchUpdate_RegisteredServicesHaveCorrectServiceTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddBatchUpdate();

            // Act & Assert - Verify services are properly registered by checking service types
            var serviceTypes = services.Select(s => s.ServiceType).ToList();

            Assert.Contains(typeof(IPlayerBatchUpdateService), serviceTypes);
            Assert.Contains(typeof(IPlayerBatchUpdateRepository), serviceTypes);
        }

        [Fact]
        public void AddBatchUpdate_RegistersExactlyExpectedServiceCount()
        {
            // Arrange
            var services = new ServiceCollection();
            var initialCount = services.Count;

            // Act
            services.AddBatchUpdate();

            // Assert
            var expectedServiceCount = 2; // 2 services should be registered
            Assert.Equal(initialCount + expectedServiceCount, services.Count);
        }
    }
}