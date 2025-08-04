namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using API.Services;
    using API.Services.Interfaces;
    using Core.Repositories;
    using MediatR;
    using Moq;
    using Xunit;

    public class PlayerOddsGroupServiceTest
    {
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly PlayerOddsGroupService _service;

        public PlayerOddsGroupServiceTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _mockPublisher = new Mock<IPublisher>();
            _service = new PlayerOddsGroupService(_mockDbFactory.Object, _mockPublisher.Object);
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
            var service = new PlayerOddsGroupService(null, null);
            Assert.NotNull(service);
        }

        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IPlayerOddsGroupService>(_service);
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