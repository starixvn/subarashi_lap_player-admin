using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.PlayerAdmin.BetLimit.Controllers;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Controllers
{
    public class DcsBetLimitControllerTests
    {
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IResourceService> _mockResourceService;
        private readonly Mock<IDcsBetLimitService> _mockDcsBetLimitService;
        private readonly Fixture _fixture;
        private readonly DcsBetLimitController _controller;

        public DcsBetLimitControllerTests()
        {
            _mockContextService = new Mock<IContextService>();
            _mockResourceService = new Mock<IResourceService>();
            _mockDcsBetLimitService = new Mock<IDcsBetLimitService>();
            _fixture = new Fixture();
            
            _controller = new DcsBetLimitController(
                _mockContextService.Object,
                _mockResourceService.Object,
                _mockDcsBetLimitService.Object);
        }

        [Fact]
        public async Task GetSetting_WithValidModel_ReturnsJsonResult()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimits = _fixture.CreateMany<PlayerBetLimit>(3);
            var status = true;

            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BetLimit_Write))
                .Returns(true);
            _mockDcsBetLimitService.Setup(x => x.GetSettingAsync(metaModel.CustId, metaModel.ProductId))
                .ReturnsAsync(betLimits);
            _mockDcsBetLimitService.Setup(x => x.GetStatusAsync(metaModel.CustId, metaModel.ProductId))
                .ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            
            Assert.NotNull(viewModel);
            Assert.Equal(betLimits, viewModel.Data);
            Assert.True(viewModel.Editable);
            Assert.True(viewModel.IsStatusEnable);
            Assert.True(viewModel.StatusEnable);
        }

        [Fact]
        public async Task GetSetting_WithNoEditPermission_ReturnsNonEditableViewModel()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimits = _fixture.CreateMany<PlayerBetLimit>(3);
            var status = true;

            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BetLimit_Write))
                .Returns(false);
            _mockDcsBetLimitService.Setup(x => x.GetSettingAsync(metaModel.CustId, metaModel.ProductId))
                .ReturnsAsync(betLimits);
            _mockDcsBetLimitService.Setup(x => x.GetStatusAsync(metaModel.CustId, metaModel.ProductId))
                .ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            var jsonResult = result as JsonResult;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            
            Assert.False(viewModel.Editable);
            Assert.False(viewModel.IsStatusEnable);
        }

        [Fact]
        public async Task GetSetting_WithNullStatus_ReturnsCorrectViewModel()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitSelectMetaModel>();
            var betLimits = _fixture.CreateMany<PlayerBetLimit>(3);
            bool? status = null;

            _mockContextService.Setup(x => x.IsInRoles(Roles.SuperAdmin, Roles.BetLimit_Write))
                .Returns(true);
            _mockDcsBetLimitService.Setup(x => x.GetSettingAsync(metaModel.CustId, metaModel.ProductId))
                .ReturnsAsync(betLimits);
            _mockDcsBetLimitService.Setup(x => x.GetStatusAsync(metaModel.CustId, metaModel.ProductId))
                .ReturnsAsync(status);

            // Act
            var result = await _controller.GetSetting(metaModel);

            // Assert
            var jsonResult = result as JsonResult;
            var viewModel = jsonResult.Value as PlayerBetLimitViewModel;
            
            Assert.True(viewModel.Editable);
            Assert.False(viewModel.IsStatusEnable);
            Assert.False(viewModel.StatusEnable);
        }

        [Fact]
        public async Task UpdateSetting_WithValidModel_ReturnsJsonResult()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = 0, Message = "Success" };

            _mockDcsBetLimitService.Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            var returnedResult = jsonResult.Value as ResultInfo;
            
            Assert.Equal(0, returnedResult.Code);
            Assert.Equal(ViewResources.UpdateSuccess, returnedResult.Message);
        }

        [Fact]
        public async Task UpdateSetting_WithError_ReturnsOriginalMessage()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            var resultInfo = new ResultInfo { Code = 1, Message = "Error occurred" };

            _mockDcsBetLimitService.Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateSetting(metaModel);

            // Assert
            var jsonResult = result as JsonResult;
            var returnedResult = jsonResult.Value as ResultInfo;
            
            Assert.Equal(1, returnedResult.Code);
            Assert.Equal("Error occurred", returnedResult.Message);
        }

        [Fact]
        public async Task UpdateStatus_WithValidModel_ReturnsJsonResult()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 0, Message = "Success" };

            _mockDcsBetLimitService.Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            Assert.IsType<JsonResult>(result);
            var jsonResult = result as JsonResult;
            var returnedResult = jsonResult.Value as ResultInfo;
            
            Assert.Equal(0, returnedResult.Code);
            Assert.Equal(ViewResources.UpdateSuccess, returnedResult.Message);
        }

        [Fact]
        public async Task UpdateStatus_WithError_ReturnsOriginalMessage()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var resultInfo = new ResultInfo { Code = 1, Message = "Error occurred" };

            _mockDcsBetLimitService.Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(resultInfo);

            // Act
            var result = await _controller.UpdateStatus(metaModel);

            // Assert
            var jsonResult = result as JsonResult;
            var returnedResult = jsonResult.Value as ResultInfo;
            
            Assert.Equal(1, returnedResult.Code);
            Assert.Equal("Error occurred", returnedResult.Message);
        }
    }
} 