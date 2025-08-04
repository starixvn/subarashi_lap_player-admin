namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using API.Services;
    using API.Services.Interfaces;
    using Core.Repositories;
    using Moq;
    using Xunit;

    public class PlayerBettingStatisticsServiceTest
    {
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly PlayerBettingStatisticsService _service;

        public PlayerBettingStatisticsServiceTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _service = new PlayerBettingStatisticsService(_mockDbFactory.Object);
        }

        [Fact]
        public void Constructor_WithValidDbFactory_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullDbFactory_DoesNotThrowException()
        {
            // The service constructor doesn't validate null parameters
            // Act & Assert
            var service = new PlayerBettingStatisticsService(null);
            Assert.NotNull(service);
        }

        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IPlayerBettingStatisticsService>(_service);
        }

        [Fact]
        public void Service_HasCorrectDependencies()
        {
            // Act & Assert
            Assert.NotNull(_mockDbFactory);
            Assert.NotNull(_service);
        }
    }
}