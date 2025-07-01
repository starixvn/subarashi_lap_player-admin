using System.Globalization;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    public class UserGuideControllerTests
    {
        private readonly Mock<IContextService> _mockContextService;
        private readonly UserGuideController _controller;

        public UserGuideControllerTests()
        {
            _mockContextService = new Mock<IContextService>();
            _controller = new UserGuideController(_mockContextService.Object);
        }

        [Fact]
        public void Index_WithValidParameters_ReturnsViewResult()
        {
            // Arrange
            var name = "test-guide";
            var culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // ViewName is not set when using View(name as object)
            Assert.Equal(name, viewResult.Model);
        }

        [Fact]
        public void Index_WithNullName_ReturnsViewResultWithNullModel()
        {
            // Arrange
            string name = null;
            var culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public void Index_WithEmptyName_ReturnsViewResultWithEmptyModel()
        {
            // Arrange
            var name = "";
            var culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal("", viewResult.Model);
        }

        [Fact]
        public void Index_WithDifferentCulture_SetsCorrectCulture()
        {
            // Arrange
            var name = "test-guide";
            var culture = "fr-FR";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(name, viewResult.Model);
            
            // Verify culture was set (this is a side effect test)
            Assert.Equal("fr-FR", CultureInfo.CurrentCulture.Name);
            Assert.Equal("fr-FR", CultureInfo.CurrentUICulture.Name);
        }

        [Fact]
        public void Index_WithInvalidCulture_ThrowsException()
        {
            // Arrange
            var name = "test-guide";
            var culture = "invalid-culture";

            // Act & Assert
            Assert.Throws<CultureNotFoundException>(() => _controller.Index(name, culture));
        }

        [Fact]
        public void Index_WithNullCulture_ThrowsException()
        {
            // Arrange
            var name = "test-guide";
            string culture = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _controller.Index(name, culture));
        }

        [Fact]
        public void Index_WithSpecialCharactersInName_ReturnsViewResult()
        {
            // Arrange
            var name = "test-guide-with-special-chars-123";
            var culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(name, viewResult.Model);
        }

        [Fact]
        public void Index_WithLongName_ReturnsViewResult()
        {
            // Arrange
            var name = "very-long-guide-name-that-might-be-used-in-real-scenarios";
            var culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            Assert.Equal(name, viewResult.Model);
        }
    }
} 