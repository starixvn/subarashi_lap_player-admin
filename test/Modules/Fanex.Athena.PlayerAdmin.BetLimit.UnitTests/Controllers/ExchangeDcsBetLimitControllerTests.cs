namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    using AutoFixture;
    using BetLimit.Controllers;
    using BetLimit.Services.Interfaces;
    using Core.Models;
    using Core.Resources;
    using Core.Services;
    using MetaModel;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using ViewModels;

    public class ExchangeDcsBetLimitControllerTests
    {
        private readonly Mock<IExchangeChargeCommService> _chargeCommService;
        private readonly Mock<IContextService> _contextService;
        private readonly ExchangeDcsBetLimitController _controller;
        private readonly Mock<IDcsBetLimitService> _dcsService;
        private readonly IFixture _fixture;
        private readonly Mock<IResourceService> _resourceService;

        public ExchangeDcsBetLimitControllerTests()
        {
            _fixture = new Fixture();
            _contextService = new Mock<IContextService>();
            _resourceService = new Mock<IResourceService>();
            _dcsService = new Mock<IDcsBetLimitService>();
            _chargeCommService = new Mock<IExchangeChargeCommService>();

            _controller = new ExchangeDcsBetLimitController(
                _contextService.Object,
                _resourceService.Object,
                _dcsService.Object,
                _chargeCommService.Object);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            metaModel.SiteName = "TestSite";

            var commissionResult = new ResultInfo<double?> { Code = 0, Message = string.Empty, SpecificExtendedData = 5.0 };
            var status = true;

            _contextService.Setup(x => x.Language).Returns("en");
            _contextService.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _chargeCommService.Setup(x => x.GetChargeCommission(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(commissionResult);
            _dcsService.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult.Value as PlayerChargeCommViewModel;
            Assert.NotNull(viewModel);
            Assert.Equal(commissionResult.SpecificExtendedData, viewModel.Commission);
            Assert.True(viewModel.Editable);
            Assert.Equal(status, viewModel.StatusEnable);
        }

        [Fact]
        public async Task GetSetting_ShouldReturnNullCommission_WhenCommissionRequestFails()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            metaModel.SiteName = "TestSite";

            var commissionResult = new ResultInfo<double?> { Code = 1, Message = "Failed" };
            var status = true;

            _contextService.Setup(x => x.Language).Returns("en");
            _contextService.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);
            _chargeCommService.Setup(x => x.GetChargeCommission(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(commissionResult);
            _dcsService.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var viewModel = jsonResult.Value as PlayerChargeCommViewModel;
            Assert.NotNull(viewModel);
            Assert.Null(viewModel.Commission);
            Assert.True(viewModel.Editable);
            Assert.Equal(status, viewModel.StatusEnable);
        }

        [Theory]
        [InlineData(0, "")]
        [InlineData(2, "Failed")]
        public async Task UpdateStatus_ShouldReturnCorrectResult_WhenUpdateStatus(int code, string message)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = code, Message = message };

            _dcsService.Setup(x => x.UpdateStatusAsync(It.IsAny<PlayerBetLimitUpdateStatusMetaModel>())).ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(resultInfo.Code, data.Code);
            Assert.Equal(resultInfo.Message, data.Message);
        }

        [Theory]
        [InlineData(2.0)]
        [InlineData(3.5)]
        [InlineData(5.0)]
        [InlineData(7.0)]
        public async Task UpdateChargeComm_ShouldReturnSuccessResult_WhenValidCommissionProvided(double commission)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = commission;

            var statusResult = true;
            var updateResult = new ResultInfo { Code = 0, Message = string.Empty };

            _dcsService.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(statusResult);
            _chargeCommService.Setup(x => x.UpdateChargeCommission(It.IsAny<PlayerUpdateChargeCommMetaModel>())).ReturnsAsync(updateResult);

            // Act
            var result = await _controller.UpdateChargeComm(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(updateResult.Code, data.Code);
            Assert.Equal(updateResult.Message, data.Message);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(8.0)]
        [InlineData(10.0)]
        public async Task UpdateChargeComm_ShouldReturnInvalidCommissionError_WhenInvalidCommissionProvided(double commission)
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = commission;

            // Act
            var result = await _controller.UpdateChargeComm(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(100, data.Code);
            Assert.Equal(ViewResources.InvalidChargeCommission, data.Message);
        }

        [Fact]
        public async Task UpdateChargeComm_ShouldReturnFunctionDeniedMessage_WhenExchangeStatusDisabled()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerUpdateChargeCommMetaModel>();
            metaModel.Comm = 5.0; // Valid commission

            var statusResult = false;

            _dcsService.Setup(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(statusResult);

            // Act
            var result = await _controller.UpdateChargeComm(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result;
            var data = jsonResult.Value as ResultInfo;
            Assert.NotNull(data);
            Assert.Equal(1, data.Code);
            Assert.Equal(ViewResources.FunctionDeniedMsg, data.Message);
        }
    }
}