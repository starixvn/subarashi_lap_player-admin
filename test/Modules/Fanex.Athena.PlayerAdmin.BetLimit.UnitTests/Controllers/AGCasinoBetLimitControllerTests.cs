namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    using AutoFixture;
    using BetLimit.Controllers;
    using BetLimit.DTO;
    using BetLimit.Services.Interfaces;
    using Core.Models;
    using Core.Resources;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    public class AGCasinoBetLimitControllerTests
    {
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly AGCasinoBetLimitController _controller;
        private readonly IFixture _fixture;
        private readonly Mock<IResourceService> _resourceServiceMock;
        private readonly Mock<IAGCasinoBetLimitService> _serviceMock;

        public AGCasinoBetLimitControllerTests()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _resourceServiceMock = new Mock<IResourceService>();
            _serviceMock = new Mock<IAGCasinoBetLimitService>();

            _controller = new AGCasinoBetLimitController(_contextServiceMock.Object, _resourceServiceMock.Object, _serviceMock.Object);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betSetting = _fixture.Create<AgCasinoBetSetting>();
            var groupBettings = _fixture.CreateMany<AgCasinoGame>().ToList();

            betSetting.CheckedGroups = new[] { "group1", "group2" };
            betSetting.AgGroup = "group3";

            _contextServiceMock.Setup(x => x.SiteName).Returns("testSite");
            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _serviceMock.Setup(x => x.GetGroupBettingsAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(groupBettings);
            _serviceMock.Setup(x => x.GetBetSettingAsync(It.IsAny<int>())).ReturnsAsync(betSetting);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            Assert.NotNull(jsonResult.Value);
        }

        [Fact]
        public async Task GetSetting_ShouldIncludeAgGroupInCheckedGroups_WhenNotPresent()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betSetting = _fixture.Create<AgCasinoBetSetting>();
            var groupBettings = _fixture.CreateMany<AgCasinoGame>().ToList();

            betSetting.CheckedGroups = new[] { "group1", "group2" };
            betSetting.AgGroup = "group3";

            _contextServiceMock.Setup(x => x.SiteName).Returns("testSite");
            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _serviceMock.Setup(x => x.GetGroupBettingsAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(groupBettings);
            _serviceMock.Setup(x => x.GetBetSettingAsync(It.IsAny<int>())).ReturnsAsync(betSetting);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            var jsonResult = result;
            var responseData = jsonResult.Value;
            Assert.NotNull(responseData);
        }

        [Fact]
        public async Task UpdateSetting_ShouldReturnSuccessMessage_WhenUpdateSuccessful()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            var betSetting = _fixture.Create<AgCasinoBetSetting>();
            var resultInfo = new ResultInfo { Code = 0 };

            betSetting.IsEnabled = true;

            _serviceMock.Setup(x => x.GetBetSettingAsync(It.IsAny<int>())).ReturnsAsync(betSetting);
            _serviceMock.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(0, data.Code);
            Assert.Equal(ViewResources.UpdateSuccess, data.Message);
        }

        [Fact]
        public async Task UpdateSetting_ShouldReturnFunctionDeniedMessage_WhenSettingNotEnabled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            var betSetting = _fixture.Create<AgCasinoBetSetting>();

            betSetting.IsEnabled = false;

            _serviceMock.Setup(x => x.GetBetSettingAsync(It.IsAny<int>())).ReturnsAsync(betSetting);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(1, data.Code);
            Assert.Equal(ViewResources.FunctionDeniedMsg, data.Message);
        }

        [Fact]
        public async Task UpdateStatus_ShouldReturnSuccessMessage_WhenUpdateSuccessful()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 0 };

            _serviceMock.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(0, data.Code);
            Assert.Equal(ViewResources.UpdateSuccess, data.Message);
        }

        [Fact]
        public async Task UpdateStatus_ShouldReturnErrorMessage_WhenUpdateFails()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 1, Message = "Error occurred" };

            _serviceMock.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(1, data.Code);
            Assert.Equal("Error occurred", data.Message);
        }
    }
}