namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Controllers
{
    using AutoFixture;
    using BatchUpdate.Controllers;
    using BatchUpdate.MetaModels;
    using BatchUpdate.Services;
    using BatchUpdate.ViewModels;
    using Core.Models;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    public class PlayerBatchUpdateControllerTests
    {
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Fixture _fixture;
        private readonly Mock<IPlayerBatchUpdateService> _serviceMock;
        private readonly PlayerBatchUpdateController _sut;

        public PlayerBatchUpdateControllerTests()
        {
            _fixture = new Fixture();
            _serviceMock = new Mock<IPlayerBatchUpdateService>();
            _contextServiceMock = new Mock<IContextService>();
            _sut = new PlayerBatchUpdateController(_serviceMock.Object);
        }

        [Fact]
        public void UpdateOddsGroup_ShouldReturnViewResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBatchUpdateMetaModel>();
            var oddsGroups = _fixture.Create<string[]>();
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(oddsGroups);

            // Act
            var result = _sut.UpdateOddsGroup(metaModel, _contextServiceMock.Object);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<BasePlayerBatchUpdateViewModel>(result.Model);
            Assert.Equal(oddsGroups, result.ViewData["OddsGroups"]);
        }

        [Fact]
        public async Task UpdateOddsGroup_ShouldReturnJsonWithError_WhenOddsGroupIdIsInvalid()
        {
            // Arrange
            var metaModel = _fixture.Create<UpdateOddsGroupApiMetaModel>();
            var oddsGroups = new[] { "group1", "group2" };
            metaModel.OddsGroupId = "invalidGroup";
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(oddsGroups);

            // Act
            var result = await _sut.UpdateOddsGroup(metaModel, _contextServiceMock.Object);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var resultInfo = jsonResult.Value as ResultInfo;
            Assert.Equal(1, resultInfo.Code);
        }

        [Fact]
        public async Task UpdateOddsGroup_ShouldCallServiceAndReturnResult_WhenOddsGroupIdIsValid()
        {
            // Arrange
            var metaModel = _fixture.Create<UpdateOddsGroupApiMetaModel>();
            var oddsGroups = new[] { metaModel.OddsGroupId };
            var expectedResult = _fixture.Create<ResultInfo>();
            _contextServiceMock.Setup(x => x.OfferedOddsGroupList).Returns(oddsGroups);
            _serviceMock.Setup(x => x.UpdateOddsGroup(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.UpdateOddsGroup(metaModel, _contextServiceMock.Object);

            // Assert
            _serviceMock.Verify(x => x.UpdateOddsGroup(metaModel), Times.Once);
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            Assert.Equal(expectedResult, jsonResult.Value);
        }

        [Fact]
        public void UpdateSpecialLevel_ShouldReturnViewResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBatchUpdateMetaModel>();

            // Act
            var result = _sut.UpdateSpecialLevel(metaModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<BasePlayerBatchUpdateViewModel>(result.Model);
        }

        [Fact]
        public async Task UpdateSpecialLevel_ShouldCallServiceAndReturnResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<UpdateSpecialLevelApiMetaModel>();
            var expectedResult = _fixture.Create<ResultInfo>();
            _serviceMock.Setup(x => x.UpdateSpecialLevel(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.UpdateSpecialLevel(metaModel);

            // Assert
            _serviceMock.Verify(x => x.UpdateSpecialLevel(metaModel), Times.Once);
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            Assert.Equal(expectedResult, jsonResult.Value);
        }
    }
}