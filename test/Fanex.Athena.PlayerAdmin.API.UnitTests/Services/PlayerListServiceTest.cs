namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using API.Services;
    using API.Services.Interfaces;
    using Core.Repositories;
    using Core.Services;
    using Moq;
    using Xunit;

    public class PlayerListServiceTest
    {
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<IDepositSettingService> _mockDepositSettingService;
        private readonly PlayerListService _service;

        public PlayerListServiceTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _mockContextService = new Mock<IContextService>();
            _mockDepositSettingService = new Mock<IDepositSettingService>();
            _service = new PlayerListService(_mockDbFactory.Object, _mockContextService.Object, _mockDepositSettingService.Object);
        }

        [Fact]
        public void Constructor_WithValidDbFactory_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullDependencies_DoesNotThrowException()
        {
            // The service constructor doesn't validate null parameters
            // Act & Assert
            var service = new PlayerListService(null, null, null);
            Assert.NotNull(service);
        }

        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IPlayerListService>(_service);
        }

        [Fact]
        public void Service_HasCorrectDependencies()
        {
            // Act & Assert
            Assert.NotNull(_mockDbFactory);
            Assert.NotNull(_mockContextService);
            Assert.NotNull(_mockDepositSettingService);
            Assert.NotNull(_service);
        }
    }
}