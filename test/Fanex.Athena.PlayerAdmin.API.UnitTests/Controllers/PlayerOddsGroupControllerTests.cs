using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerOddsGroupControllerTests
    {
        private readonly Mock<IPlayerOddsGroupService> _playerOddsGroupServiceMock;
        private readonly Mock<ILicenseeService> _licenseeServiceMock;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Mock<ILogger<PlayerOddsGroupController>> _loggerMock;
        private readonly PlayerOddsGroupController _controller;

        public PlayerOddsGroupControllerTests()
        {
            _playerOddsGroupServiceMock = new Mock<IPlayerOddsGroupService>();
            _licenseeServiceMock = new Mock<ILicenseeService>();
            _contextServiceMock = new Mock<IContextService>();
            _loggerMock = new Mock<ILogger<PlayerOddsGroupController>>();

            _controller = new PlayerOddsGroupController(
                _playerOddsGroupServiceMock.Object,
                _licenseeServiceMock.Object,
                _contextServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Update_WithValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                UserId = 456,
                OddsGroup = "A",
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "test-site",
                LicenseeCustId = 789
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Update successful",
                ErrorCode = 0
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(new[] { "A", "B", "C" });
            _playerOddsGroupServiceMock.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.True(returnedResult.Success);
            Assert.Equal("Update successful", returnedResult.Message);
            Assert.Equal(0, returnedResult.ErrorCode);
            
            _playerOddsGroupServiceMock.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithFakeLicensee_ReturnsFunctionDenied()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A"
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(true);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("FunctionDeniedMsg", returnedResult.ErrorMessage);
            
            _playerOddsGroupServiceMock.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithInvalidOddsGroup_ReturnsFunctionDenied()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "INVALID"
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(new[] { "A", "B", "C" });

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("FunctionDeniedMsg", returnedResult.ErrorMessage);
            
            _playerOddsGroupServiceMock.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithValidOddsGroupCaseInsensitive_ReturnsSuccessResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "a" // lowercase
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Update successful",
                ErrorCode = 0
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(new[] { "A", "B", "C" });
            _playerOddsGroupServiceMock.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.True(returnedResult.Success);
            
            _playerOddsGroupServiceMock.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithServiceException_ReturnsHttpErrDefault()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A"
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(new[] { "A", "B", "C" });
            _playerOddsGroupServiceMock.Setup(x => x.Update(metaModel)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("HttpError_Default", returnedResult.ErrorMessage);
            
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Update_WithEmptyOddsGroupList_ReturnsFunctionDenied()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A"
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(Enumerable.Empty<string>());

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("FunctionDeniedMsg", returnedResult.ErrorMessage);
            
            _playerOddsGroupServiceMock.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithNullOddsGroup_ReturnsFunctionDenied()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = null
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(new[] { "A", "B", "C" });

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("FunctionDeniedMsg", returnedResult.ErrorMessage);
            
            _playerOddsGroupServiceMock.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithServiceReturningFailure_ReturnsServiceResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A"
            };

            var expectedResult = new ResultViewModel
            {
                Success = false,
                Message = "Update failed",
                ErrorCode = 1
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(new[] { "A", "B", "C" });
            _playerOddsGroupServiceMock.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("Update failed", returnedResult.Message);
            Assert.Equal(1, returnedResult.ErrorCode);
            
            _playerOddsGroupServiceMock.Verify(x => x.Update(metaModel), Times.Once);
        }
    }
} 