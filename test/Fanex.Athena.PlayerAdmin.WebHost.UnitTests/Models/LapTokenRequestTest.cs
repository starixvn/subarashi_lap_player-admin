namespace Fanex.Athena.PlayerAdmin.WebHost.UnitTests.Models
{
    using System.ComponentModel.DataAnnotations;
    using WebHost.Models;

    public class LapTokenRequestTest
    {
        [Fact]
        public void DefaultConstructor_CreatesModel()
        {
            // Act
            var model = new LapTokenRequest();

            // Assert
            Assert.NotNull(model);
        }

        [Fact]
        public void Token_CanBeSetAndRetrieved()
        {
            // Arrange
            var model = new LapTokenRequest();
            const string expectedToken = "test-token-123";

            // Act
            model.Token = expectedToken;

            // Assert
            Assert.Equal(expectedToken, model.Token);
        }

        [Fact]
        public void Token_WithValidLength_PassesValidation()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = "valid-token"
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Token_WithNullValue_FailsValidation()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = null
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.ErrorMessage.Contains("required") || r.ErrorMessage.Contains("Required"));
        }

        [Fact]
        public void Token_WithEmptyString_FailsValidation()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = ""
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Token_WithMaxLength_PassesValidation()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = new string('a', 30) // Max length is 30
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }

        [Fact]
        public void Token_WithExceedsMaxLength_FailsValidation()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = new string('a', 31) // Exceeds max length of 30
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.ErrorMessage.Contains("length") || r.ErrorMessage.Contains("Length"));
        }

        [Fact]
        public void Token_HasRequiredAttribute()
        {
            // Arrange
            var property = typeof(LapTokenRequest).GetProperty("Token");

            // Assert
            Assert.NotNull(property);
            var requiredAttribute = property.GetCustomAttributes(typeof(RequiredAttribute), false);
            Assert.NotEmpty(requiredAttribute);
        }

        [Fact]
        public void Token_HasStringLengthAttribute()
        {
            // Arrange
            var property = typeof(LapTokenRequest).GetProperty("Token");

            // Assert
            Assert.NotNull(property);
            var stringLengthAttributes = property.GetCustomAttributes(typeof(StringLengthAttribute), false);
            Assert.NotEmpty(stringLengthAttributes);

            var stringLengthAttribute = (StringLengthAttribute)stringLengthAttributes.First();
            Assert.Equal(30, stringLengthAttribute.MaximumLength);
        }

        [Fact]
        public void DefaultValue_IsNull()
        {
            // Act
            var model = new LapTokenRequest();

            // Assert
            Assert.Null(model.Token);
        }

        [Fact]
        public void Token_WithSpecialCharacters_CanBeSet()
        {
            // Arrange
            var model = new LapTokenRequest();
            const string specialToken = "token-!@#$%^&*()_+-={}[]";

            // Act
            model.Token = specialToken;

            // Assert
            Assert.Equal(specialToken, model.Token);
        }

        [Fact]
        public void Token_WithNumericCharacters_CanBeSet()
        {
            // Arrange
            var model = new LapTokenRequest();
            const string numericToken = "1234567890";

            // Act
            model.Token = numericToken;

            // Assert
            Assert.Equal(numericToken, model.Token);
        }

        [Fact]
        public void Token_WithMixedCaseCharacters_CanBeSet()
        {
            // Arrange
            var model = new LapTokenRequest();
            const string mixedCaseToken = "AbCdEfGhIjKlMnOpQrStUvWxYz";

            // Act
            model.Token = mixedCaseToken;

            // Assert
            Assert.Equal(mixedCaseToken, model.Token);
        }

        [Fact]
        public void ValidationContext_WithPropertyName_ValidatesSpecificProperty()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = new string('a', 31) // Exceeds max length
            };
            var context = new ValidationContext(model) { MemberName = "Token" };
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateProperty(model.Token, context, results);

            // Assert
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Model_HasCorrectNamespace()
        {
            // Assert
            Assert.Equal("Fanex.Athena.PlayerAdmin.WebHost.Models", typeof(LapTokenRequest).Namespace);
        }

        [Fact]
        public void Token_WithWhitespaceOnly_FailsValidation()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = "   " // Only whitespace
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            // Note: The Required attribute actually fails validation for whitespace-only strings
            // This test documents the actual behavior
            Assert.False(isValid); // Whitespace-only strings fail Required validation
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Token_WithSingleCharacter_PassesValidation()
        {
            // Arrange
            var model = new LapTokenRequest
            {
                Token = "a"
            };
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, context, results, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}