using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.Controllers;
using Fanex.Athena.PlayerAdmin.MVC.Excel;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Moq;
using NPOI.HSSF.UserModel;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    public class PlayerDetailTransactionControllerTests
    {
        private readonly Mock<IPlayerDetailTransactionService> _mockPlayerDetailTransactionService;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IFileProvider> _mockFileProvider;
        private readonly Mock<IFileInfo> _mockFileInfo;
        private readonly Mock<IPlayerDetailTransactionExcel> _mockExcelService;
        private readonly PlayerDetailTransactionController _controller;

        public PlayerDetailTransactionControllerTests()
        {
            _mockPlayerDetailTransactionService = new Mock<IPlayerDetailTransactionService>();
            _mockContextService = new Mock<IContextService>();
            _mockFileProvider = new Mock<IFileProvider>();
            _mockFileInfo = new Mock<IFileInfo>();
            _mockExcelService = new Mock<IPlayerDetailTransactionExcel>();
            _controller = new PlayerDetailTransactionController(_mockPlayerDetailTransactionService.Object, _mockContextService.Object);
        }

        [Fact]
        public void Index_WithValidParameters_ReturnsViewResult()
        {
            // Arrange
            var custId = 123;
            var currencyId = 1;

            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(false);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<PlayerDetailTransactionReportViewModel>(viewResult.Model);
            Assert.Equal(custId, viewModel.CustId);
            Assert.Equal(currencyId, viewModel.CurrencyId);
            Assert.True(viewModel.IsEnableSingleWallet);
            Assert.False(viewModel.IsShowSabaCoins);
        }

        [Fact]
        public async Task GetDetailTransactionReport_WithValidCriteria_ReturnsOkResult()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel { CustId = 123 };
            var expectedData = new PlayerDetailTransactionReportViewModel { CustId = 123 };

            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockPlayerDetailTransactionService.Setup(x => x.GetDetailTransactionReport(criteria))
                .ReturnsAsync(expectedData);

            // Act
            var result = await _controller.GetDetailTransactionReport(criteria);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedData = Assert.IsType<PlayerDetailTransactionReportViewModel>(okResult.Value);
            Assert.Equal(expectedData.CustId, returnedData.CustId);
            Assert.True(criteria.IsEnableSingleWallet);
        }

        [Fact]
        public async Task ExportDetailTransactionReport_WithValidCriteria_ReturnsFileResult()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel { CustId = 123 };
            var data = new PlayerDetailTransactionReportViewModel { CustId = 123 };
            var templatePath = "C:\\Templates\\DetailTransaction.xls";
            var workbook = new HSSFWorkbook();

            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockPlayerDetailTransactionService.Setup(x => x.GetDetailTransactionReport(criteria))
                .ReturnsAsync(data);
            _mockFileProvider.Setup(x => x.GetFileInfo("/App_Data/ExcelTemplate/DetailTransaction.xls"))
                .Returns(_mockFileInfo.Object);
            _mockFileInfo.Setup(x => x.PhysicalPath).Returns(templatePath);
            _mockExcelService.Setup(x => x.Workbook).Returns(workbook);

            // Act
            var result = await _controller.ExportDetailTransactionReport(_mockFileProvider.Object, _mockExcelService.Object, criteria);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/octet-stream", fileResult.ContentType);
            Assert.Equal("DetailTransaction.xls", fileResult.FileDownloadName);
            Assert.Null(criteria.PageSize);
            Assert.Null(criteria.PageIndex);
            Assert.True(criteria.IsEnableSingleWallet);
            Assert.True(data.IsEnableSingleWallet);
            _mockExcelService.Verify(x => x.SetData(templatePath, data), Times.Once);
            _mockExcelService.Verify(x => x.Export(), Times.Once);
        }

        [Fact]
        public async Task ExportDetailTransactionReport_SetsCorrectCriteriaValues()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel 
            { 
                CustId = 123, 
                PageSize = 10, 
                PageIndex = 1 
            };
            var data = new PlayerDetailTransactionReportViewModel { CustId = 123 };
            var templatePath = "C:\\Templates\\DetailTransaction.xls";
            var workbook = new HSSFWorkbook();

            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(false);
            _mockPlayerDetailTransactionService.Setup(x => x.GetDetailTransactionReport(criteria))
                .ReturnsAsync(data);
            _mockFileProvider.Setup(x => x.GetFileInfo("/App_Data/ExcelTemplate/DetailTransaction.xls"))
                .Returns(_mockFileInfo.Object);
            _mockFileInfo.Setup(x => x.PhysicalPath).Returns(templatePath);
            _mockExcelService.Setup(x => x.Workbook).Returns(workbook);

            // Act
            await _controller.ExportDetailTransactionReport(_mockFileProvider.Object, _mockExcelService.Object, criteria);

            // Assert
            Assert.Null(criteria.PageSize);
            Assert.Null(criteria.PageIndex);
            Assert.False(criteria.IsEnableSingleWallet);
            Assert.False(data.IsEnableSingleWallet);
        }

        [Fact]
        public async Task ExportDetailTransactionReport_WithNullFileInfo_ThrowsException()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel { CustId = 123 };
            var data = new PlayerDetailTransactionReportViewModel { CustId = 123 };

            _mockContextService.Setup(x => x.IsEnableSingleWallet).Returns(true);
            _mockPlayerDetailTransactionService.Setup(x => x.GetDetailTransactionReport(criteria))
                .ReturnsAsync(data);
            _mockFileProvider.Setup(x => x.GetFileInfo("/App_Data/ExcelTemplate/DetailTransaction.xls"))
                .Returns((IFileInfo)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => 
                _controller.ExportDetailTransactionReport(_mockFileProvider.Object, _mockExcelService.Object, criteria));
        }
    }
} 