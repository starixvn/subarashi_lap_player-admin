namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Helpers
{
    using BetLimit.Helpers;

    public class ExchangeProductHelperTests
    {
        [Theory]
        [InlineData("12Bet", 0)]
        [InlineData("12bet", 0)]
        [InlineData("12BET", 0)]
        public void GetGroupMode_ShouldReturn12BMode_When12BetSite(string siteName, int expectedGroupMode)
        {
            // Act
            var result = ExchangeProductHelper.GetGroupMode(siteName);

            // Assert
            Assert.Equal(expectedGroupMode, result);
        }

        [Theory]
        [InlineData("OtherSite", 1)]
        [InlineData("TestSite", 1)]
        [InlineData("", 1)]
        [InlineData(null, 1)]
        [InlineData("12BetOther", 1)]
        [InlineData("Other12Bet", 1)]
        public void GetGroupMode_ShouldReturnOtherMode_WhenNot12BetSite(string siteName, int expectedGroupMode)
        {
            // Act
            var result = ExchangeProductHelper.GetGroupMode(siteName);

            // Assert
            Assert.Equal(expectedGroupMode, result);
        }

        [Fact]
        public void GetGroupMode_ShouldBeCaseInsensitive_When12BetSite()
        {
            // Arrange
            var variations = new[] { "12Bet", "12bet", "12BET", "12BeT" };

            foreach (var variation in variations)
            {
                // Act
                var result = ExchangeProductHelper.GetGroupMode(variation);

                // Assert
                Assert.Equal(0, result);
            }
        }

        [Fact]
        public void GetGroupMode_ShouldReturnConsistentResults_WhenCalledMultipleTimes()
        {
            // Arrange
            const string siteName = "12Bet";

            // Act
            var result1 = ExchangeProductHelper.GetGroupMode(siteName);
            var result2 = ExchangeProductHelper.GetGroupMode(siteName);
            var result3 = ExchangeProductHelper.GetGroupMode(siteName);

            // Assert
            Assert.Equal(result2, result1);
            Assert.Equal(result3, result2);
            Assert.Equal(0, result1);
        }

        [Theory]
        [InlineData("12Bet")]
        [InlineData("OtherSite")]
        [InlineData("")]
        [InlineData("TestSite123")]
        public void GetGroupMode_ShouldReturnValidGroupMode_WhenAnySiteName(string siteName)
        {
            // Act
            var result = ExchangeProductHelper.GetGroupMode(siteName);

            // Assert
            Assert.Contains(result, new[] { 0, 1 });
        }
    }
}