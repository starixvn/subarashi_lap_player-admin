namespace Fanex.Athena.PlayerAdmin.API.UnitTests.DTO
{
    using API.DTO;
    using Xunit;

    public class PlayerTest
    {
        [Fact]
        public void Player_DefaultConstructor_CreatesInstance()
        {
            // Act
            var player = new Player();

            // Assert
            Assert.NotNull(player);
            Assert.Equal(0, player.IbcCustId);
            Assert.Null(player.UserName);
            Assert.Null(player.SystemId);
            Assert.Equal(0, player.CurrencyId);
            Assert.Null(player.Currency);
            Assert.Equal(0m, player.CurrentBalance);
            Assert.Equal(0m, player.OutstandingBalance);
            Assert.Null(player.OddsGroup);
            Assert.Equal(0, player.SpecialLevel);
            Assert.Null(player.CommissionGroup);
            Assert.Null(player.CustomerClass);
            Assert.Null(player.RiskLevel);
            Assert.Null(player.RiskLevelKey);
            Assert.Null(player.Status);
        }

        [Fact]
        public void Player_SetProperties_StoresValuesCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.IbcCustId = 123;
            player.UserName = "testuser";
            player.SystemId = "SYS001";
            player.CurrencyId = 1;
            player.Currency = "USD";
            player.CurrentBalance = 1000.50m;
            player.OutstandingBalance = 250.25m;
            player.OddsGroup = "A";
            player.SpecialLevel = 5;
            player.CommissionGroup = "Standard";
            player.CustomerClass = "VIP";
            player.RiskLevel = "Low";
            player.RiskLevelKey = "LOW_RISK";
            player.Status = 1;

            // Assert
            Assert.Equal(123, player.IbcCustId);
            Assert.Equal("testuser", player.UserName);
            Assert.Equal("SYS001", player.SystemId);
            Assert.Equal(1, player.CurrencyId);
            Assert.Equal("USD", player.Currency);
            Assert.Equal(1000.50m, player.CurrentBalance);
            Assert.Equal(250.25m, player.OutstandingBalance);
            Assert.Equal("A", player.OddsGroup);
            Assert.Equal(5, player.SpecialLevel);
            Assert.Equal("Standard", player.CommissionGroup);
            Assert.Equal("VIP", player.CustomerClass);
            Assert.Equal("Low", player.RiskLevel);
            Assert.Equal("LOW_RISK", player.RiskLevelKey);
            Assert.Equal(1, player.Status);
        }

        [Fact]
        public void Player_WithNegativeValues_StoresCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.IbcCustId = -1;
            player.CurrencyId = -1;
            player.CurrentBalance = -500.75m;
            player.OutstandingBalance = -100.00m;
            player.SpecialLevel = -2;
            player.Status = -1;

            // Assert
            Assert.Equal(-1, player.IbcCustId);
            Assert.Equal(-1, player.CurrencyId);
            Assert.Equal(-500.75m, player.CurrentBalance);
            Assert.Equal(-100.00m, player.OutstandingBalance);
            Assert.Equal(-2, player.SpecialLevel);
            Assert.Equal(-1, player.Status);
        }

        [Fact]
        public void Player_WithNullableStatus_HandlesNullCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.Status = null;

            // Assert
            Assert.Null(player.Status);
        }

        [Fact]
        public void Player_WithEmptyStrings_StoresCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.UserName = "";
            player.SystemId = "";
            player.Currency = "";
            player.OddsGroup = "";
            player.CommissionGroup = "";
            player.CustomerClass = "";
            player.RiskLevel = "";
            player.RiskLevelKey = "";

            // Assert
            Assert.Equal("", player.UserName);
            Assert.Equal("", player.SystemId);
            Assert.Equal("", player.Currency);
            Assert.Equal("", player.OddsGroup);
            Assert.Equal("", player.CommissionGroup);
            Assert.Equal("", player.CustomerClass);
            Assert.Equal("", player.RiskLevel);
            Assert.Equal("", player.RiskLevelKey);
        }

        [Fact]
        public void Player_WithLargeDecimalValues_StoresCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.CurrentBalance = 999999999.99m;
            player.OutstandingBalance = 888888888.88m;

            // Assert
            Assert.Equal(999999999.99m, player.CurrentBalance);
            Assert.Equal(888888888.88m, player.OutstandingBalance);
        }

        [Fact]
        public void Player_WithZeroValues_StoresCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.IbcCustId = 0;
            player.CurrencyId = 0;
            player.CurrentBalance = 0.00m;
            player.OutstandingBalance = 0.00m;
            player.SpecialLevel = 0;
            player.Status = 0;

            // Assert
            Assert.Equal(0, player.IbcCustId);
            Assert.Equal(0, player.CurrencyId);
            Assert.Equal(0.00m, player.CurrentBalance);
            Assert.Equal(0.00m, player.OutstandingBalance);
            Assert.Equal(0, player.SpecialLevel);
            Assert.Equal(0, player.Status);
        }

        [Fact]
        public void Player_WithSpecialCharacters_StoresCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.UserName = "test@user#123";
            player.SystemId = "SYS-001_v2";
            player.Currency = "€UR";
            player.OddsGroup = "A+";
            player.CommissionGroup = "High-Value";
            player.CustomerClass = "VIP*";
            player.RiskLevel = "Medium/High";
            player.RiskLevelKey = "MED_HIGH_RISK";

            // Assert
            Assert.Equal("test@user#123", player.UserName);
            Assert.Equal("SYS-001_v2", player.SystemId);
            Assert.Equal("€UR", player.Currency);
            Assert.Equal("A+", player.OddsGroup);
            Assert.Equal("High-Value", player.CommissionGroup);
            Assert.Equal("VIP*", player.CustomerClass);
            Assert.Equal("Medium/High", player.RiskLevel);
            Assert.Equal("MED_HIGH_RISK", player.RiskLevelKey);
        }

        [Fact]
        public void Player_WithMaxIntValues_StoresCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.IbcCustId = int.MaxValue;
            player.CurrencyId = int.MaxValue;
            player.SpecialLevel = int.MaxValue;
            player.Status = int.MaxValue;

            // Assert
            Assert.Equal(int.MaxValue, player.IbcCustId);
            Assert.Equal(int.MaxValue, player.CurrencyId);
            Assert.Equal(int.MaxValue, player.SpecialLevel);
            Assert.Equal(int.MaxValue, player.Status);
        }

        [Fact]
        public void Player_WithMinIntValues_StoresCorrectly()
        {
            // Arrange
            var player = new Player();

            // Act
            player.IbcCustId = int.MinValue;
            player.CurrencyId = int.MinValue;
            player.SpecialLevel = int.MinValue;
            player.Status = int.MinValue;

            // Assert
            Assert.Equal(int.MinValue, player.IbcCustId);
            Assert.Equal(int.MinValue, player.CurrencyId);
            Assert.Equal(int.MinValue, player.SpecialLevel);
            Assert.Equal(int.MinValue, player.Status);
        }
    }
}