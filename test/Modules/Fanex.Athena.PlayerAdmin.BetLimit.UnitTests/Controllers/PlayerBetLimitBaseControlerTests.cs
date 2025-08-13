namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    using AutoFixture;
    using BetLimit.Controllers;
    using BetLimit.DTO;
    using Core.Constants;
    using Core.Services;
    using Moq;

    public class PlayerBetLimitBaseControlerTests
    {
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly TestablePlayerBetLimitBaseController _controller;
        private readonly IFixture _fixture;
        private readonly Mock<IResourceService> _resourceServiceMock;

        public PlayerBetLimitBaseControlerTests()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _resourceServiceMock = new Mock<IResourceService>();

            _controller = new TestablePlayerBetLimitBaseController(_contextServiceMock.Object, _resourceServiceMock.Object);
        }

        [Fact]
        public void AddSettingTypeName_ShouldSetBetSettingTypeForNormalBets_WhenBetTypeIsNotParlay()
        {
            // Arrange
            var playerBetLimits = new List<PlayerBetLimit>
            {
                new() { SportType = "1", BetType = BetType.DefaultBetType },
                new() { SportType = "2", BetType = BetType.DefaultBetType }
            };

            _contextServiceMock.Setup(x => x.SpecialPlatform).Returns(0);
            _resourceServiceMock.Setup(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>())).Returns("Sport Name");

            // Act
            var result = _controller.TestAddSettingTypeName(playerBetLimits);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => !string.IsNullOrEmpty(x.BetSettingType)));
        }

        [Fact]
        public void AddSettingTypeName_ShouldSetBetSettingTypeForParlayBets_WhenBetTypeIsParlay()
        {
            // Arrange
            var playerBetLimits = new List<PlayerBetLimit>
            {
                new() { SportType = "1", BetType = BetType.Parlay },
                new() { SportType = "99", BetType = BetType.Parlay }
            };

            _resourceServiceMock.Setup(x => x.GetSportName(It.IsAny<string>(), It.IsAny<string>())).Returns("Sport Name");

            // Act
            var result = _controller.TestAddSettingTypeName(playerBetLimits);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.True(result.All(x => !string.IsNullOrEmpty(x.BetSettingType)));
        }

        [Fact]
        public void GetParlaySettingTypeName_ShouldReturnSoccerParlay_WhenSportTypeIsSoccer()
        {
            // Arrange
            const string soccerId = "1";

            // Act
            var result = _controller.TestGetParlaySettingTypeName(soccerId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetParlaySettingTypeName_ShouldReturnMixSportsParlay_WhenSportTypeIsOtherSports()
        {
            // Arrange
            const string otherSportsId = "99";

            // Act
            var result = _controller.TestGetParlaySettingTypeName(otherSportsId);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetParlaySettingTypeName_ShouldReturnSportName_WhenSportTypeIsNotSoccerOrOtherSports()
        {
            // Arrange
            const string customSportId = "5";
            const string expectedSportName = "Custom Sport";

            _resourceServiceMock.Setup(x => x.GetSportName(customSportId, It.IsAny<string>())).Returns(expectedSportName);

            // Act
            var result = _controller.TestGetParlaySettingTypeName(customSportId);

            // Assert
            Assert.Equal(expectedSportName, result);
        }

        [Fact]
        public void HasEditPermission_ShouldReturnTrue_WhenUserIsInRequiredRoles()
        {
            // Arrange
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BetLimit_Write)).Returns(true);

            // Act
            var result = _controller.TestHasEditPermission();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasEditPermission_ShouldReturnFalse_WhenUserIsNotInRequiredRoles()
        {
            // Arrange
            _contextServiceMock.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BetLimit_Write)).Returns(false);

            // Act
            var result = _controller.TestHasEditPermission();

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("43", false)]
        [InlineData("44", false)]
        [InlineData("45", false)]
        public void GetSportName_ShouldReturnSportNameWithPrefix_WhenSabaSportTypes(string sportId, bool isHideSaba)
        {
            // Arrange
            const string expectedSportName = "Soccer";
            _resourceServiceMock.Setup(x => x.GetSportName(It.IsAny<object>(), It.IsAny<string>())).Returns(expectedSportName);

            // Act
            var result = _controller.TestGetSportName(sportId, isHideSaba);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains(expectedSportName, result);
        }

        [Fact]
        public void GetSportName_ShouldReturnVirtualSportsName_WhenVirtualSportsType()
        {
            // Arrange
            const string virtualSportsId = "9908";
            const string expectedName = "Virtual Sports";

            _resourceServiceMock.Setup(x => x.GettResourceData("DummySport", virtualSportsId)).Returns(expectedName);

            // Act
            var result = _controller.TestGetSportName(virtualSportsId, false);

            // Assert
            Assert.Equal(expectedName, result);
        }

        [Fact]
        public void GetBetTypeName_ShouldReturnBetTypeName_WhenValidBetTypeId()
        {
            // Arrange
            const int betTypeId = 1;
            const string expectedBetTypeName = "Single Bet";

            _resourceServiceMock.Setup(x => x.GetBettype(betTypeId)).Returns(expectedBetTypeName);

            // Act
            var result = _controller.TestGetBetTypeName(betTypeId);

            // Assert
            Assert.Equal(expectedBetTypeName, result);
        }
    }

    // Test wrapper class to expose protected methods
    public class TestablePlayerBetLimitBaseController : PlayerBetLimitBaseControler
    {
        public TestablePlayerBetLimitBaseController(IContextService contextService, IResourceService resourceService)
            : base(contextService, resourceService)
        {
        }

        public IEnumerable<PlayerBetLimit> TestAddSettingTypeName(IEnumerable<PlayerBetLimit> data)
            => AddSettingTypeName(data);

        public string TestGetParlaySettingTypeName(string id)
            => GetParlaySettingTypeName(id);

        public bool TestHasEditPermission()
            => HasEditPermission();

        public string TestGetSportName(string sportId, bool isHideSaba)
            => GetSportName(sportId, isHideSaba);

        public string TestGetBetTypeName(int bettypeId)
            => GetBetTypeName(bettypeId);
    }
}