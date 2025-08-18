namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Extensions
{
    using Commission.Extensions;
    using Commission.Repositories;
    using Commission.Repositories.Interfaces;
    using Commission.Services;
    using Commission.Services.Interfaces;
    using Core.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public class DependencyInjectionExtensionTests
    {
        [Fact]
        public void AddCommission_ShouldRegisterAllServices_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IDbFactory, DbFactory>();

            // Act
            var result = services.AddCommission();

            // Assert
            Assert.Same(services, result);

            var serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(serviceProvider.GetService<IPlayerCommissionService>());
            Assert.NotNull(serviceProvider.GetService<IPlayerCommissionRepository>());
        }

        [Fact]
        public void AddCommission_ShouldRegisterPlayerCommissionServiceAsTransient_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IDbFactory, DbFactory>();

            // Act
            services.AddCommission();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerCommissionService));
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
            Assert.Equal(typeof(PlayerCommissionService), serviceDescriptor.ImplementationType);
        }

        [Fact]
        public void AddCommission_ShouldRegisterPlayerCommissionRepositoryAsTransient_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IDbFactory, DbFactory>();

            // Act
            services.AddCommission();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerCommissionRepository));
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
            Assert.Equal(typeof(PlayerCommissionRepository), serviceDescriptor.ImplementationType);
        }

        [Fact]
        public void AddCommission_ShouldReturnSameServiceCollection_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IDbFactory, DbFactory>();

            // Act
            var result = services.AddCommission();

            // Assert
            Assert.Same(services, result);
        }

        [Fact]
        public void AddCommission_ShouldRegisterExactlyTwoServices_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddCommission();

            // Assert
            Assert.Equal(2, services.Count);
        }

        [Fact]
        public void AddCommission_ShouldAllowMultipleCalls_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IDbFactory, DbFactory>();

            // Act
            services.AddCommission();
            services.AddCommission();

            // Assert
            Assert.Equal(5, services.Count); // DbFactory + 2 services added twice

            var serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(serviceProvider.GetService<IPlayerCommissionService>());
            Assert.NotNull(serviceProvider.GetService<IPlayerCommissionRepository>());
        }

        [Fact]
        public void AddCommission_ShouldWorkWithExistingServices_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSingleton<string>("test");
            services.AddTransient<IDbFactory, DbFactory>();

            // Act
            services.AddCommission();

            // Assert
            Assert.Equal(4, services.Count); // string + DbFactory + 2 commission services

            var serviceProvider = services.BuildServiceProvider();
            Assert.Equal("test", serviceProvider.GetService<string>());
            Assert.NotNull(serviceProvider.GetService<IPlayerCommissionService>());
            Assert.NotNull(serviceProvider.GetService<IPlayerCommissionRepository>());
        }

        [Fact]
        public void AddCommission_ShouldRegisterCorrectImplementations_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IDbFactory, DbFactory>();

            // Act
            services.AddCommission();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            var playerCommissionService = serviceProvider.GetService<IPlayerCommissionService>();
            Assert.IsType<PlayerCommissionService>(playerCommissionService);

            var playerCommissionRepository = serviceProvider.GetService<IPlayerCommissionRepository>();
            Assert.IsType<PlayerCommissionRepository>(playerCommissionRepository);
        }

        [Fact]
        public void AddCommission_ShouldCreateNewInstancesEachTime_WhenTransientServicesRequested()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddTransient<IDbFactory, DbFactory>();
            services.AddCommission();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var service1 = serviceProvider.GetService<IPlayerCommissionService>();
            var service2 = serviceProvider.GetService<IPlayerCommissionService>();
            var repository1 = serviceProvider.GetService<IPlayerCommissionRepository>();
            var repository2 = serviceProvider.GetService<IPlayerCommissionRepository>();

            // Assert
            Assert.NotSame(service2, service1);
            Assert.NotSame(repository2, repository1);
        }
    }
}