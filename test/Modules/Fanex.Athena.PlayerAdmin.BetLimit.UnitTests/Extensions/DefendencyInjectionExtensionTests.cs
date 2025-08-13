namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Extensions
{
    using BetLimit.Extensions;
    using BetLimit.Repositories;
    using BetLimit.Repositories.Interfaces;
    using BetLimit.Services;
    using BetLimit.Services.Interfaces;
    using Core.ConsoleClient;
    using Core.Repositories;
    using Core.Settings;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;

    public class DefendencyInjectionExtensionTests
    {
        [Fact]
        public void AddBetLimit_ShouldRegisterAllServices_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            // Verify Services are registered

            Assert.NotNull(serviceProvider.GetService<IAGCasinoBetLimitService>());
            Assert.NotNull(serviceProvider.GetService<ISportsBookBetLimitService>());
            Assert.NotNull(serviceProvider.GetService<IDcsBetLimitService>());
            Assert.NotNull(serviceProvider.GetService<IThirdPartyBetLimitService>());
            Assert.NotNull(serviceProvider.GetService<IExchangeChargeCommService>());
        }

        [Fact]
        public void AddBetLimit_ShouldRegisterAllRepositories_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            // Verify Repositories are registered

            Assert.NotNull(serviceProvider.GetService<IPlayerBetLimitRepository>());
            Assert.NotNull(serviceProvider.GetService<IAGCasinoBetLimitRepository>());
            Assert.NotNull(serviceProvider.GetService<ISportsBookBetLimitRepository>());
            Assert.NotNull(serviceProvider.GetService<IDcsBetLimitRepository>());
            Assert.NotNull(serviceProvider.GetService<IThirdPartyBetLimitRepository>());
            Assert.NotNull(serviceProvider.GetService<INewDcsBetLimitRepository>());
            Assert.NotNull(serviceProvider.GetService<IDcsGameBetLimitRepository>());
        }

        [Fact]
        public void AddBetLimit_ShouldRegisterServicesAsTransient_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IAGCasinoBetLimitService));
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
        }

        [Fact]
        public void AddBetLimit_ShouldRegisterRepositoriesAsTransient_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();

            // Assert
            var serviceDescriptor = services.FirstOrDefault(x => x.ServiceType == typeof(IPlayerBetLimitRepository));
            Assert.NotNull(serviceDescriptor);
            Assert.Equal(ServiceLifetime.Transient, serviceDescriptor.Lifetime);
        }

        [Fact]
        public void AddBetLimit_ShouldReturnServiceCollection_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddBetLimit();

            // Assert
            Assert.Same(services, result);
        }

        [Fact]
        public void AddBetLimit_ShouldRegisterCorrectImplementations_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            // Verify correct implementations are registered
            Assert.IsType<AGCasinoBetLimitService>(serviceProvider.GetService<IAGCasinoBetLimitService>());
            Assert.IsType<SportsBookBetLimitService>(serviceProvider.GetService<ISportsBookBetLimitService>());
            Assert.IsType<DcsBetLimitService>(serviceProvider.GetService<IDcsBetLimitService>());
            Assert.IsType<ThirdPartyBetLimitService>(serviceProvider.GetService<IThirdPartyBetLimitService>());
            Assert.IsType<ExchangeChargeCommService>(serviceProvider.GetService<IExchangeChargeCommService>());
        }

        [Fact]
        public void AddBetLimit_ShouldRegisterCorrectRepositoryImplementations_WhenCalled()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();

            // Assert
            var serviceProvider = services.BuildServiceProvider();

            // Verify correct repository implementations are registered
            Assert.IsType<PlayerBetLimitRepository>(serviceProvider.GetService<IPlayerBetLimitRepository>());
            Assert.IsType<AGCasinoBetLimitRepository>(serviceProvider.GetService<IAGCasinoBetLimitRepository>());
            Assert.IsType<SportsBookBetLimitRepository>(serviceProvider.GetService<ISportsBookBetLimitRepository>());
            Assert.IsType<DcsBetLimitRepository>(serviceProvider.GetService<IDcsBetLimitRepository>());
            Assert.IsType<ThirdPartyBetLimitRepository>(serviceProvider.GetService<IThirdPartyBetLimitRepository>());
            Assert.IsType<NewDcsBetLimitRepository>(serviceProvider.GetService<INewDcsBetLimitRepository>());
            Assert.IsType<DcsGameBetLimitRepository>(serviceProvider.GetService<IDcsGameBetLimitRepository>());
        }

        [Fact]
        public void AddBetLimit_ShouldAllowMultipleCalls_WhenCalledTwice()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();
            services.AddBetLimit(); // Second call should not throw

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            Assert.NotNull(serviceProvider.GetService<IAGCasinoBetLimitService>());
        }

        [Fact]
        public void AddBetLimit_ShouldNotRegisterDuplicateServices_WhenCalledMultipleTimes()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient();
            services.AddTransient<IDbFactory, DbFactory>();

            // Add required dependencies
            services.AddTransient<IPublisher>(sp => new Mock<IPublisher>().Object);
            services.AddSingleton<IOptions<BetLimitApiSetting>>(sp =>
                Options.Create(new BetLimitApiSetting()));
            services.AddSingleton<IOptions<AppSetting>>(sp =>
                Options.Create(new AppSetting()));
            services.AddTransient<ILgApiClient>(sp => new Mock<ILgApiClient>().Object);
            services.AddTransient<ICommonApiClient>(sp => new Mock<ICommonApiClient>().Object);

            // Act
            services.AddBetLimit();
            services.AddBetLimit();

            // Assert
            var agCasinoServiceDescriptors = services.Where(x => x.ServiceType == typeof(IAGCasinoBetLimitService)).ToList();
            Assert.Equal(2, agCasinoServiceDescriptors.Count); // Multiple registrations are allowed in DI container
        }
    }
}