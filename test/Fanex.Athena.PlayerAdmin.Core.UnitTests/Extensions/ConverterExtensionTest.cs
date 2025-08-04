namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using System.Globalization;
    using Core.Extensions;
    using Xunit;

    public class ConverterExtensionTest
    {
        [Theory]
        [InlineData("123.45", 123.45)]
        [InlineData("0", 0)]
        [InlineData("-123.45", -123.45)]
        [InlineData("999999999.99", 999999999.99)]
        public void ToDecimal_ReturnsCorrectValue_WhenValidDecimalString(string input, decimal expected)
        {
            // Act
            var result = Converter.ToDecimal(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("invalid", 0)]
        [InlineData("", 0)]
        [InlineData("abc123", 0)]
        [InlineData("123.45.67", 0)]
        public void ToDecimal_ReturnsDefaultValue_WhenInvalidString(string input, decimal expected)
        {
            // Act
            var result = Converter.ToDecimal(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDecimal_ReturnsCustomDefaultValue_WhenInvalidString()
        {
            // Arrange
            var input = "invalid";
            var defaultValue = 99.99m;

            // Act
            var result = Converter.ToDecimal(input, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void ToDecimal_HandlesNullInput()
        {
            // Act
            var result = Converter.ToDecimal(null);

            // Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("TRUE", true)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("FALSE", false)]
        public void ToBoolean_ReturnsCorrectValue_WhenValidBooleanString(string input, bool expected)
        {
            // Act
            var result = Converter.ToBoolean(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("0")]
        [InlineData("yes")]
        [InlineData("no")]
        public void ToBoolean_ReturnsFalse_WhenInvalidString(string input)
        {
            // Act
            var result = Converter.ToBoolean(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ToBoolean_HandlesNullInput()
        {
            // Act
            var result = Converter.ToBoolean(null);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("123", 123)]
        [InlineData("0", 0)]
        [InlineData("-123", -123)]
        [InlineData("2147483647", 2147483647)]
        [InlineData("-2147483648", -2147483648)]
        public void ToInt_ReturnsCorrectValue_WhenValidIntegerString(string input, int expected)
        {
            // Act
            var result = Converter.ToInt(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("invalid", 0)]
        [InlineData("", 0)]
        [InlineData("123.45", 0)]
        [InlineData("abc123", 0)]
        [InlineData("2147483648", 0)] // Overflow
        public void ToInt_ReturnsDefaultValue_WhenInvalidString(string input, int expected)
        {
            // Act
            var result = Converter.ToInt(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToInt_ReturnsCustomDefaultValue_WhenInvalidString()
        {
            // Arrange
            var input = "invalid";
            var defaultValue = 999;

            // Act
            var result = Converter.ToInt(input, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void ToInt_HandlesNullInput()
        {
            // Act
            var result = Converter.ToInt(null);

            // Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("", "")]
        [InlineData("123", "123")]
        [InlineData("special chars !@#$%", "special chars !@#$%")]
        public void ToString_ReturnsCorrectValue_WhenValidInput(object input, string expected)
        {
            // Act
            var result = Converter.ToString(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToString_ReturnsDefaultValue_WhenNullInput()
        {
            // Act
            var result = Converter.ToString(null);

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ToString_ReturnsCustomDefaultValue_WhenNullInput()
        {
            // Arrange
            var defaultValue = "default";

            // Act
            var result = Converter.ToString(null, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void ToString_HandlesComplexObjects()
        {
            // Arrange
            var obj = new { Name = "Test", Value = 123 };

            // Act
            var result = Converter.ToString(obj);

            // Assert
            Assert.Contains("Name", result);
            Assert.Contains("Test", result);
            Assert.Contains("Value", result);
            Assert.Contains("123", result);
        }

        [Theory]
        [InlineData("2023-01-01", 2023, 1, 1)]
        [InlineData("2023-12-31 23:59:59", 2023, 12, 31)]
        [InlineData("01/01/2023", 2023, 1, 1)]
        public void ToDateTime_ReturnsCorrectValue_WhenValidDateTimeString(string input, int year, int month, int day)
        {
            // Act
            var result = Converter.ToDateTime(input);

            // Assert
            Assert.Equal(year, result.Year);
            Assert.Equal(month, result.Month);
            Assert.Equal(day, result.Day);
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("")]
        [InlineData("2023-13-01")] // Invalid month
        [InlineData("2023-01-32")] // Invalid day
        [InlineData("not a date")]
        public void ToDateTime_ReturnsDefault_WhenInvalidString(string input)
        {
            // Act
            var result = Converter.ToDateTime(input);

            // Assert
            Assert.Equal(default, result);
        }

        [Fact]
        public void ToDateTime_HandlesNullInput()
        {
            // Act
            var result = Converter.ToDateTime(null);

            // Assert
            Assert.Equal(default, result);
        }

        [Fact]
        public void ToDateTime_UsesInvariantCulture()
        {
            // Arrange
            var dateString = "12/31/2023"; // US format
            var originalCulture = CultureInfo.CurrentCulture;

            try
            {
                // Set to a culture that uses different date format
                CultureInfo.CurrentCulture = new CultureInfo("de-DE");

                // Act
                var result = Converter.ToDateTime(dateString);

                // Assert
                // Should parse using InvariantCulture, not German culture
                Assert.NotEqual(default, result);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public void AllMethods_HandleDifferentObjectTypes()
        {
            // Arrange
            var intObj = (object)123;
            var decimalObj = (object)123.45m;
            var boolObj = (object)true;
            var stringObj = (object)"test";

            // Act & Assert
            Assert.Equal(123, Converter.ToInt(intObj));
            Assert.Equal(123.45m, Converter.ToDecimal(decimalObj));
            Assert.True(Converter.ToBoolean(boolObj));
            Assert.Equal("test", Converter.ToString(stringObj));
        }
    }
}