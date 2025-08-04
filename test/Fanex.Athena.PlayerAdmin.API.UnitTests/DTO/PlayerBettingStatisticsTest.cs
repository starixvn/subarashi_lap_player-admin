namespace Fanex.Athena.PlayerAdmin.API.UnitTests.DTO
{
    using API.DTO;
    using Xunit;

    public class PlayerBettingStatisticsTest
    {
        [Fact]
        public void PlayerBettingStatistics_DefaultConstructor_CreatesInstance()
        {
            // Act
            var statistics = new PlayerBettingStatistics();

            // Assert
            Assert.NotNull(statistics);
            Assert.Equal(0, statistics.WonTicketCount);
            Assert.Equal(0, statistics.LoseTicketCount);
            Assert.Equal(0, statistics.DrawTicketCount);
            Assert.Equal(0, statistics.CancelledTicketCount);
            Assert.Equal(0m, statistics.Margin);
            Assert.Equal(0m, statistics.AverageBetSize);
            Assert.Equal(0m, statistics.Winnings);
            Assert.Equal(0m, statistics.Loss);
        }

        [Fact]
        public void PlayerBettingStatistics_SetProperties_StoresValuesCorrectly()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();

            // Act
            statistics.WonTicketCount = 10;
            statistics.LoseTicketCount = 5;
            statistics.DrawTicketCount = 2;
            statistics.CancelledTicketCount = 1;
            statistics.Margin = 15.5m;
            statistics.AverageBetSize = 25.75m;
            statistics.Winnings = 1000.00m;
            statistics.Loss = -500.00m;

            // Assert
            Assert.Equal(10, statistics.WonTicketCount);
            Assert.Equal(5, statistics.LoseTicketCount);
            Assert.Equal(2, statistics.DrawTicketCount);
            Assert.Equal(1, statistics.CancelledTicketCount);
            Assert.Equal(15.5m, statistics.Margin);
            Assert.Equal(25.75m, statistics.AverageBetSize);
            Assert.Equal(1000.00m, statistics.Winnings);
            Assert.Equal(-500.00m, statistics.Loss);
        }

        [Fact]
        public void NetWinLoss_WithPositiveWinningsAndNegativeLoss_ReturnsCorrectSum()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.Winnings = 1000.00m;
            statistics.Loss = -300.00m;

            // Act
            var netWinLoss = statistics.NetWinLoss;

            // Assert
            Assert.Equal(700.00m, netWinLoss);
        }

        [Fact]
        public void NetWinLoss_WithZeroValues_ReturnsZero()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.Winnings = 0m;
            statistics.Loss = 0m;

            // Act
            var netWinLoss = statistics.NetWinLoss;

            // Assert
            Assert.Equal(0m, netWinLoss);
        }

        [Fact]
        public void NetWinLoss_WithNegativeWinningsAndPositiveLoss_ReturnsCorrectSum()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.Winnings = -200.00m;
            statistics.Loss = 100.00m;

            // Act
            var netWinLoss = statistics.NetWinLoss;

            // Assert
            Assert.Equal(-100.00m, netWinLoss);
        }

        [Fact]
        public void HasData_WithNoTickets_ReturnsFalse()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.WonTicketCount = 0;
            statistics.LoseTicketCount = 0;
            statistics.DrawTicketCount = 0;
            statistics.CancelledTicketCount = 0;

            // Act
            var hasData = statistics.HasData;

            // Assert
            Assert.False(hasData);
        }

        [Fact]
        public void HasData_WithWonTickets_ReturnsTrue()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.WonTicketCount = 1;
            statistics.LoseTicketCount = 0;
            statistics.DrawTicketCount = 0;
            statistics.CancelledTicketCount = 0;

            // Act
            var hasData = statistics.HasData;

            // Assert
            Assert.True(hasData);
        }

        [Fact]
        public void HasData_WithLoseTickets_ReturnsTrue()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.WonTicketCount = 0;
            statistics.LoseTicketCount = 1;
            statistics.DrawTicketCount = 0;
            statistics.CancelledTicketCount = 0;

            // Act
            var hasData = statistics.HasData;

            // Assert
            Assert.True(hasData);
        }

        [Fact]
        public void HasData_WithDrawTickets_ReturnsTrue()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.WonTicketCount = 0;
            statistics.LoseTicketCount = 0;
            statistics.DrawTicketCount = 1;
            statistics.CancelledTicketCount = 0;

            // Act
            var hasData = statistics.HasData;

            // Assert
            Assert.True(hasData);
        }

        [Fact]
        public void HasData_WithCancelledTickets_ReturnsTrue()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.WonTicketCount = 0;
            statistics.LoseTicketCount = 0;
            statistics.DrawTicketCount = 0;
            statistics.CancelledTicketCount = 1;

            // Act
            var hasData = statistics.HasData;

            // Assert
            Assert.True(hasData);
        }

        [Fact]
        public void HasData_WithMultipleTicketTypes_ReturnsTrue()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.WonTicketCount = 5;
            statistics.LoseTicketCount = 3;
            statistics.DrawTicketCount = 1;
            statistics.CancelledTicketCount = 2;

            // Act
            var hasData = statistics.HasData;

            // Assert
            Assert.True(hasData);
        }

        [Fact]
        public void PlayerBettingStatistics_WithNegativeTicketCounts_StoresCorrectly()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();

            // Act
            statistics.WonTicketCount = -1;
            statistics.LoseTicketCount = -2;
            statistics.DrawTicketCount = -3;
            statistics.CancelledTicketCount = -4;

            // Assert
            Assert.Equal(-1, statistics.WonTicketCount);
            Assert.Equal(-2, statistics.LoseTicketCount);
            Assert.Equal(-3, statistics.DrawTicketCount);
            Assert.Equal(-4, statistics.CancelledTicketCount);
        }

        [Fact]
        public void PlayerBettingStatistics_WithLargeDecimalValues_StoresCorrectly()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();

            // Act
            statistics.Margin = 999999999.99m;
            statistics.AverageBetSize = 888888888.88m;
            statistics.Winnings = 777777777.77m;
            statistics.Loss = -666666666.66m;

            // Assert
            Assert.Equal(999999999.99m, statistics.Margin);
            Assert.Equal(888888888.88m, statistics.AverageBetSize);
            Assert.Equal(777777777.77m, statistics.Winnings);
            Assert.Equal(-666666666.66m, statistics.Loss);
        }

        [Fact]
        public void PlayerBettingStatistics_WithMaxIntValues_StoresCorrectly()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();

            // Act
            statistics.WonTicketCount = int.MaxValue;
            statistics.LoseTicketCount = int.MaxValue;
            statistics.DrawTicketCount = int.MaxValue;
            statistics.CancelledTicketCount = int.MaxValue;

            // Assert
            Assert.Equal(int.MaxValue, statistics.WonTicketCount);
            Assert.Equal(int.MaxValue, statistics.LoseTicketCount);
            Assert.Equal(int.MaxValue, statistics.DrawTicketCount);
            Assert.Equal(int.MaxValue, statistics.CancelledTicketCount);
        }

        [Fact]
        public void NetWinLoss_WithLargeValues_CalculatesCorrectly()
        {
            // Arrange
            var statistics = new PlayerBettingStatistics();
            statistics.Winnings = 1000000.50m;
            statistics.Loss = -250000.25m;

            // Act
            var netWinLoss = statistics.NetWinLoss;

            // Assert
            Assert.Equal(750000.25m, netWinLoss);
        }
    }
}