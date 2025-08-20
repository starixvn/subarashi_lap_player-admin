namespace Fanex.Athena.PlayerAdmin.WebHost.UnitTests.Models
{
    using WebHost.Models;

    public class ErrorViewModelTest
    {
        [Fact]
        public void DefaultConstructor_CreatesModel()
        {
            // Act
            var model = new ErrorViewModel();

            // Assert
            Assert.NotNull(model);
        }

        [Fact]
        public void RequestId_CanBeSetAndRetrieved()
        {
            // Arrange
            var model = new ErrorViewModel();
            const string expectedRequestId = "test-request-id-123";

            // Act
            model.RequestId = expectedRequestId;

            // Assert
            Assert.Equal(expectedRequestId, model.RequestId);
        }

        [Fact]
        public void Code_CanBeSetAndRetrieved()
        {
            // Arrange
            var model = new ErrorViewModel();
            const int expectedCode = 404;

            // Act
            model.Code = expectedCode;

            // Assert
            Assert.Equal(expectedCode, model.Code);
        }

        [Fact]
        public void ShowRequestId_WithNonEmptyRequestId_ReturnsTrue()
        {
            // Arrange
            var model = new ErrorViewModel
            {
                RequestId = "test-request-id"
            };

            // Act
            var result = model.ShowRequestId;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShowRequestId_WithEmptyRequestId_ReturnsFalse()
        {
            // Arrange
            var model = new ErrorViewModel
            {
                RequestId = ""
            };

            // Act
            var result = model.ShowRequestId;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShowRequestId_WithNullRequestId_ReturnsFalse()
        {
            // Arrange
            var model = new ErrorViewModel
            {
                RequestId = null
            };

            // Act
            var result = model.ShowRequestId;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShowRequestId_WithWhitespaceRequestId_ReturnsTrue()
        {
            // Arrange
            var model = new ErrorViewModel
            {
                RequestId = "   "
            };

            // Act
            var result = model.ShowRequestId;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DefaultValues_AreCorrect()
        {
            // Act
            var model = new ErrorViewModel();

            // Assert
            Assert.Null(model.RequestId);
            Assert.Equal(0, model.Code);
            Assert.False(model.ShowRequestId);
        }

        [Fact]
        public void AllProperties_CanBeSetSimultaneously()
        {
            // Arrange
            var model = new ErrorViewModel();
            const string expectedRequestId = "multi-prop-test-id";
            const int expectedCode = 500;

            // Act
            model.RequestId = expectedRequestId;
            model.Code = expectedCode;

            // Assert
            Assert.Equal(expectedRequestId, model.RequestId);
            Assert.Equal(expectedCode, model.Code);
            Assert.True(model.ShowRequestId);
        }

        [Fact]
        public void Code_WithNegativeValue_CanBeSet()
        {
            // Arrange
            var model = new ErrorViewModel();
            const int negativeCode = -1;

            // Act
            model.Code = negativeCode;

            // Assert
            Assert.Equal(negativeCode, model.Code);
        }

        [Fact]
        public void Code_WithLargeValue_CanBeSet()
        {
            // Arrange
            var model = new ErrorViewModel();
            const int largeCode = 999999;

            // Act
            model.Code = largeCode;

            // Assert
            Assert.Equal(largeCode, model.Code);
        }

        [Fact]
        public void RequestId_WithSpecialCharacters_CanBeSet()
        {
            // Arrange
            var model = new ErrorViewModel();
            const string specialRequestId = "test-id-!@#$%^&*()_+-={}[]|\\:;\"'<>?,./";

            // Act
            model.RequestId = specialRequestId;

            // Assert
            Assert.Equal(specialRequestId, model.RequestId);
            Assert.True(model.ShowRequestId);
        }

        [Fact]
        public void RequestId_WithVeryLongString_CanBeSet()
        {
            // Arrange
            var model = new ErrorViewModel();
            var longRequestId = new string('a', 1000);

            // Act
            model.RequestId = longRequestId;

            // Assert
            Assert.Equal(longRequestId, model.RequestId);
            Assert.True(model.ShowRequestId);
        }

        [Fact]
        public void ShowRequestId_IsReadOnlyProperty()
        {
            // Arrange
            var model = new ErrorViewModel();

            // Assert
            var property = typeof(ErrorViewModel).GetProperty("ShowRequestId");
            Assert.NotNull(property);
            Assert.True(property.CanRead);
            Assert.False(property.CanWrite);
        }

        [Fact]
        public void Properties_AreIndependent()
        {
            // Arrange
            var model = new ErrorViewModel();

            // Act
            model.RequestId = "test-id";
            model.Code = 404;

            // Assert - Changing one property doesn't affect others
            Assert.Equal("test-id", model.RequestId);
            Assert.Equal(404, model.Code);
            Assert.True(model.ShowRequestId);

            // Change RequestId to null
            model.RequestId = null;

            // Code should remain unchanged
            Assert.Equal(404, model.Code);
            Assert.False(model.ShowRequestId);
        }

        [Fact]
        public void Model_HasCorrectNamespace()
        {
            // Assert
            Assert.Equal("Fanex.Athena.PlayerAdmin.WebHost.Models", typeof(ErrorViewModel).Namespace);
        }
    }
}