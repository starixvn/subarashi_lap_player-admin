namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    using AutoFixture;
    using BetLimit.Controllers;
    using BetLimit.DTO;
    using BetLimit.Services;
    using Core.Models;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using ViewModels;

    public class ThirdPartyBetLimitControllerTests
    {
        private readonly Mock<IContextService> _contextService;
        private readonly ThirdPartyBetLimitController _controller;
        private readonly IFixture _fixture;
        private readonly Mock<IResourceService> _resourceService;
        private readonly Mock<IThirdPartyBetLimitService> _service;

        public ThirdPartyBetLimitControllerTests()
        {
            _fixture = new Fixture();
            _contextService = new Mock<IContextService>();
            _resourceService = new Mock<IResourceService>();
            _service = new Mock<IThirdPartyBetLimitService>();

            _controller = new ThirdPartyBetLimitController(_contextService.Object, _resourceService.Object, _service.Object);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnPlayerBetLimitViewModel_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var thirdPartyBetSetting = _fixture.Create<ThirdPartyBetSetting>();
            thirdPartyBetSetting.IsEnabled = true;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(thirdPartyBetSetting);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<PlayerBetLimitViewModel>>(result);
            Assert.IsType<PlayerBetLimitViewModel>(result.Value);
        }

        [Fact]
        public async Task GetSetting_ShouldSetIsStatusEnableTrue_WhenUserHasEditPermission()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var thirdPartyBetSetting = _fixture.Create<ThirdPartyBetSetting>();
            thirdPartyBetSetting.IsEnabled = true;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(thirdPartyBetSetting);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.True(result.Value.IsStatusEnable);
        }

        [Fact]
        public async Task GetSetting_ShouldSetIsStatusEnableFalse_WhenUserHasNoEditPermission()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var thirdPartyBetSetting = _fixture.Create<ThirdPartyBetSetting>();
            thirdPartyBetSetting.IsEnabled = true;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(false);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(thirdPartyBetSetting);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.Value.IsStatusEnable);
        }

        [Fact]
        public async Task GetSetting_ShouldSetStatusEnableFromSetting_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var thirdPartyBetSetting = _fixture.Create<ThirdPartyBetSetting>();
            thirdPartyBetSetting.IsEnabled = false;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(false);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(thirdPartyBetSetting);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.False(result.Value.StatusEnable);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "Failed")]
        public async Task UpdateStatus_ShouldReturnCorrectResult_WhenUpdateStatus(int code, string message)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = code, Message = message };

            _service.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var data = result.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(resultInfo.Code, data.Code);
            Assert.Equal(resultInfo.Message, data.Message);
        }

        [Fact]
        public async Task GetSetting_ShouldCallServiceWithCorrectParameters_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var thirdPartyBetSetting = _fixture.Create<ThirdPartyBetSetting>();

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _service.Setup(x => x.GetSettingAsync(metaModel.CustId, metaModel.ProductId)).ReturnsAsync(thirdPartyBetSetting);

            // Act
            _ = await _controller.GetSetting(metaModel);

            // Assert
            _service.Verify(x => x.GetSettingAsync(metaModel.CustId, metaModel.ProductId), Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_ShouldCallServiceWithCorrectParameters_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 0 };

            _service.Setup(x => x.UpdateStatusAsync(metaModel)).ReturnsAsync(resultInfo);

            // Act
            _ = await _controller.UpdateStatus(metaModel);

            // Assert
            _service.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
        }
    }
}