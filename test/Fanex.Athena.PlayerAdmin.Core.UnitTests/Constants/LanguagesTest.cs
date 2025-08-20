namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Constants
{
    using Core.Constants;

    public class LanguagesTest
    {
        [Theory]
        [InlineData("en", "en-US")]
        [InlineData("cs", "zh-CN")]
        [InlineData("vn", "vi-VN")]
        [InlineData("th", "th-TH")]
        [InlineData("id", "id-ID")]
        public void LanguageCodeMappers_ContainsExpectedMappings(string key, string expectedValue)
        {
            // Act
            var actualValue = Languages.LanguageCodeMappers[key];

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LanguageCodeMappers_IsReadOnly()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IReadOnlyDictionary<string, string>>(Languages.LanguageCodeMappers);
        }

        [Fact]
        public void LanguageCodeMappers_IsCaseInsensitive()
        {
            // Arrange
            const string lowerKey = "en";
            const string upperKey = "EN";
            const string mixedKey = "En";

            // Act
            var lowerResult = Languages.LanguageCodeMappers[lowerKey];
            var upperResult = Languages.LanguageCodeMappers[upperKey];
            var mixedResult = Languages.LanguageCodeMappers[mixedKey];

            // Assert
            Assert.Equal("en-US", lowerResult);
            Assert.Equal("en-US", upperResult);
            Assert.Equal("en-US", mixedResult);
            Assert.Equal(lowerResult, upperResult);
            Assert.Equal(upperResult, mixedResult);
        }

        [Fact]
        public void LanguageCodeMappers_HandlesMissingKeys()
        {
            // Arrange
            const string nonExistentKey = "nonexistent";

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => Languages.LanguageCodeMappers[nonExistentKey]);
        }

        [Fact]
        public void LanguageCodeMappers_ContainsAllExpectedKeys()
        {
            // Arrange
            var expectedKeys = new[] { "en", "cs", "vn", "th", "id" };

            // Act
            var actualKeys = Languages.LanguageCodeMappers.Keys.ToArray();

            // Assert
            Assert.Equal(expectedKeys.Length, actualKeys.Length);
            foreach (var expectedKey in expectedKeys)
            {
                Assert.Contains(expectedKey, actualKeys, StringComparer.InvariantCultureIgnoreCase);
            }
        }

        [Fact]
        public void LanguageCodeMappers_TryGetValue_ReturnsCorrectResults()
        {
            // Act & Assert - existing key
            var existsResult = Languages.LanguageCodeMappers.TryGetValue("en", out var existingValue);
            Assert.True(existsResult);
            Assert.Equal("en-US", existingValue);

            // Act & Assert - non-existing key
            var notExistsResult = Languages.LanguageCodeMappers.TryGetValue("nonexistent", out var nonExistingValue);
            Assert.False(notExistsResult);
            Assert.Null(nonExistingValue);
        }

        [Fact]
        public void LanguageCodeMappers_CaseInsensitive_TryGetValue()
        {
            // Act & Assert
            var lowerResult = Languages.LanguageCodeMappers.TryGetValue("en", out var lowerValue);
            var upperResult = Languages.LanguageCodeMappers.TryGetValue("EN", out var upperValue);

            Assert.True(lowerResult);
            Assert.True(upperResult);
            Assert.Equal("en-US", lowerValue);
            Assert.Equal("en-US", upperValue);
            Assert.Equal(lowerValue, upperValue);
        }

        [Fact]
        public void LanguageCodeMappers_ContainsKey_WorksCorrectly()
        {
            // Act & Assert
            Assert.True(Languages.LanguageCodeMappers.ContainsKey("en"));
            Assert.True(Languages.LanguageCodeMappers.ContainsKey("EN"));
            Assert.True(Languages.LanguageCodeMappers.ContainsKey("En"));
            Assert.False(Languages.LanguageCodeMappers.ContainsKey("nonexistent"));
        }

        [Fact]
        public void LanguageCodeMappers_AllMappingsAreValidCultureCodes()
        {
            // Act & Assert
            foreach (var mapping in Languages.LanguageCodeMappers)
            {
                Assert.NotNull(mapping.Key);
                Assert.NotEmpty(mapping.Key);
                Assert.NotNull(mapping.Value);
                Assert.NotEmpty(mapping.Value);
                Assert.Contains("-", mapping.Value); // Culture codes should contain a hyphen
            }
        }
    }
}