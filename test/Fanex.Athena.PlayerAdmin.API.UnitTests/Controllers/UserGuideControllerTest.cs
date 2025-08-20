namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    using API.Controllers;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.FileProviders;
    using Moq;

    public class UserGuideControllerTest
    {
        private readonly UserGuideController _controller;
        private readonly Mock<IContextService> _mockContextService;

        public UserGuideControllerTest()
        {
            _mockContextService = new Mock<IContextService>();
            _controller = new UserGuideController(_mockContextService.Object);
        }

        [Fact]
        public void Constructor_WithValidContextService_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void OddsGroupPdf_WithExistingFileInUserLanguage_ReturnsFileResult()
        {
            // Arrange
            var mockFileProvider = new Mock<IFileProvider>();
            var mockFileInfo = new Mock<IFileInfo>();
            var mockStream = new MemoryStream();
            var fileName = "test.pdf";
            var language = "zh-CN";

            _mockContextService.Setup(x => x.Language).Returns(language);
            mockFileInfo.Setup(x => x.Exists).Returns(true);
            mockFileInfo.Setup(x => x.CreateReadStream()).Returns(mockStream);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(mockFileInfo.Object);

            // Act
            var result = _controller.OddsGroupPdf(mockFileProvider.Object, fileName);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            Assert.Equal(mockStream, fileResult.FileStream);
        }

        [Fact]
        public void OddsGroupPdf_WithNonExistingFileInUserLanguage_ReturnsDefaultLanguageFile()
        {
            // Arrange
            var mockFileProvider = new Mock<IFileProvider>();
            var mockUserLanguageFileInfo = new Mock<IFileInfo>();
            var mockDefaultLanguageFileInfo = new Mock<IFileInfo>();
            var mockStream = new MemoryStream();
            var fileName = "test.pdf";
            var userLanguage = "fr-FR";
            var defaultLanguage = "en-US";

            _mockContextService.Setup(x => x.Language).Returns(userLanguage);

            // User language file doesn't exist
            mockUserLanguageFileInfo.Setup(x => x.Exists).Returns(false);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{userLanguage}/{fileName}"))
                .Returns(mockUserLanguageFileInfo.Object);

            // Default language file exists
            mockDefaultLanguageFileInfo.Setup(x => x.Exists).Returns(true);
            mockDefaultLanguageFileInfo.Setup(x => x.CreateReadStream()).Returns(mockStream);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{defaultLanguage}/{fileName}"))
                .Returns(mockDefaultLanguageFileInfo.Object);

            // Act
            var result = _controller.OddsGroupPdf(mockFileProvider.Object, fileName);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            Assert.Equal(mockStream, fileResult.FileStream);

            // Verify both file checks were made
            mockFileProvider.Verify(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{userLanguage}/{fileName}"), Times.Once);
            mockFileProvider.Verify(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{defaultLanguage}/{fileName}"), Times.Once);
        }

        [Fact]
        public void OddsGroupPdf_WithEnglishLanguage_ReturnsEnglishFile()
        {
            // Arrange
            var mockFileProvider = new Mock<IFileProvider>();
            var mockFileInfo = new Mock<IFileInfo>();
            var mockStream = new MemoryStream();
            var fileName = "guide.pdf";
            var language = "en-US";

            _mockContextService.Setup(x => x.Language).Returns(language);
            mockFileInfo.Setup(x => x.Exists).Returns(true);
            mockFileInfo.Setup(x => x.CreateReadStream()).Returns(mockStream);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(mockFileInfo.Object);

            // Act
            var result = _controller.OddsGroupPdf(mockFileProvider.Object, fileName);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            mockFileProvider.Verify(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"), Times.Once);
        }

        [Fact]
        public void OddsGroupPdf_WithDifferentFileName_UsesCorrectFileName()
        {
            // Arrange
            var mockFileProvider = new Mock<IFileProvider>();
            var mockFileInfo = new Mock<IFileInfo>();
            var mockStream = new MemoryStream();
            var fileName = "odds-group-manual.pdf";
            var language = "ja-JP";

            _mockContextService.Setup(x => x.Language).Returns(language);
            mockFileInfo.Setup(x => x.Exists).Returns(true);
            mockFileInfo.Setup(x => x.CreateReadStream()).Returns(mockStream);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(mockFileInfo.Object);

            // Act
            var result = _controller.OddsGroupPdf(mockFileProvider.Object, fileName);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            mockFileProvider.Verify(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"), Times.Once);
        }

        [Fact]
        public void OddsGroupPdf_WithNullLanguage_UsesNullInPath()
        {
            // Arrange
            var mockFileProvider = new Mock<IFileProvider>();
            var mockUserLanguageFileInfo = new Mock<IFileInfo>();
            var mockDefaultLanguageFileInfo = new Mock<IFileInfo>();
            var mockStream = new MemoryStream();
            var fileName = "test.pdf";
            var defaultLanguage = "en-US";

            _mockContextService.Setup(x => x.Language).Returns((string)null);

            // Null language file doesn't exist
            mockUserLanguageFileInfo.Setup(x => x.Exists).Returns(false);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{(string)null}/{fileName}"))
                .Returns(mockUserLanguageFileInfo.Object);

            // Default language file exists
            mockDefaultLanguageFileInfo.Setup(x => x.Exists).Returns(true);
            mockDefaultLanguageFileInfo.Setup(x => x.CreateReadStream()).Returns(mockStream);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{defaultLanguage}/{fileName}"))
                .Returns(mockDefaultLanguageFileInfo.Object);

            // Act
            var result = _controller.OddsGroupPdf(mockFileProvider.Object, fileName);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
        }

        [Fact]
        public void OddsGroupPdf_WithEmptyFileName_UsesEmptyFileName()
        {
            // Arrange
            var mockFileProvider = new Mock<IFileProvider>();
            var mockFileInfo = new Mock<IFileInfo>();
            var mockStream = new MemoryStream();
            var fileName = "";
            var language = "en-US";

            _mockContextService.Setup(x => x.Language).Returns(language);
            mockFileInfo.Setup(x => x.Exists).Returns(true);
            mockFileInfo.Setup(x => x.CreateReadStream()).Returns(mockStream);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(mockFileInfo.Object);

            // Act
            var result = _controller.OddsGroupPdf(mockFileProvider.Object, fileName);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
        }

        [Fact]
        public void OddsGroupPdf_WithSpecialCharactersInFileName_HandlesCorrectly()
        {
            // Arrange
            var mockFileProvider = new Mock<IFileProvider>();
            var mockFileInfo = new Mock<IFileInfo>();
            var mockStream = new MemoryStream();
            var fileName = "odds-group_guide (v2).pdf";
            var language = "ko-KR";

            _mockContextService.Setup(x => x.Language).Returns(language);
            mockFileInfo.Setup(x => x.Exists).Returns(true);
            mockFileInfo.Setup(x => x.CreateReadStream()).Returns(mockStream);
            mockFileProvider.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(mockFileInfo.Object);

            // Act
            var result = _controller.OddsGroupPdf(mockFileProvider.Object, fileName);

            // Assert
            Assert.NotNull(result);
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
        }
    }
}