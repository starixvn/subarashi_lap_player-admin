namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.DTO
{
    using AutoFixture;
    using BetLimit.DTO;
    using Core.Constants;

    public class PlayerBetLimitTests
    {
        private readonly IFixture _fixture;

        public PlayerBetLimitTests()
        {
            _fixture = new Fixture();
        }

        [Theory]
        [InlineData("1", BetType.DefaultBetType, "1_1")]
        [InlineData("5", BetType.Parlay, "5_9")]
        [InlineData("99", BetType.DefaultBetType, "99_1")]
        public void Id_ShouldReturnCombinedSportTypeAndBetType_WhenCalled(string sportType, int betType, string expectedId)
        {
            // Arrange
            var playerBetLimit = new PlayerBetLimit
            {
                SportType = sportType,
                BetType = betType
            };

            // Act
            var result = playerBetLimit.Id;

            // Assert
            Assert.Equal(expectedId, result);
        }

        [Fact]
        public void Id_ShouldUpdateWhenSportTypeChanges_WhenSportTypeIsModified()
        {
            // Arrange
            var playerBetLimit = new PlayerBetLimit
            {
                SportType = "1",
                BetType = BetType.DefaultBetType
            };
            var initialId = playerBetLimit.Id;

            // Act
            playerBetLimit.SportType = "2";
            var newId = playerBetLimit.Id;

            // Assert
            Assert.Equal("1_1", initialId);
            Assert.Equal("2_1", newId);
        }

        [Fact]
        public void Id_ShouldUpdateWhenBetTypeChanges_WhenBetTypeIsModified()
        {
            // Arrange
            var playerBetLimit = new PlayerBetLimit
            {
                SportType = "1",
                BetType = BetType.DefaultBetType
            };
            var initialId = playerBetLimit.Id;

            // Act
            playerBetLimit.BetType = BetType.Parlay;
            var newId = playerBetLimit.Id;

            // Assert
            Assert.Equal("1_1", initialId);
            Assert.Equal("1_9", newId);
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenCreated()
        {
            // Arrange
            var playerBetLimit = new PlayerBetLimit();
            const string sportType = "5";
            const int betType = BetType.Parlay;
            const string betSettingType = "Test Setting";
            const decimal minBet = 10.5m;
            const decimal maxBet = 1000.0m;
            const decimal maxPerMatch = 500.0m;
            const decimal maxPerMatchMax = 600.0m;
            const decimal maxPerMatchMin = 400.0m;
            const decimal betValueMax = 1100.0m;
            const decimal betValueMin = 5.0m;
            const decimal maxPerBall = 250.0m;
            const decimal maxPerBallMax = 300.0m;
            const decimal maxPayoutPerMatch = 5000.0m;
            const decimal maxPayoutPerMatchMax = 6000.0m;
            const bool isEnable = true;

            // Act
            playerBetLimit.SportType = sportType;
            playerBetLimit.BetType = betType;
            playerBetLimit.BetSettingType = betSettingType;
            playerBetLimit.MinBet = minBet;
            playerBetLimit.MaxBet = maxBet;
            playerBetLimit.MaxPerMatch = maxPerMatch;
            playerBetLimit.MaxPerMatchMax = maxPerMatchMax;
            playerBetLimit.MaxPerMatchMin = maxPerMatchMin;
            playerBetLimit.BetValueMax = betValueMax;
            playerBetLimit.BetValueMin = betValueMin;
            playerBetLimit.MaxPerBall = maxPerBall;
            playerBetLimit.MaxPerBallMax = maxPerBallMax;
            playerBetLimit.MaxPayoutPerMatch = maxPayoutPerMatch;
            playerBetLimit.MaxPayoutPerMatchMax = maxPayoutPerMatchMax;
            playerBetLimit.IsEnable = isEnable;

            // Assert
            Assert.Equal(sportType, playerBetLimit.SportType);
            Assert.Equal(betType, playerBetLimit.BetType);
            Assert.Equal(betSettingType, playerBetLimit.BetSettingType);
            Assert.Equal(minBet, playerBetLimit.MinBet);
            Assert.Equal(maxBet, playerBetLimit.MaxBet);
            Assert.Equal(maxPerMatch, playerBetLimit.MaxPerMatch);
            Assert.Equal(maxPerMatchMax, playerBetLimit.MaxPerMatchMax);
            Assert.Equal(maxPerMatchMin, playerBetLimit.MaxPerMatchMin);
            Assert.Equal(betValueMax, playerBetLimit.BetValueMax);
            Assert.Equal(betValueMin, playerBetLimit.BetValueMin);
            Assert.Equal(maxPerBall, playerBetLimit.MaxPerBall);
            Assert.Equal(maxPerBallMax, playerBetLimit.MaxPerBallMax);
            Assert.Equal(maxPayoutPerMatch, playerBetLimit.MaxPayoutPerMatch);
            Assert.Equal(maxPayoutPerMatchMax, playerBetLimit.MaxPayoutPerMatchMax);
            Assert.Equal(isEnable, playerBetLimit.IsEnable);
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues_WhenCreated()
        {
            // Act
            var playerBetLimit = new PlayerBetLimit();

            // Assert
            Assert.Null(playerBetLimit.SportType);
            Assert.Equal(0, playerBetLimit.BetType);
            Assert.Null(playerBetLimit.BetSettingType);
            Assert.Equal(0, playerBetLimit.MinBet);
            Assert.Equal(0, playerBetLimit.MaxBet);
            Assert.Equal(0, playerBetLimit.MaxPerMatch);
            Assert.Equal(0, playerBetLimit.MaxPerMatchMax);
            Assert.Equal(0, playerBetLimit.MaxPerMatchMin);
            Assert.Equal(0, playerBetLimit.BetValueMax);
            Assert.Equal(0, playerBetLimit.BetValueMin);
            Assert.Equal(0, playerBetLimit.MaxPerBall);
            Assert.Equal(0, playerBetLimit.MaxPerBallMax);
            Assert.Equal(0, playerBetLimit.MaxPayoutPerMatch);
            Assert.Equal(0, playerBetLimit.MaxPayoutPerMatchMax);
            Assert.Null(playerBetLimit.IsEnable);
        }

        [Theory]
        [InlineData(null, BetType.DefaultBetType, "_1")]
        [InlineData("", BetType.DefaultBetType, "_1")]
        [InlineData("test", 0, "test_0")]
        public void Id_ShouldHandleNullOrEmptyValues_WhenPropertiesAreNotSet(string sportType, int betType, string expectedId)
        {
            // Arrange
            var playerBetLimit = new PlayerBetLimit
            {
                SportType = sportType,
                BetType = betType
            };

            // Act
            var result = playerBetLimit.Id;

            // Assert
            Assert.Equal(expectedId, result);
        }
    }
}