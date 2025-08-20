namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Helpers
{
    using System.Globalization;
    using Core.Helpers;

    public class DataFormatHelperTests
    {
        [Fact]
        public void AsDecimal_ShouldReturnZero_WhenNull()
        {
            var result = DataFormatHelper.AsDecimal(null);
            Assert.Equal(0m, result);
        }

        [Fact]
        public void AsDecimal_ShouldReturnDefault_WhenDbNull()
        {
            var result = DataFormatHelper.AsDecimal(DBNull.Value, 5m, CultureInfo.InvariantCulture);
            Assert.Equal(5m, result);
        }

        [Fact]
        public void AsDecimal_ShouldParseNumberString_WithInvariantCulture()
        {
            var result = DataFormatHelper.AsDecimal("1234.56", 0m, CultureInfo.InvariantCulture);
            Assert.Equal(1234.56m, result);
        }

        [Fact]
        public void AsDecimal_ShouldReturnDefault_WhenInvalidString()
        {
            var result = DataFormatHelper.AsDecimal("abc", 7m, CultureInfo.InvariantCulture);
            Assert.Equal(7m, result);
        }

        [Fact]
        public void AsDecimal_ShouldConvertNumericTypes()
        {
            var result = DataFormatHelper.AsDecimal(42);
            Assert.Equal(42m, result);
        }

        // Additional tests to improve coverage

        [Fact]
        public void AsDecimal_ShouldHandleInvalidCastException_WhenObjectCannotBeConverted()
        {
            // Arrange
            var invalidObject = new object(); // Object that cannot be converted to decimal
            var defaultValue = 100m;

            // Act
            var result = DataFormatHelper.AsDecimal(invalidObject, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void AsDecimal_ShouldUseCurrentCulture_WhenProviderIsNull()
        {
            // Arrange
            var value = "123.45";
            var defaultValue = 999m;

            // Act
            var result = DataFormatHelper.AsDecimal(value, defaultValue, null);

            // Assert
            Assert.Equal(123.45m, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleDoubleValue_WithInvariantCulture()
        {
            // Arrange
            double doubleValue = 567.89;
            var defaultValue = 0m;

            // Act
            var result = DataFormatHelper.AsDecimal(doubleValue, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(567.89m, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleFloatValue_WithInvariantCulture()
        {
            // Arrange
            float floatValue = 123.45f;
            var defaultValue = 0m;

            // Act
            var result = DataFormatHelper.AsDecimal(floatValue, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(123.45m, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleIntegerValue_WithInvariantCulture()
        {
            // Arrange
            int intValue = 999;
            var defaultValue = 0m;

            // Act
            var result = DataFormatHelper.AsDecimal(intValue, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(999m, result);
        }

        [Fact]
        public void AsDecimal_ShouldReturnDefault_WhenStringIsEmpty()
        {
            // Arrange
            var emptyString = "";
            var defaultValue = 50m;

            // Act
            var result = DataFormatHelper.AsDecimal(emptyString, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void AsDecimal_ShouldReturnDefault_WhenStringIsWhitespace()
        {
            // Arrange
            var whitespaceString = "   ";
            var defaultValue = 75m;

            // Act
            var result = DataFormatHelper.AsDecimal(whitespaceString, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleDecimalString_WithCurrentCulture()
        {
            // Arrange
            var decimalString = "1234.56";

            // Act
            var result = DataFormatHelper.AsDecimal(decimalString);

            // Assert
            Assert.Equal(1234.56m, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleNegativeDecimal_WithInvariantCulture()
        {
            // Arrange
            var negativeDecimalString = "-987.65";
            var defaultValue = 0m;

            // Act
            var result = DataFormatHelper.AsDecimal(negativeDecimalString, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(-987.65m, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleZeroValue_WithInvariantCulture()
        {
            // Arrange
            var zeroString = "0";
            var defaultValue = 100m;

            // Act
            var result = DataFormatHelper.AsDecimal(zeroString, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleVeryLargeDecimal_WithInvariantCulture()
        {
            // Arrange
            var largeDecimalString = "79228162514264337593543950335"; // Decimal.MaxValue
            var defaultValue = 0m;

            // Act
            var result = DataFormatHelper.AsDecimal(largeDecimalString, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(decimal.MaxValue, result);
        }

        [Fact]
        public void AsDecimal_ShouldReturnDefault_WhenStringContainsSpecialCharacters()
        {
            // Arrange
            var invalidString = "$123.45@";
            var defaultValue = 200m;

            // Act
            var result = DataFormatHelper.AsDecimal(invalidString, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Theory]
        [InlineData("", 30, 30)]
        [InlineData("   ", 35, 35)]
        [InlineData("invalid", 40, 40)]
        [InlineData("123.45", 50, 123.45)]
        [InlineData("123", 60, 123)]
        [InlineData("0", 70, 0)]
        [InlineData("-456.78", 80, -456.78)]
        public void AsDecimal_TheoryTest_ShouldHandleStringInputs_WithDefaultValue(string input, double defaultValueDouble, double expectedDouble)
        {
            // Arrange
            var defaultValue = (decimal)defaultValueDouble;
            var expected = (decimal)expectedDouble;

            // Act
            var result = DataFormatHelper.AsDecimal(input, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleNullInput_WithTheoryPattern()
        {
            // Arrange
            object input = null;
            var defaultValue = 10m;
            var expected = 10m;

            // Act
            var result = DataFormatHelper.AsDecimal(input, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void AsDecimal_ShouldHandleDBNullInput_WithTheoryPattern()
        {
            // Arrange
            var input = DBNull.Value;
            var defaultValue = 25m;
            var expected = 25m;

            // Act
            var result = DataFormatHelper.AsDecimal(input, defaultValue, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}