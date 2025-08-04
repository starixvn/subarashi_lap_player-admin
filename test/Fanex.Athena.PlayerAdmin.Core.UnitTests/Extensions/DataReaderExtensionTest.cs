namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using System;
    using System.Data;
    using Core.Extensions;
    using Moq;
    using Xunit;

    public class DataReaderExtensionTest
    {
        private readonly Mock<IDataReader> _mockDataReader;

        public DataReaderExtensionTest()
        {
            _mockDataReader = new Mock<IDataReader>();
        }

        [Fact]
        public void ToDecimal_ReturnsCorrectValue_WhenValidDecimal()
        {
            // Arrange
            var columnName = "Amount";
            var expectedValue = 123.45m;
            _mockDataReader.Setup(x => x[columnName]).Returns(expectedValue);

            // Act
            var result = _mockDataReader.Object.ToDecimal(columnName);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void ToDecimal_ReturnsZero_WhenDBNull()
        {
            // Arrange
            var columnName = "Amount";
            _mockDataReader.Setup(x => x[columnName]).Returns(DBNull.Value);

            // Act
            var result = _mockDataReader.Object.ToDecimal(columnName);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void ToDecimal_ReturnsZero_WhenNull()
        {
            // Arrange
            var columnName = "Amount";
            _mockDataReader.Setup(x => x[columnName]).Returns(null);

            // Act
            var result = _mockDataReader.Object.ToDecimal(columnName);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void ToDecimal_ReturnsCorrectValue_WhenStringDecimal()
        {
            // Arrange
            var columnName = "Amount";
            var stringValue = "456.78";
            _mockDataReader.Setup(x => x[columnName]).Returns(stringValue);

            // Act
            var result = _mockDataReader.Object.ToDecimal(columnName);

            // Assert
            Assert.Equal(456.78m, result);
        }

        [Fact]
        public void ToBool_ReturnsTrue_WhenValidTrue()
        {
            // Arrange
            var columnName = "IsActive";
            _mockDataReader.Setup(x => x[columnName]).Returns(true);

            // Act
            var result = _mockDataReader.Object.ToBool(columnName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ToBool_ReturnsFalse_WhenValidFalse()
        {
            // Arrange
            var columnName = "IsActive";
            _mockDataReader.Setup(x => x[columnName]).Returns(false);

            // Act
            var result = _mockDataReader.Object.ToBool(columnName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToBool_ReturnsFalse_WhenDBNull()
        {
            // Arrange
            var columnName = "IsActive";
            _mockDataReader.Setup(x => x[columnName]).Returns(DBNull.Value);

            // Act
            var result = _mockDataReader.Object.ToBool(columnName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToBool_ReturnsTrue_WhenStringTrue()
        {
            // Arrange
            var columnName = "IsActive";
            _mockDataReader.Setup(x => x[columnName]).Returns("true");

            // Act
            var result = _mockDataReader.Object.ToBool(columnName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ToBool_ReturnsFalse_WhenInvalidString()
        {
            // Arrange
            var columnName = "IsActive";
            _mockDataReader.Setup(x => x[columnName]).Returns("invalid");

            // Act
            var result = _mockDataReader.Object.ToBool(columnName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToString_ReturnsCorrectValue_WhenValidString()
        {
            // Arrange
            var columnName = "Name";
            var expectedValue = "Test Name";
            _mockDataReader.Setup(x => x[columnName]).Returns(expectedValue);

            // Act
            var result = _mockDataReader.Object.ToString(columnName);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void ToString_ReturnsEmptyString_WhenDBNull()
        {
            // Arrange
            var columnName = "Name";
            _mockDataReader.Setup(x => x[columnName]).Returns(DBNull.Value);

            // Act
            var result = _mockDataReader.Object.ToString(columnName);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ToString_ReturnsEmptyString_WhenNull()
        {
            // Arrange
            var columnName = "Name";
            _mockDataReader.Setup(x => x[columnName]).Returns(null);

            // Act
            var result = _mockDataReader.Object.ToString(columnName);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ToString_ReturnsStringRepresentation_WhenNonStringType()
        {
            // Arrange
            var columnName = "Id";
            var intValue = 123;
            _mockDataReader.Setup(x => x[columnName]).Returns(intValue);

            // Act
            var result = _mockDataReader.Object.ToString(columnName);

            // Assert
            Assert.Equal("123", result);
        }

        [Fact]
        public void ToInt_ReturnsCorrectValue_WhenValidInteger()
        {
            // Arrange
            var columnName = "Id";
            var expectedValue = 123;
            _mockDataReader.Setup(x => x[columnName]).Returns(expectedValue);

            // Act
            var result = _mockDataReader.Object.ToInt(columnName);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void ToInt_ReturnsZero_WhenDBNull()
        {
            // Arrange
            var columnName = "Id";
            _mockDataReader.Setup(x => x[columnName]).Returns(DBNull.Value);

            // Act
            var result = _mockDataReader.Object.ToInt(columnName);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void ToInt_ReturnsCorrectValue_WhenStringInteger()
        {
            // Arrange
            var columnName = "Id";
            var stringValue = "456";
            _mockDataReader.Setup(x => x[columnName]).Returns(stringValue);

            // Act
            var result = _mockDataReader.Object.ToInt(columnName);

            // Assert
            Assert.Equal(456, result);
        }

        [Fact]
        public void ToInt_ReturnsZero_WhenInvalidString()
        {
            // Arrange
            var columnName = "Id";
            _mockDataReader.Setup(x => x[columnName]).Returns("invalid");

            // Act
            var result = _mockDataReader.Object.ToInt(columnName);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void ToDateTime_ReturnsCorrectValue_WhenValidDateTime()
        {
            // Arrange
            var columnName = "CreatedDate";
            var expectedValue = new DateTime(2023, 1, 1, 12, 0, 0);
            _mockDataReader.Setup(x => x[columnName]).Returns(expectedValue);

            // Act
            var result = _mockDataReader.Object.ToDateTime(columnName);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void ToDateTime_ReturnsDefault_WhenDBNull()
        {
            // Arrange
            var columnName = "CreatedDate";
            _mockDataReader.Setup(x => x[columnName]).Returns(DBNull.Value);

            // Act
            var result = _mockDataReader.Object.ToDateTime(columnName);

            // Assert
            Assert.Equal(default, result);
        }

        [Fact]
        public void ToDateTime_ReturnsCorrectValue_WhenStringDateTime()
        {
            // Arrange
            var columnName = "CreatedDate";
            var stringValue = "2023-01-01 12:00:00";
            _mockDataReader.Setup(x => x[columnName]).Returns(stringValue);

            // Act
            var result = _mockDataReader.Object.ToDateTime(columnName);

            // Assert
            Assert.Equal(new DateTime(2023, 1, 1, 12, 0, 0), result);
        }

        [Fact]
        public void ToDateTime_ReturnsDefault_WhenInvalidString()
        {
            // Arrange
            var columnName = "CreatedDate";
            _mockDataReader.Setup(x => x[columnName]).Returns("invalid date");

            // Act
            var result = _mockDataReader.Object.ToDateTime(columnName);

            // Assert
            Assert.Equal(default, result);
        }

        [Fact]
        public void AllMethods_HandleDifferentColumnNames()
        {
            // Arrange
            _mockDataReader.Setup(x => x["decimal_col"]).Returns(123.45m);
            _mockDataReader.Setup(x => x["bool_col"]).Returns(true);
            _mockDataReader.Setup(x => x["string_col"]).Returns("test");
            _mockDataReader.Setup(x => x["int_col"]).Returns(789);
            _mockDataReader.Setup(x => x["date_col"]).Returns(new DateTime(2023, 1, 1));

            // Act & Assert
            Assert.Equal(123.45m, _mockDataReader.Object.ToDecimal("decimal_col"));
            Assert.True(_mockDataReader.Object.ToBool("bool_col"));
            Assert.Equal("test", _mockDataReader.Object.ToString("string_col"));
            Assert.Equal(789, _mockDataReader.Object.ToInt("int_col"));
            Assert.Equal(new DateTime(2023, 1, 1), _mockDataReader.Object.ToDateTime("date_col"));
        }
    }
}