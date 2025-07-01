using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.RemotingRef;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class ResourceServiceTests
    {
        private readonly Mock<IRemotingRefService> _remotingRefServiceMock;
        private readonly ResourceService _resourceService;

        public ResourceServiceTests()
        {
            _remotingRefServiceMock = new Mock<IRemotingRefService>();
            _resourceService = new ResourceService(_remotingRefServiceMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void GetBettype_InvalidId_ReturnsEmptyString(object bettypeId)
        {
            // Act
            var result = _resourceService.GetBettype(bettypeId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetBettype_ValidId_ReturnsBetTypeName()
        {
            // Arrange
            var bettypeId = "1";
            var expectedBetTypeName = "Test Bet Type";
            _remotingRefServiceMock.Setup(x => x.GetBetTypeName(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedBetTypeName);

            // Act
            var result = _resourceService.GetBettype(bettypeId);

            // Assert
            Assert.Equal(expectedBetTypeName, result);
        }

        [Theory]
        [InlineData(null, "1")]
        [InlineData("1", null)]
        [InlineData("0", "1")]
        [InlineData("1", "0")]
        public void GettResourceData_InvalidId_ReturnsEmptyString(object refId, object resourceId)
        {
            // Act
            var result = _resourceService.GettResourceData(refId, resourceId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GettResourceData_ValidId_ReturnsResourceData()
        {
            // Arrange
            var refId = "1";
            var resourceId = "2";
            var expectedResourceData = "Test Resource Data";
            _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedResourceData);

            // Act
            var result = _resourceService.GettResourceData(refId, resourceId);

            // Assert
            Assert.Equal(expectedResourceData, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        public void GetSportName_InvalidId_ReturnsEmptyString(object sportId)
        {
            // Act
            var result = _resourceService.GetSportName(sportId);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetSportName_ValidId_ReturnsSportName()
        {
            // Arrange
            var sportId = "1";
            var expectedSportName = "Test Sport";
            _remotingRefServiceMock.Setup(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedSportName);

            // Act
            var result = _resourceService.GetSportName(sportId);

            // Assert
            Assert.Equal(expectedSportName, result);
        }
    }
} 