namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc;
    using MVC.Controllers;

    public class UserGuideControllerTest
    {
        private readonly UserGuideController _controller;

        public UserGuideControllerTest()
        {
            _controller = new UserGuideController();
        }

        [Fact]
        public void Constructor_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void Index_WithValidNameAndCulture_ReturnsViewWithCorrectModel()
        {
            // Arrange
            const string name = "TestGuide";
            const string culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(name, viewResult.Model);
        }

        [Fact]
        public void Index_WithValidNameAndCulture_SetsCultureCorrectly()
        {
            // Arrange
            const string name = "TestGuide";
            const string culture = "fr-FR";
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                // Act
                var result = _controller.Index(name, culture);

                // Assert
                Assert.Equal(culture, CultureInfo.CurrentCulture.Name);
                Assert.Equal(culture, CultureInfo.CurrentUICulture.Name);
            }
            finally
            {
                // Restore original culture
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }

        [Fact]
        public void Index_WithDifferentCultures_SetsCultureCorrectly()
        {
            // Arrange
            const string name = "TestGuide";
            const string culture = "zh-CN";
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                // Act
                var result = _controller.Index(name, culture);

                // Assert
                Assert.Equal(culture, CultureInfo.CurrentCulture.Name);
                Assert.Equal(culture, CultureInfo.CurrentUICulture.Name);
                Assert.NotNull(result);
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal(name, viewResult.Model);
            }
            finally
            {
                // Restore original culture
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }

        [Fact]
        public void Index_WithNullName_ReturnsViewWithNullModel()
        {
            // Arrange
            string name = null;
            const string culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.Model);
        }

        [Fact]
        public void Index_WithEmptyName_ReturnsViewWithEmptyModel()
        {
            // Arrange
            const string name = "";
            const string culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("", viewResult.Model);
        }

        // [Fact]
        // public void Index_WithInvalidCulture_ThrowsCultureNotFoundException()
        // {
        //     // Arrange
        //     const string name = "TestGuide";
        //     const string culture = "invalid-culture";

        //     // Act & Assert
        //     Assert.Throws<CultureNotFoundException>(() => _controller.Index(name, culture));
        // }

        [Fact]
        public void Index_WithNullCulture_ThrowsArgumentNullException()
        {
            // Arrange
            const string name = "TestGuide";
            string culture = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _controller.Index(name, culture));
        }

        [Fact]
        public void Index_WithSpecialCharactersInName_ReturnsViewWithCorrectModel()
        {
            // Arrange
            const string name = "Test-Guide_123";
            const string culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(name, viewResult.Model);
        }

        [Fact]
        public void Index_WithLongName_ReturnsViewWithCorrectModel()
        {
            // Arrange
            const string name = "VeryLongUserGuideNameThatExceedsNormalLength";
            const string culture = "en-US";

            // Act
            var result = _controller.Index(name, culture);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(name, viewResult.Model);
        }
    }
}
