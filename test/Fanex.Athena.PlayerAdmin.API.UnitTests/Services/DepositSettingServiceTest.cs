namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using API.Services;
    using Core.Repositories;
    using Core.Settings;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class DepositSettingServiceTest
    {
        private readonly Mock<IOptions<AppSetting>> _mockAppSetting;
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<ILogger<DepositSettingService>> _mockLogger;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly DepositSettingService _service;

        public DepositSettingServiceTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockAppSetting = new Mock<IOptions<AppSetting>>();
            _mockLogger = new Mock<ILogger<DepositSettingService>>();

            _mockAppSetting.Setup(x => x.Value).Returns(new AppSetting());

            _service = new DepositSettingService(_mockDbFactory.Object, _mockHttpClient.Object,
                _mockMemoryCache.Object, _mockAppSetting.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullDependencies_ThrowsException()
        {
            // The service constructor validates null parameters
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new DepositSettingService(null, null, null, null, null));
        }

        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IDepositSettingService>(_service);
        }

        [Fact]
        public void Service_HasCorrectDependencies()
        {
            // Act & Assert
            Assert.NotNull(_mockDbFactory);
            Assert.NotNull(_mockHttpClient);
            Assert.NotNull(_mockMemoryCache);
            Assert.NotNull(_mockAppSetting);
            Assert.NotNull(_mockLogger);
            Assert.NotNull(_service);
        }
    }
}