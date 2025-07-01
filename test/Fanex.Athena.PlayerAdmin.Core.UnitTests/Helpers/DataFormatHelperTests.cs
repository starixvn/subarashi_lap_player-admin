using Fanex.Athena.PlayerAdmin.Core.Helpers;
using System;
using System.Globalization;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Helpers
{
    public class DataFormatHelperTests
    {
        [Theory]
        [InlineData(123, 123.0)]
        [InlineData("456.78", 456.78)]
        [InlineData(123.45d, 123.45)]
        public void AsDecimal_ValidInput_ReturnsDecimal(object value, double expected)
        {
            // Act
            var result = DataFormatHelper.AsDecimal(value);

            // Assert
            Assert.Equal((decimal)expected, result);
        }

        [Fact]
        public void AsDecimal_NullInput_ReturnsDefaultValue()
        {
            // Act
            var result = DataFormatHelper.AsDecimal(null, 5.5m, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(5.5m, result);
        }

        [Fact]
        public void AsDecimal_DBNullInput_ReturnsDefaultValue()
        {
            // Act
            var result = DataFormatHelper.AsDecimal(DBNull.Value, 5.5m, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(5.5m, result);
        }

        [Fact]
        public void AsDecimal_InvalidCast_ReturnsDefaultValue()
        {
            // Arrange
            var value = new object();

            // Act
            var result = DataFormatHelper.AsDecimal(value, 5.5m, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(5.5m, result);
        }

        [Fact]
        public void AsDecimal_InvalidFormat_ReturnsDefaultValue()
        {
            // Arrange
            var value = "invalid";

            // Act
            var result = DataFormatHelper.AsDecimal(value, 5.5m, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(5.5m, result);
        }

        [Fact]
        public void AsDecimal_FrenchCulture_ParsesCorrectly()
        {
            // Arrange
            var value = "123,45"; // Comma as decimal separator in French culture
            var culture = new CultureInfo("fr-FR");

            // Act
            var result = DataFormatHelper.AsDecimal(value, 0m, culture);

            // Assert
            Assert.Equal(123.45m, result);
        }
    }
} 