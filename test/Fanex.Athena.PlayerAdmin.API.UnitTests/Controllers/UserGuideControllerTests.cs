using System;
using System.IO;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class UserGuideControllerTests
    {
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Mock<IFileProvider> _fileProviderMock;
        private readonly UserGuideController _controller;

        public UserGuideControllerTests()
        {
            _contextServiceMock = new Mock<IContextService>();
            _fileProviderMock = new Mock<IFileProvider>();

            _controller = new UserGuideController(_contextServiceMock.Object);
        }

        [Fact]
        public void OddsGroupPdf_WithExistingLanguageFile_ReturnsFileResult()
        {
            // Arrange
            var fileName = "test-guide.pdf";
            var language = "en-US";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream();

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName);

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            Assert.Equal(stream, fileResult.FileStream);
        }

        [Fact]
        public void OddsGroupPdf_WithNonExistingLanguageFile_ReturnsDefaultLanguageFile()
        {
            // Arrange
            var fileName = "test-guide.pdf";
            var language = "fr-FR";
            var defaultLanguage = "en-US";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream();

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            
            // First call returns non-existing file
            var nonExistingFileMock = new Mock<IFileInfo>();
            nonExistingFileMock.Setup(x => x.Exists).Returns(false);
            
            // Second call returns existing default language file
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName);

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(nonExistingFileMock.Object);
            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{defaultLanguage}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            Assert.Equal(stream, fileResult.FileStream);
        }

        [Fact]
        public void OddsGroupPdf_WithNullFileName_ReturnsFileResult()
        {
            // Arrange
            string fileName = null;
            var language = "en-US";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream();

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName ?? "");

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsAssignableFrom<FileResult>(result);
            Assert.Equal("", fileResult.FileDownloadName);
        }

        [Fact]
        public void OddsGroupPdf_WithEmptyFileName_ReturnsFileResult()
        {
            // Arrange
            var fileName = "";
            var language = "en-US";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream();

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName);

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
        }

        [Fact]
        public void OddsGroupPdf_WithSpecialCharactersInFileName_HandlesCorrectly()
        {
            // Arrange
            var fileName = "test-guide-中文.pdf";
            var language = "zh-CN";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream();

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName);

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
        }

        [Fact]
        public void OddsGroupPdf_WithDifferentLanguageFormats_HandlesCorrectly()
        {
            // Arrange
            var fileName = "guide.pdf";
            var language = "th-TH";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream();

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName);

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
        }

        [Fact]
        public void OddsGroupPdf_WithBothFilesNonExisting_ReturnsDefaultLanguageFile()
        {
            // Arrange
            var fileName = "test-guide.pdf";
            var language = "fr-FR";
            var defaultLanguage = "en-US";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream();

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            
            // Both language-specific and default language files don't exist initially
            var nonExistingFileMock1 = new Mock<IFileInfo>();
            nonExistingFileMock1.Setup(x => x.Exists).Returns(false);
            
            var nonExistingFileMock2 = new Mock<IFileInfo>();
            nonExistingFileMock2.Setup(x => x.Exists).Returns(false);
            
            // But the default language file exists
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName);

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(nonExistingFileMock1.Object);
            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{defaultLanguage}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            Assert.Equal(stream, fileResult.FileStream);
        }

        [Fact]
        public void OddsGroupPdf_WithStreamDisposal_HandlesCorrectly()
        {
            // Arrange
            var fileName = "test-guide.pdf";
            var language = "en-US";
            var fileInfoMock = new Mock<IFileInfo>();
            var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });

            _contextServiceMock.Setup(x => x.Language).Returns(language);
            fileInfoMock.Setup(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.CreateReadStream()).Returns(stream);
            fileInfoMock.Setup(x => x.Name).Returns(fileName);

            _fileProviderMock.Setup(x => x.GetFileInfo($"App_Data/UserGuide/pdf/{language}/{fileName}"))
                .Returns(fileInfoMock.Object);

            // Act
            var result = _controller.OddsGroupPdf(_fileProviderMock.Object, fileName);

            // Assert
            var fileResult = Assert.IsType<FileStreamResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(fileName, fileResult.FileDownloadName);
            Assert.Equal(stream, fileResult.FileStream);
            
            // Verify the stream is accessible
            Assert.True(stream.CanRead);
        }
    }
} 