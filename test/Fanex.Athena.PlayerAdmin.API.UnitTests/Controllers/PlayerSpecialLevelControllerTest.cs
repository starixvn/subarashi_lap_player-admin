namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    using API.Controllers;
    using API.MetaModel;
    using API.Services.Interfaces;
    using Core.Models;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class PlayerSpecialLevelControllerTest
    {
        private readonly PlayerSpecialLevelController _controller;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<ILogger<PlayerSpecialLevelController>> _mockLogger;
        private readonly Mock<IPlayerSpecialLevelService> _mockService;

        public PlayerSpecialLevelControllerTest()
        {
            _mockService = new Mock<IPlayerSpecialLevelService>();
            _mockContextService = new Mock<IContextService>();
            _mockLogger = new Mock<ILogger<PlayerSpecialLevelController>>();
            _controller = new PlayerSpecialLevelController(_mockService.Object, _mockContextService.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidServices_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public async Task Update_WithValidMetaModelAndNotFakeLicensee_ReturnsOkWithSuccessResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 5,
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Update successful",
                ErrorCode = 0
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockService.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.True(actualResult.Success);
            Assert.Equal(expectedResult.Message, actualResult.Message);
            _mockService.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithFakeLicensee_ReturnsOkWithFailureResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 5,
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(true);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("FunctionDeniedMsg", actualResult.ErrorMessage);
            _mockService.Verify(x => x.Update(It.IsAny<SpecialLevelUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithServiceThrowingException_ReturnsOkWithErrorResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 5,
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockService.Setup(x => x.Update(metaModel)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("FunctionDeniedMsg", actualResult.ErrorMessage);

            // Verify that the exception was logged

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Update_WithServiceReturningFailure_ReturnsOkWithFailureResult()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 5,
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            var expectedResult = new ResultViewModel
            {
                Success = false,
                Message = "Update failed",
                ErrorCode = 1
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockService.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("Update failed", actualResult.Message);
            Assert.Equal(1, actualResult.ErrorCode);
            _mockService.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithDifferentSpecialLevel_CallsServiceWithCorrectParameters()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 456,
                SpecialLevel = 10,
                UserId = 2,
                Ip = "10.0.0.1",
                Language = "zh",
                SiteName = "AnotherSite"
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Update successful"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockService.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.True(actualResult.Success);


            _mockService.Verify(x => x.Update(It.Is<SpecialLevelUpdatingMetaModel>(m => m.CustId == 456 &&
                                                                                        m.SpecialLevel == 10 &&
                                                                                        m.UserId == 2 &&
                                                                                        m.Ip == "10.0.0.1" &&
                                                                                        m.Language == "zh" &&
                                                                                        m.SiteName == "AnotherSite")), Times.Once);
        }

        [Fact]
        public async Task Update_WithZeroSpecialLevel_StillCallsService()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 123,
                SpecialLevel = 0,
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Update successful"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockService.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.True(actualResult.Success);
            _mockService.Verify(x => x.Update(metaModel), Times.Once);
        }

        [Fact]
        public async Task Update_WithZeroCustId_StillCallsService()
        {
            // Arrange
            var metaModel = new SpecialLevelUpdatingMetaModel
            {
                CustId = 0,
                SpecialLevel = 5,
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            var expectedResult = new ResultViewModel
            {
                Success = true,
                Message = "Update successful"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockService.Setup(x => x.Update(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            _mockService.Verify(x => x.Update(metaModel), Times.Once);
        }
    }
}