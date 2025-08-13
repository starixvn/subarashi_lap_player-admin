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
    using ViewModels;

    public class DcsBetLimitControllerTests
    {
        private readonly Mock<IContextService> _contextService;
        private readonly DcsBetLimitController _controller;
        private readonly IFixture _fixture;
        private readonly Mock<IResourceService> _resourceService;
        private readonly Mock<IDcsBetLimitService> _service;

        public DcsBetLimitControllerTests()
        {
            _fixture = new Fixture();
            _contextService = new Mock<IContextService>();
            _resourceService = new Mock<IResourceService>();
            _service = new Mock<IDcsBetLimitService>();


            _controller = new DcsBetLimitController(_contextService.Object, _resourceService.Object, _service.Object);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>();
            var status = true;


            _contextService.Setup(x => x.IsInRoles(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(betLimitData);
            _service.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            Assert.Equal(betLimitData, viewModel.Data);
            Assert.Equal(status, viewModel.IsStatusEnable);
            Assert.True(viewModel.Editable);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnEditableFalse_WhenStatusIsDisabled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>();
            var status = false;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(betLimitData);
            _service.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            Assert.Equal(betLimitData, viewModel.Data);
            Assert.True(viewModel.IsStatusEnable); // status.HasValue && isEditable = true && true = true
            Assert.False(viewModel.Editable); // isEditable && (!status.HasValue || status.Value) = true && (false || false) = false
        }

        [Fact]
        public async Task GetSetting_ShouldReturnEditableFalse_WhenUserHasNoEditPermission()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>().ToList();
            var status = true;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(betLimitData);
            _service.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            Assert.Equal(betLimitData, viewModel.Data);
            Assert.False(viewModel.IsStatusEnable); // status.HasValue && isEditable = true && false = false
            Assert.False(viewModel.Editable); // isEditable && (!status.HasValue || status.Value) = false && (false || true) = false
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(1, "Update failed")]
        public async Task UpdateSetting_ShouldReturnCorrectResult_WhenUpdateSetting(int code, string message)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = code, Message = message };


            _service.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.Equal(resultInfo.Code, data.Code);
            Assert.Equal(resultInfo.Message, data.Message);
        }

        [Fact]
        public async Task UpdateSetting_ShouldSetSuccessMessage_WhenCodeIsZero()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = 0, Message = "Original message" };

            _service.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.Equal(0, data.Code);
            Assert.Equal(ViewResources.UpdateSuccess, data.Message);
        }

        [Fact]
        public async Task UpdateSetting_ShouldKeepOriginalMessage_WhenCodeIsNotZero()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var originalMessage = "Error occurred";
            var resultInfo = new ResultInfo { Code = 1, Message = originalMessage };

            _service.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.Equal(1, data.Code);
            Assert.Equal(originalMessage, data.Message);
        }

        [Fact]
        public async Task UpdateStatus_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 1, Message = "Status update failed" };

            _service.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.Equal(resultInfo.Code, data.Code);
            Assert.Equal(resultInfo.Message, data.Message);
        }

        [Fact]
        public async Task UpdateStatus_ShouldSetSuccessMessage_WhenCodeIsZero()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 0, Message = "Original message" };

            _service.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.Equal(0, data.Code);
            Assert.Equal(ViewResources.UpdateSuccess, data.Message);
        }

        [Fact]
        public async Task UpdateStatus_ShouldKeepOriginalMessage_WhenCodeIsNotZero()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var originalMessage = "Status update error";
            var resultInfo = new ResultInfo { Code = 2, Message = originalMessage };

            _service.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.Equal(2, data.Code);
            Assert.Equal(originalMessage, data.Message);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnCorrectViewModel_WhenStatusIsNull()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>();
            bool? status = null;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(betLimitData);
            _service.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            Assert.Equal(betLimitData, viewModel.Data);
            Assert.False(viewModel.IsStatusEnable); // status.HasValue && isEditable = false && true = false
            Assert.True(viewModel.Editable); // isEditable && (!status.HasValue || status.Value) = true && (true || false) = true
            Assert.False(viewModel.StatusEnable); // status.GetValueOrDefault() = false when null
        }

        [Fact]
        public async Task GetSetting_ShouldReturnCorrectViewModel_WhenStatusIsNullAndUserHasNoEditPermission()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>();
            bool? status = null;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(betLimitData);
            _service.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            Assert.Equal(betLimitData, viewModel.Data);
            Assert.False(viewModel.IsStatusEnable); // status.HasValue && isEditable = false && false = false
            Assert.False(viewModel.Editable); // isEditable && (!status.HasValue || status.Value) = false && (true || false) = false
            Assert.False(viewModel.StatusEnable); // status.GetValueOrDefault() = false when null
        }

        [Fact]
        public void Constructor_ShouldInitializeAllDependencies_WhenCalled()
        {
            // Arrange
            var contextService = new Mock<IContextService>();
            var resourceService = new Mock<IResourceService>();
            var dcsBetLimitService = new Mock<IDcsBetLimitService>();

            // Act
            var controller = new DcsBetLimitController(contextService.Object, resourceService.Object, dcsBetLimitService.Object);

            // Assert
            Assert.NotNull(controller);
            Assert.IsType<DcsBetLimitController>(controller);
        }

        [Fact]
        public async Task GetSetting_ShouldCallServiceMethods_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimitData = _fixture.CreateMany<PlayerBetLimit>();
            var status = true;

            _contextService.Setup(x => x.IsInRoles(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _service.Setup(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(betLimitData);
            _service.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            _service.Verify(x => x.GetSettingAsync(metaModel.CustId, metaModel.ProductId), Times.Once);
            _service.Verify(x => x.GetStatusAsync(metaModel.CustId, metaModel.ProductId), Times.Once);
        }

        [Fact]
        public async Task UpdateSetting_ShouldCallServiceMethod_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = 0, Message = "Success" };

            _service.Setup(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            _service.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
        }

        [Fact]
        public async Task UpdateStatus_ShouldCallServiceMethod_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 0, Message = "Success" };

            _service.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            _service.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
        }
    }
}