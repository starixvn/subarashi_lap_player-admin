using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class UserGuideControllerTests
    {
        private const string DefaultLanguage = "en-US";

        private IContextService _contextService;
        private IFileProvider _fileProvider;

        [SetUp]
        public void SetUp()
        {
            _contextService = Substitute.For<IContextService>();
            _fileProvider = Substitute.For<IFileProvider>();
        }

        [Test]
        public void OddsGroupPdf_WhenFileExist_ReturnCorrectFile()
        {
            //Arrange
            var fileInfo = Substitute.For<IFileInfo>();
            var stream = Substitute.For<Stream>();
            fileInfo.CreateReadStream().Returns(stream);
            var defaultFileInfo = Substitute.For<IFileInfo>();
            _fileProvider.GetFileInfo(default).ReturnsForAnyArgs(fileInfo, defaultFileInfo);
            fileInfo.Exists.Returns(true);

            var controller = CreateController();

            //Act
            var result = controller.OddsGroupPdf(_fileProvider, "test") as FileStreamResult;

            //Assert
            Assert.AreSame(stream, result.FileStream);
        }

        [Test]
        public void OddsGroupPdf_WhenFileNotExist_ReturnCorrectFile()
        {
            //Arrange
            var fileInfo = Substitute.For<IFileInfo>();
            var stream = Substitute.For<Stream>();
            fileInfo.CreateReadStream().Returns(stream);
            var defaultFileInfo = Substitute.For<IFileInfo>();
            var defaultFileStream = Substitute.For<Stream>();
            defaultFileInfo.CreateReadStream().Returns(defaultFileStream);
            _fileProvider.GetFileInfo(default).ReturnsForAnyArgs(fileInfo, defaultFileInfo);
            fileInfo.Exists.Returns(false);

            var controller = CreateController();

            //Act
            var result = controller.OddsGroupPdf(_fileProvider, "test") as FileStreamResult;

            //Assert
            Assert.AreSame(defaultFileStream, result.FileStream);
        }

        private UserGuideController CreateController()
            => new UserGuideController(_contextService);
    }
}
