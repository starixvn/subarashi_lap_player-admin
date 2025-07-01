using System;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerSpecialLevelControllerTests
    {
        private readonly Mock<IPlayerSpecialLevelService> _playerSpecialLevelServiceMock;
        private readonly Mock<ILicenseeService> _licenseeServiceMock;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Mock<ILogger<PlayerSpecialLevelController>> _loggerMock;
        private readonly PlayerSpecialLevelController _controller;

        public PlayerSpecialLevelControllerTests()
        {
            _playerSpecialLevelServiceMock = new Mock<IPlayerSpecialLevelService>();
            _licenseeServiceMock = new Mock<ILicenseeService>();
            _contextServiceMock = new Mock<IContextService>();
            _loggerMock = new Mock<ILogger<PlayerSpecialLevelController>>();

            _controller = new PlayerSpecialLevelController(
                _playerSpecialLevelServiceMock.Object,
                _licenseeServiceMock.Object,
                _contextServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Update_WithValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                UserId = 456,
                SpecialLevel = 5,
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
            _playerSpecialLevelServiceMock.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.True(returnedResult.Success);
            Assert.Equal("Update successful", returnedResult.Message);
            Assert.Equal(0, returnedResult.ErrorCode);
            
            _playerSpecialLevelServiceMock.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithFakeLicensee_ReturnsFunctionDenied()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 5
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(true);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("FunctionDeniedMsg", returnedResult.ErrorMessage);
            
            _playerSpecialLevelServiceMock.Verify(x => x.Update(It.IsAny<SpecialLevelUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithServiceException_ReturnsFunctionDenied()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 5
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _playerSpecialLevelServiceMock.Setup(x => x.Update(metaModel)).ThrowsAsync(new Exception("Service error"));

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("FunctionDeniedMsg", returnedResult.ErrorMessage);
            
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
        public async Task Update_WithServiceReturningFailure_ReturnsServiceResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 5
            };

            var expectedResult = new ResultViewModel
            {
                Success = false,
                Message = "Update failed",
                ErrorCode = 1
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _playerSpecialLevelServiceMock.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.False(returnedResult.Success);
            Assert.Equal("Update failed", returnedResult.Message);
            Assert.Equal(1, returnedResult.ErrorCode);
            
            _playerSpecialLevelServiceMock.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithZeroSpecialLevel_ReturnsSuccessResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 0
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Special level removed",
                ErrorCode = 0
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _playerSpecialLevelServiceMock.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.True(returnedResult.Success);
            Assert.Equal("Special level removed", returnedResult.Message);
            
            _playerSpecialLevelServiceMock.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithMaximumSpecialLevel_ReturnsSuccessResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 10
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Update successful",
                ErrorCode = 0
            };

            _contextServiceMock.Setup(x => x.IsFakeLicensee).Returns(false);
            _playerSpecialLevelServiceMock.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResult = Assert.IsType<ResultViewModel>(okResult.Value);
            
            Assert.True(returnedResult.Success);
            
            _playerSpecialLevelServiceMock.Verify(x => x.Update(metaModel), Times.Once);
        }
    }
} 