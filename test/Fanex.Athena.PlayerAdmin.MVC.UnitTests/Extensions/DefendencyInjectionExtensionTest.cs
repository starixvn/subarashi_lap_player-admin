namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using MVC.Excel;
    using MVC.Extensions;
    using MVC.Repositories;
    using MVC.Repositories.Interfaces;
    using MVC.Services;
    using MVC.Services.Interfaces;

    public class DefendencyInjectionExtensionTest
    {
        [Fact]
        public void AddMVC_RegistersAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddMVC();

            // Assert
            Assert.Same(services, result); // Verify method chaining

            // Verify service registrations
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerDetailTransactionService) &&
                                           s.ImplementationType == typeof(PlayerDetailTransactionService));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerDetailTransactionExcel) &&
                                           s.ImplementationType == typeof(PlayerDetailTransactionExcel));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerStatementService) &&
                                           s.ImplementationType == typeof(PlayerStatementService));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerDetailTransactionRepository) &&
                                           s.ImplementationType == typeof(PlayerDetailTransactionRepository));
        }

        [Fact]
        public void AddMVC_RegistersCorrectServiceLifetimes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMVC();

            // Assert - All services should be registered as Transient
            var playerDetailTransactionServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerDetailTransactionService));
            Assert.NotNull(playerDetailTransactionServiceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, playerDetailTransactionServiceDescriptor.Lifetime);

            var playerDetailTransactionExcelDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerDetailTransactionExcel));
            Assert.NotNull(playerDetailTransactionExcelDescriptor);
            Assert.Equal(ServiceLifetime.Transient, playerDetailTransactionExcelDescriptor.Lifetime);

            var playerStatementServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerStatementService));
            Assert.NotNull(playerStatementServiceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, playerStatementServiceDescriptor.Lifetime);

            var playerDetailTransactionRepositoryDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerDetailTransactionRepository));
            Assert.NotNull(playerDetailTransactionRepositoryDescriptor);
            Assert.Equal(ServiceLifetime.Transient, playerDetailTransactionRepositoryDescriptor.Lifetime);
        }

        [Fact]
        public void AddMVC_RegistersCorrectImplementations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddMVC();

            // Assert - Verify correct interface to implementation mappings
            var playerDetailTransactionServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerDetailTransactionService));
            Assert.NotNull(playerDetailTransactionServiceDescriptor);
            Assert.Equal(typeof(PlayerDetailTransactionService), playerDetailTransactionServiceDescriptor.ImplementationType);

            var playerDetailTransactionExcelDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerDetailTransactionExcel));
            Assert.NotNull(playerDetailTransactionExcelDescriptor);
            Assert.Equal(typeof(PlayerDetailTransactionExcel), playerDetailTransactionExcelDescriptor.ImplementationType);

            var playerStatementServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerStatementService));
            Assert.NotNull(playerStatementServiceDescriptor);
            Assert.Equal(typeof(PlayerStatementService), playerStatementServiceDescriptor.ImplementationType);

            var playerDetailTransactionRepositoryDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerDetailTransactionRepository));
            Assert.NotNull(playerDetailTransactionRepositoryDescriptor);
            Assert.Equal(typeof(PlayerDetailTransactionRepository), playerDetailTransactionRepositoryDescriptor.ImplementationType);
        }

        [Fact]
        public void AddMVC_EnablesMethodChaining()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddMVC();

            // Assert
            Assert.Same(services, result);
            Assert.NotNull(result);
        }

        [Fact]
        public void AddMVC_CanBeCalledMultipleTimes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert - Should not throw
            services.AddMVC();
            services.AddMVC();

            // Should handle duplicate registrations gracefully
            Assert.True(services.Count >= 4); // At least 4 services registered (could be more with duplicates)

            // Verify services are still registered correctly after multiple calls
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerDetailTransactionService));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerDetailTransactionExcel));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerStatementService));
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerDetailTransactionRepository));
        }

        [Fact]
        public void AddMVC_RegisteredServicesHaveCorrectServiceTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddMVC();

            // Act & Assert - Verify services are properly registered by checking service types
            var serviceTypes = services.Select(s => s.ServiceType).ToList();

            Assert.Contains(typeof(IPlayerDetailTransactionService), serviceTypes);
            Assert.Contains(typeof(IPlayerDetailTransactionExcel), serviceTypes);
            Assert.Contains(typeof(IPlayerStatementService), serviceTypes);
            Assert.Contains(typeof(IPlayerDetailTransactionRepository), serviceTypes);
        }

        [Fact]
        public void AddMVC_RegistersExactlyExpectedServiceCount()
        {
            // Arrange
            var services = new ServiceCollection();
            var initialCount = services.Count;

            // Act
            services.AddMVC();

            // Assert
            var expectedServiceCount = 4; // 4 services should be registered
            Assert.Equal(initialCount + expectedServiceCount, services.Count);
        }
    }
}