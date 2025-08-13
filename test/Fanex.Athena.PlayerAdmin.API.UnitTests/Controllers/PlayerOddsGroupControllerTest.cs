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

    public class PlayerOddsGroupControllerTest
    {
        private readonly PlayerOddsGroupController _controller;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<ILogger<PlayerOddsGroupController>> _mockLogger;
        private readonly Mock<IPlayerOddsGroupService> _mockService;

        public PlayerOddsGroupControllerTest()
        {
            _mockService = new Mock<IPlayerOddsGroupService>();
            _mockContextService = new Mock<IContextService>();
            _mockLogger = new Mock<ILogger<PlayerOddsGroupController>>();
            _controller = new PlayerOddsGroupController(_mockService.Object, _mockContextService.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_WithValidServices_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public async Task Update_WithValidOddsGroupAndNotFakeLicensee_ReturnsOkWithSuccessResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A",
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
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string> { "A", "B", "C" });
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
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A",
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(true);
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string> { "A", "B", "C" });

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("FunctionDeniedMsg", actualResult.ErrorMessage);
            _mockService.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithInvalidOddsGroup_ReturnsOkWithFailureResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "Z", // Invalid odds group
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string> { "A", "B", "C" });

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("FunctionDeniedMsg", actualResult.ErrorMessage);
            _mockService.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithServiceThrowingException_ReturnsOkWithErrorResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A",
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string> { "A", "B", "C" });
            _mockService.Setup(x => x.Update(metaModel)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("HttpError_Default", actualResult.ErrorMessage);

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
        public async Task Update_WithCaseInsensitiveOddsGroup_ReturnsOkWithSuccessResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "a", // Lowercase
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
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string> { "A", "B", "C" });
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
        public async Task Update_WithEmptyOddsGroupList_ReturnsOkWithFailureResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A",
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string>());

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("FunctionDeniedMsg", actualResult.ErrorMessage);
            _mockService.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithNullOddsGroup_ReturnsOkWithFailureResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = null,
                UserId = 1,
                Ip = "192.168.1.1",
                Language = "en",
                SiteName = "TestSite"
            };

            _mockContextService.Setup(x => x.IsFakeLicensee).Returns(false);
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string> { "A", "B", "C" });

            // Act
            var result = await _controller.Update(metaModel);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<ResultViewModel>(okResult.Value);
            Assert.False(actualResult.Success);
            Assert.Equal("FunctionDeniedMsg", actualResult.ErrorMessage);
            _mockService.Verify(x => x.Update(It.IsAny<OddsGroupUpdatingMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Update_WithServiceReturningFailure_ReturnsOkWithFailureResult()
        {
            // Arrange
            var metaModel = new OddsGroupUpdatingMetaModel
            {
                CustId = 123,
                OddsGroup = "A",
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
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string> { "A", "B", "C" });
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
    }
}