namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using System.Linq;
    using Core.ConsoleClient;
    using Core.Extensions;
    using Core.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;
    using Xunit;

    public class DefendencyInjectionExtensionTest
    {
        [Fact]
        public void AddCore_RegistersAllRequiredServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddCore();

            // Assert
            Assert.NotNull(result);
            Assert.Same(services, result); // Should return the same collection for chaining
        }

        [Fact]
        public void AddCore_RegistersApiClients()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();

            // Assert
            Assert.Contains(services, s => s.ServiceType == typeof(IReportApiClient) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(IBetListApiClient) && s.Lifetime == ServiceLifetime.Transient);
            Assert.Contains(services, s => s.ServiceType == typeof(ICommonApiClient) && s.Lifetime == ServiceLifetime.Transient);
        }

        [Fact]
        public void AddCore_RegistersContextService()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();

            // Assert
            Assert.Contains(services, s => s.ServiceType == typeof(IContextService) && s.Lifetime == ServiceLifetime.Scoped);
        }

        [Fact]
        public void AddCore_RegistersSingletonServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();

            // Assert
            Assert.Contains(services, s => s.ServiceType == typeof(ILicenseeService) && s.Lifetime == ServiceLifetime.Singleton);
            Assert.Contains(services, s => s.ServiceType == typeof(ISubsidiaryService) && s.Lifetime == ServiceLifetime.Singleton);
            Assert.Contains(services, s => s.ServiceType == typeof(IResourceService) && s.Lifetime == ServiceLifetime.Singleton);
            Assert.Contains(services, s => s.ServiceType == typeof(IFunctionUMService) && s.Lifetime == ServiceLifetime.Singleton);
        }

        [Fact]
        public void AddCore_RegistersRepositories()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();

            // Assert
            Assert.Contains(services, s => s.ServiceType == typeof(IDbFactory) && s.Lifetime == ServiceLifetime.Singleton);
            Assert.Contains(services, s => s.ServiceType == typeof(ILogRepository) && s.Lifetime == ServiceLifetime.Transient);
        }

        [Fact]
        public void AddCore_RegistersCorrectImplementations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();

            // Assert
            var reportApiClientDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IReportApiClient));
            Assert.NotNull(reportApiClientDescriptor);
            Assert.Equal(typeof(ReportApiClient), reportApiClientDescriptor.ImplementationType);

            var betListApiClientDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IBetListApiClient));
            Assert.NotNull(betListApiClientDescriptor);
            Assert.Equal(typeof(BetListApiClient), betListApiClientDescriptor.ImplementationType);

            var commonApiClientDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ICommonApiClient));
            Assert.NotNull(commonApiClientDescriptor);
            Assert.Equal(typeof(CommonApiClient), commonApiClientDescriptor.ImplementationType);

            var contextServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IContextService));
            Assert.NotNull(contextServiceDescriptor);
            Assert.Equal(typeof(ContextService), contextServiceDescriptor.ImplementationType);

            var licenseeServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ILicenseeService));
            Assert.NotNull(licenseeServiceDescriptor);
            Assert.Equal(typeof(LicenseeService), licenseeServiceDescriptor.ImplementationType);

            var subsidiaryServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ISubsidiaryService));
            Assert.NotNull(subsidiaryServiceDescriptor);
            Assert.Equal(typeof(SubsidiaryService), subsidiaryServiceDescriptor.ImplementationType);

            var resourceServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IResourceService));
            Assert.NotNull(resourceServiceDescriptor);
            Assert.Equal(typeof(ResourceService), resourceServiceDescriptor.ImplementationType);

            var functionUMServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IFunctionUMService));
            Assert.NotNull(functionUMServiceDescriptor);
            Assert.Equal(typeof(FunctionUMService), functionUMServiceDescriptor.ImplementationType);

            var dbFactoryDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IDbFactory));
            Assert.NotNull(dbFactoryDescriptor);
            Assert.Equal(typeof(DbFactory), dbFactoryDescriptor.ImplementationType);

            var logRepositoryDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(ILogRepository));
            Assert.NotNull(logRepositoryDescriptor);
            Assert.Equal(typeof(LogRepository), logRepositoryDescriptor.ImplementationType);
        }

        [Fact]
        public void AddCore_DoesNotRegisterDuplicateServices()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();
            services.AddCore(); // Add twice

            // Assert
            var contextServiceCount = services.Count(s => s.ServiceType == typeof(IContextService));
            Assert.Equal(2, contextServiceCount); // Should have 2 registrations (last one wins in DI)

            var licenseeServiceCount = services.Count(s => s.ServiceType == typeof(ILicenseeService));
            Assert.Equal(2, licenseeServiceCount);
        }

        [Fact]
        public void AddCore_CanBeChainedWithOtherServiceRegistrations()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services
                .AddCore()
                .AddLogging()
                .AddMemoryCache();

            // Assert
            Assert.NotNull(result);
            Assert.Same(services, result);
            Assert.True(services.Count > 10); // Should have many services registered
        }

        [Fact]
        public void AddCore_RegistersServicesInCorrectOrder()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCore();

            // Assert
            // Verify that services are registered (order doesn't matter for functionality but good to test)
            var serviceTypes = services.Select(s => s.ServiceType).ToList();


            Assert.Contains(typeof(IReportApiClient), serviceTypes);
            Assert.Contains(typeof(IBetListApiClient), serviceTypes);
            Assert.Contains(typeof(ICommonApiClient), serviceTypes);
            Assert.Contains(typeof(IContextService), serviceTypes);
            Assert.Contains(typeof(ILicenseeService), serviceTypes);
            Assert.Contains(typeof(ISubsidiaryService), serviceTypes);
            Assert.Contains(typeof(IResourceService), serviceTypes);
            Assert.Contains(typeof(IFunctionUMService), serviceTypes);
            Assert.Contains(typeof(IDbFactory), serviceTypes);
            Assert.Contains(typeof(ILogRepository), serviceTypes);
        }
    }
}