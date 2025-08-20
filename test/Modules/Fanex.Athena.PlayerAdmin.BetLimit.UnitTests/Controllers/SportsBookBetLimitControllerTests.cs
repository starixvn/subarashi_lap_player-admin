namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    using AutoFixture;
    using BetLimit.Controllers;
    using BetLimit.DTO;
    using BetLimit.Services.Interfaces;
    using Core.Constants;
    using Core.Models;
    using Core.Resources;
    using Core.Services;
    using Core.Settings;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Moq;
    using ViewModels;

    public class SportsBookBetLimitControllerTests
    {
        private readonly IOptions<AppSetting> _appSetting;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly SportsBookBetLimitController _controller;
        private readonly IFixture _fixture;
        private readonly Mock<IResourceService> _resourceServiceMock;
        private readonly Mock<ISportsBookBetLimitService> _serviceMock;

        public SportsBookBetLimitControllerTests()
        {
            _fixture = new Fixture();
            _contextServiceMock = new Mock<IContextService>();
            _resourceServiceMock = new Mock<IResourceService>();
            _serviceMock = new Mock<ISportsBookBetLimitService>();
            _appSetting = Options.Create(new AppSetting { IsShowMixedSport = true });

            _controller = new SportsBookBetLimitController(_contextServiceMock.Object, _resourceServiceMock.Object, _serviceMock.Object, _appSetting);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _serviceMock.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(betLimitData);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult?.Value as PlayerBetLimitViewModel;
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.Data);
            Assert.True(viewModel.Editable);
        }

        [Fact]
        public async Task GetSetting_ShouldSetCopyIdAndEnableCopy_WhenSoccerDefaultBetTypeExists()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var copySportId = $"{SportType.Soccer}_{BetType.DefaultBetType}";
            var betLimitData = new List<PlayerBetLimit>
            {
                new() { SportType = SportType.Soccer.ToString(), BetType = BetType.DefaultBetType }
            };

            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _serviceMock.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(betLimitData);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult?.Value as PlayerBetLimitViewModel;
            Assert.NotNull(viewModel);
            Assert.Equal(copySportId, viewModel.CopyId);
            Assert.True(viewModel.EnableCopy);
        }

        [Fact]
        public async Task GetSetting_ShouldSetEnableCopyFalse_WhenSoccerDefaultBetTypeNotExists()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = new List<PlayerBetLimit>
            {
                new() { SportType = SportType.BasketBall.ToString(), BetType = BetType.DefaultBetType }
            };

            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _serviceMock.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(betLimitData);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult?.Value as PlayerBetLimitViewModel;
            Assert.NotNull(viewModel);
            Assert.False(viewModel.EnableCopy);
        }

        [Fact]
        public async Task GetSetting_ShouldSetEditableFalse_WhenUserHasNoEditPermission()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(false);
            _serviceMock.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(betLimitData);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult?.Value as PlayerBetLimitViewModel;
            Assert.NotNull(viewModel);
            Assert.False(viewModel.Editable);
        }

        [Fact]
        public async Task UpdateSetting_ShouldReturnSuccessMessage_WhenUpdateSuccessful()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = 0 };

            _serviceMock.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>(), It.IsAny<bool>()))
                .ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult?.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(0, data.Code);
            Assert.Equal(ViewResources.UpdateSuccess, data.Message);
        }

        [Fact]
        public async Task UpdateSetting_ShouldReturnErrorMessage_WhenUpdateFails()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = 1, Message = "Update failed" };

            _serviceMock.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>(), It.IsAny<bool>()))
                .ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult?.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(1, data.Code);
            Assert.Equal("Update failed", data.Message);
        }

        [Fact]
        public async Task UpdateSetting_ShouldUseIsShowMixedSportFromAppSetting_WhenCalling()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = 0 };

            _serviceMock.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>(), true))
                .ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            _serviceMock.Verify(x => x.UpdateSettingAsync(metaModel, true), Times.Once);
        }

        [Fact]
        public async Task GetSetting_ShouldUseIsShowMixedSportFromAppSetting_WhenGettingData()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _serviceMock.Setup(x => x.GetSettingAsync(It.IsAny<int>(), true)).ReturnsAsync(betLimitData);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            _serviceMock.Verify(x => x.GetSettingAsync(metaModel.CustId, true), Times.Once);
        }
    }
}