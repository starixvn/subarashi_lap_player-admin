namespace Fanex.Athena.PlayerAdmin.API.UnitTests.DTO
{
    using API.DTO;

    public class PlayerActivityRecordTest
    {
        [Fact]
        public void PlayerActivityRecord_DefaultConstructor_CreatesInstance()
        {
            // Act
            var record = new PlayerActivityRecord();

            // Assert
            Assert.NotNull(record);
            Assert.Null(record.CreateDate);
            Assert.Null(record.LastLogged);
            Assert.Null(record.LastTransferDate);
            Assert.Null(record.LastUpdateBA);
            Assert.Null(record.BAUpdatedBy);
            Assert.Null(record.SpecialLevelUpdateBy);
            Assert.Null(record.SpecialLevelUpdateDate);
        }

        [Fact]
        public void PlayerActivityRecord_SetProperties_StoresValuesCorrectly()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            var createDate = new DateTime(2023, 1, 1, 10, 0, 0);
            var lastLogged = new DateTime(2023, 1, 2, 11, 0, 0);
            var lastTransferDate = new DateTime(2023, 1, 3, 12, 0, 0);
            var lastUpdateBA = new DateTime(2023, 1, 4, 13, 0, 0);
            var specialLevelUpdateDate = new DateTime(2023, 1, 5, 14, 0, 0);

            // Act
            record.CreateDate = createDate;
            record.LastLogged = lastLogged;
            record.LastTransferDate = lastTransferDate;
            record.LastUpdateBA = lastUpdateBA;
            record.BAUpdatedBy = "TestUser";
            record.SpecialLevelUpdateBy = "AdminUser";
            record.SpecialLevelUpdateDate = specialLevelUpdateDate;

            // Assert
            Assert.Equal(createDate, record.CreateDate);
            Assert.Equal(lastLogged, record.LastLogged);
            Assert.Equal(lastTransferDate, record.LastTransferDate);
            Assert.Equal(lastUpdateBA, record.LastUpdateBA);
            Assert.Equal("TestUser", record.BAUpdatedBy);
            Assert.Equal("AdminUser", record.SpecialLevelUpdateBy);
            Assert.Equal(specialLevelUpdateDate, record.SpecialLevelUpdateDate);
        }

        [Fact]
        public void CreatedDateString_WithValidDate_ReturnsFormattedString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            var createDate = new DateTime(2023, 1, 15, 14, 30, 45);
            record.CreateDate = createDate;

            // Act
            var result = record.CreatedDateString;

            // Assert
            Assert.Equal("01/15/2023 02:30:45 PM", result);
        }

        [Fact]
        public void CreatedDateString_WithNullDate_ReturnsEmptyString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            record.CreateDate = null;

            // Act
            var result = record.CreatedDateString;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void LastLoginString_WithValidDate_ReturnsFormattedString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            var lastLogged = new DateTime(2023, 12, 25, 23, 59, 59);
            record.LastLogged = lastLogged;

            // Act
            var result = record.LastLoginString;

            // Assert
            Assert.Equal("12/25/2023 11:59:59 PM", result);
        }

        [Fact]
        public void LastLoginString_WithNullDate_ReturnsEmptyString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            record.LastLogged = null;

            // Act
            var result = record.LastLoginString;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void LastTxn_WithValidDate_ReturnsFormattedString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            var lastTransferDate = new DateTime(2023, 6, 15, 9, 15, 30);
            record.LastTransferDate = lastTransferDate;

            // Act
            var result = record.LastTxn;

            // Assert
            Assert.Equal("06/15/2023 09:15:30 AM", result);
        }

        [Fact]
        public void LastTxn_WithNullDate_ReturnsEmptyString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            record.LastTransferDate = null;

            // Act
            var result = record.LastTxn;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void LastUpdateBAString_WithValidDate_ReturnsFormattedString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            var lastUpdateBA = new DateTime(2023, 3, 10, 16, 45, 20);
            record.LastUpdateBA = lastUpdateBA;

            // Act
            var result = record.LastUpdateBAString;

            // Assert
            Assert.Equal("03/10/2023 04:45:20 PM", result);
        }

        [Fact]
        public void LastUpdateBAString_WithNullDate_ReturnsEmptyString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            record.LastUpdateBA = null;

            // Act
            var result = record.LastUpdateBAString;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void LastUpdateSpecialLevelString_WithValidDate_ReturnsFormattedString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            var specialLevelUpdateDate = new DateTime(2023, 8, 20, 8, 30, 15);
            record.SpecialLevelUpdateDate = specialLevelUpdateDate;

            // Act
            var result = record.LastUpdateSpecialLevelString;

            // Assert
            Assert.Equal("08/20/2023 08:30:15 AM", result);
        }

        [Fact]
        public void LastUpdateSpecialLevelString_WithNullDate_ReturnsEmptyString()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            record.SpecialLevelUpdateDate = null;

            // Act
            var result = record.LastUpdateSpecialLevelString;

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void StringProperties_WithEmptyStrings_StoreCorrectly()
        {
            // Arrange
            var record = new PlayerActivityRecord();

            // Act
            record.BAUpdatedBy = "";
            record.SpecialLevelUpdateBy = "";

            // Assert
            Assert.Equal("", record.BAUpdatedBy);
            Assert.Equal("", record.SpecialLevelUpdateBy);
        }

        [Fact]
        public void StringProperties_WithSpecialCharacters_StoreCorrectly()
        {
            // Arrange
            var record = new PlayerActivityRecord();

            // Act
            record.BAUpdatedBy = "user@domain.com";
            record.SpecialLevelUpdateBy = "admin-user_123";

            // Assert
            Assert.Equal("user@domain.com", record.BAUpdatedBy);
            Assert.Equal("admin-user_123", record.SpecialLevelUpdateBy);
        }

        [Fact]
        public void DateTimeProperties_WithMinMaxValues_StoreCorrectly()
        {
            // Arrange
            var record = new PlayerActivityRecord();

            // Act
            record.CreateDate = DateTime.MinValue;
            record.LastLogged = DateTime.MaxValue;

            // Assert
            Assert.Equal(DateTime.MinValue, record.CreateDate);
            Assert.Equal(DateTime.MaxValue, record.LastLogged);
        }

        [Fact]
        public void FormattedStrings_WithMinMaxDates_ReturnCorrectFormat()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            record.CreateDate = DateTime.MinValue;
            record.LastLogged = DateTime.MaxValue;

            // Act
            var createdString = record.CreatedDateString;
            var lastLoginString = record.LastLoginString;

            // Assert
            Assert.Contains("01/01/0001", createdString);
            Assert.Contains("12/31/9999", lastLoginString);
        }

        [Fact]
        public void AllFormattedStrings_UseConsistentDateFormat()
        {
            // Arrange
            var record = new PlayerActivityRecord();
            var testDate = new DateTime(2023, 5, 10, 15, 30, 45);

            record.CreateDate = testDate;
            record.LastLogged = testDate;
            record.LastTransferDate = testDate;
            record.LastUpdateBA = testDate;
            record.SpecialLevelUpdateDate = testDate;

            // Act
            var expectedFormat = "05/10/2023 03:30:45 PM";

            // Assert
            Assert.Equal(expectedFormat, record.CreatedDateString);
            Assert.Equal(expectedFormat, record.LastLoginString);
            Assert.Equal(expectedFormat, record.LastTxn);
            Assert.Equal(expectedFormat, record.LastUpdateBAString);
            Assert.Equal(expectedFormat, record.LastUpdateSpecialLevelString);
        }
    }
}