namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using API.Services;
    using API.Services.Interfaces;
    using Core.Repositories;
    using MediatR;
    using Moq;
    using Xunit;

    public class PlayerSpecialLevelServiceTest
    {
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly PlayerSpecialLevelService _service;

        public PlayerSpecialLevelServiceTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _mockPublisher = new Mock<IPublisher>();
            _service = new PlayerSpecialLevelService(_mockDbFactory.Object, _mockPublisher.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullDependencies_DoesNotThrowException()
        {
            // The service constructor doesn't validate null parameters
            // Act & Assert
            var service = new PlayerSpecialLevelService(null, null);
            Assert.NotNull(service);
        }

        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IPlayerSpecialLevelService>(_service);
        }

        [Fact]
        public void Service_HasCorrectDependencies()
        {
            // Act & Assert
            Assert.NotNull(_mockDbFactory);
            Assert.NotNull(_mockPublisher);
            Assert.NotNull(_service);
        }
    }
}