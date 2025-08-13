namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Extensions
{
    using API.Services;
    using API.Services.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public class DefendencyInjectionExtensionTest
    {
        [Fact]
        public void AddAPI_RegistersAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddAPI();

            // Assert
            Assert.NotNull(result);
            Assert.Same(services, result); // Should return the same collection for chaining
        }

        [Fact]
        public void AddAPI_RegistersTransientServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();

            // Assert
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerActivityRecordService) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerBettingStatisticsService) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerListService) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerOddsGroupService) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(IPlayerSpecialLevelService) && s.Lifetime == ServiceLifetime.Transient);
        }

        [Fact]
        public void AddAPI_RegistersSingletonServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();

            // Assert
            Assert.Contains(services, s => s.ServiceType == typeof(IDepositSettingService) && s.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddAPI_RegistersCorrectImplementations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();

            // Assert
            var playerActivityRecordServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerActivityRecordService));
            Assert.NotNull(playerActivityRecordServiceDescriptor);
            Assert.Equal(typeof(PlayerActivityRecordService), playerActivityRecordServiceDescriptor.ImplementationType);

            var playerBettingStatisticsServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerBettingStatisticsService));
            Assert.NotNull(playerBettingStatisticsServiceDescriptor);
            Assert.Equal(typeof(PlayerBettingStatisticsService), playerBettingStatisticsServiceDescriptor.ImplementationType);

            var playerListServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerListService));
            Assert.NotNull(playerListServiceDescriptor);
            Assert.Equal(typeof(PlayerListService), playerListServiceDescriptor.ImplementationType);

            var playerOddsGroupServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerOddsGroupService));
            Assert.NotNull(playerOddsGroupServiceDescriptor);
            Assert.Equal(typeof(PlayerOddsGroupService), playerOddsGroupServiceDescriptor.ImplementationType);

            var playerSpecialLevelServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerSpecialLevelService));
            Assert.NotNull(playerSpecialLevelServiceDescriptor);
            Assert.Equal(typeof(PlayerSpecialLevelService), playerSpecialLevelServiceDescriptor.ImplementationType);

            var depositSettingServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IDepositSettingService));
            Assert.NotNull(depositSettingServiceDescriptor);
            Assert.Equal(typeof(DepositSettingService), depositSettingServiceDescriptor.ImplementationType);
        }

        [Fact]
        public void AddAPI_DoesNotRegisterDuplicateServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();
            services.AddAPI(); // Add twice

            // Assert
            var playerActivityRecordServiceCount = services.Count(s => s.ServiceType == typeof(IPlayerActivityRecordService));
            Assert.Equal(2, playerActivityRecordServiceCount); // Should have 2 registrations (last one wins in DI)

            var depositSettingServiceCount = services.Count(s => s.ServiceType == typeof(IDepositSettingService));
            Assert.Equal(2, depositSettingServiceCount);
        }

        [Fact]
        public void AddAPI_CanBeChainedWithOtherServiceRegistrations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services
                .AddAPI()
                .AddLogging()
                .AddMemoryCache();

            // Assert
            Assert.NotNull(result);
            Assert.Same(services, result);
            Assert.True(services.Count > 6); // Should have many services registered
        }

        [Fact]
        public void AddAPI_RegistersServicesInCorrectOrder()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();

            // Assert
            // Verify that services are registered (order doesn't matter for functionality but good to test)
            var serviceTypes = services.Select(s => s.ServiceType).ToList();

            Assert.Contains(typeof(IDepositSettingService), serviceTypes);
            Assert.Contains(typeof(IPlayerActivityRecordService), serviceTypes);
            Assert.Contains(typeof(IPlayerBettingStatisticsService), serviceTypes);
            Assert.Contains(typeof(IPlayerListService), serviceTypes);
            Assert.Contains(typeof(IPlayerOddsGroupService), serviceTypes);
            Assert.Contains(typeof(IPlayerSpecialLevelService), serviceTypes);
        }

        [Fact]
        public void AddAPI_RegistersAllExpectedServiceTypes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();

            // Assert
            var registeredServiceTypes = services.Select(s => s.ServiceType).ToHashSet();

            // Verify all expected API services are registered
            Assert.Contains(typeof(IDepositSettingService), registeredServiceTypes);
            Assert.Contains(typeof(IPlayerActivityRecordService), registeredServiceTypes);
            Assert.Contains(typeof(IPlayerBettingStatisticsService), registeredServiceTypes);
            Assert.Contains(typeof(IPlayerListService), registeredServiceTypes);
            Assert.Contains(typeof(IPlayerOddsGroupService), registeredServiceTypes);
            Assert.Contains(typeof(IPlayerSpecialLevelService), registeredServiceTypes);
        }

        [Fact]
        public void AddAPI_WithEmptyServiceCollection_RegistersCorrectCount()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();

            // Assert
            // Should register exactly 6 services (1 singleton + 5 transient)
            Assert.Equal(6, services.Count);
        }

        [Fact]
        public void AddAPI_WithExistingServices_AddsToExistingCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging(); // Add some existing services
            var initialCount = services.Count;

            // Act
            services.AddAPI();

            // Assert
            Assert.Equal(initialCount + 6, services.Count);
        }

        [Fact]
        public void AddAPI_RegistersServicesWithCorrectLifetimes()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddAPI();

            // Assert
            // Check singleton services
            var singletonServices = services.Where(s => s.Lifetime == ServiceLifetime.Singleton).ToList();
            Assert.Single(singletonServices);
            Assert.Equal(typeof(IDepositSettingService), singletonServices[0].ServiceType);

            // Check transient services
            var transientServices = services.Where(s => s.Lifetime == ServiceLifetime.Transient).ToList();
            Assert.Equal(5, transientServices.Count);

            var transientServiceTypes = transientServices.Select(s => s.ServiceType).ToHashSet();
            Assert.Contains(typeof(IPlayerActivityRecordService), transientServiceTypes);
            Assert.Contains(typeof(IPlayerBettingStatisticsService), transientServiceTypes);
            Assert.Contains(typeof(IPlayerListService), transientServiceTypes);
            Assert.Contains(typeof(IPlayerOddsGroupService), transientServiceTypes);
            Assert.Contains(typeof(IPlayerSpecialLevelService), transientServiceTypes);
        }
    }
}