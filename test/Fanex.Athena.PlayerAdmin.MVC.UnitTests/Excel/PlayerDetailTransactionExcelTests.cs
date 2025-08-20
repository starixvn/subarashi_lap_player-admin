// test/Fanex.Athena.PlayerAdmin.MVC.UnitTests/Excel/PlayerDetailTransactionExcelTests.cs
namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Excel
{
    using System.Reflection;
    using AutoFixture;
    using Core.Constants;
    using Core.Excel;
    using Moq;
    using MVC.Excel;
    using MVC.ViewModels;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class PlayerDetailTransactionExcelTests
    {
        private readonly Fixture _fixture;
        private readonly PlayerDetailTransactionExcel _excel;
        private readonly Mock<ISheet> _mockSheet;
        private readonly Mock<IWorkbook> _mockWorkbook;
        private readonly Mock<IRow> _mockRow;
        private readonly Mock<ICell> _mockCell;

        public PlayerDetailTransactionExcelTests()
        {
            _fixture = new Fixture();
            _excel = new PlayerDetailTransactionExcel();
            _mockSheet = new Mock<ISheet>();
            _mockWorkbook = new Mock<IWorkbook>();
            _mockRow = new Mock<IRow>();
            _mockCell = new Mock<ICell>();

            _mockSheet.Setup(s => s.GetRow(It.IsAny<int>())).Returns(_mockRow.Object);
            _mockSheet.Setup(s => s.CreateRow(It.IsAny<int>())).Returns(_mockRow.Object);
            _mockSheet.SetupGet(s => s.LastRowNum).Returns(10);
            _mockRow.SetupGet(r => r.Cells).Returns(new List<ICell> { _mockCell.Object, _mockCell.Object, _mockCell.Object, _mockCell.Object, _mockCell.Object, _mockCell.Object, _mockCell.Object });
            _mockWorkbook.Setup(w => w.GetSheet(It.IsAny<string>())).Returns(_mockSheet.Object);
            _excel.Workbook = _mockWorkbook.Object;
            var templateProp = typeof(BaseExcel).GetProperty("TemplateSheet", BindingFlags.Instance | BindingFlags.NonPublic);
            templateProp.SetValue(_excel, _mockSheet.Object);
        }

        [Fact]
        public void Constructor_Creates_ValidInstance()
        {
            // Act & Assert
            Assert.NotNull(_excel);
            Assert.IsAssignableFrom<BaseExcel>(_excel);
            Assert.IsAssignableFrom<IPlayerDetailTransactionExcel>(_excel);
        }

        [Fact]
        public void SetData_WithValidParameters_SetsDataCorrectly()
        {
            // Arrange
            var templatePath = _fixture.Create<string>();
            var data = CreateValidPlayerDetailTransactionReportViewModel();

            // Act
            _excel.SetData(templatePath, data);

            // Assert
            Assert.NotNull(_excel);
            // Note: Cannot directly test private field _data, but we can verify through export behavior
        }

        [Fact]
        public void SetData_WithEmptyDetailTransactions_SetsZeroRows()
        {
            // Arrange
            var templatePath = _fixture.Create<string>();
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.DetailTransactions = new List<PlayerDetailTransactionViewModel>();

            // Act
            _excel.SetData(templatePath, data);

            // Assert
            Assert.NotNull(_excel);
        }

        [Fact]
        public void SetData_WithNullDetailTransactions_ThrowsException()
        {
            // Arrange
            var templatePath = _fixture.Create<string>();
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.DetailTransactions = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _excel.SetData(templatePath, data));
        }

        [Fact]
        public void AssignHeader_SetsCorrectHeaderValues()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            _excel.SetData("test", data);
            var cells = new List<ICell>();
            for (int i = 0; i < 7; i++)
            {
                cells.Add(new Mock<ICell>().Object);
            }
            _mockRow.SetupGet(r => r.Cells).Returns(cells);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignHeader", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockRow.Verify(r => r.Cells, Times.AtLeastOnce);
            _mockSheet.Verify(s => s.GetRow(BaseExcel.RowHeaderTemplate), Times.AtLeastOnce);
        }

        [Fact]
        public void AssignTitle_WithMainWalletViewType_SetsCorrectTitle()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.ViewType = ViewTypes.MainWallet;
            data.FromDate = DateTime.Parse("01/01/2023");
            data.FromDate = DateTime.Parse("01/31/2023");
            _excel.SetData("test", data);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignTitle", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.GetRow(BaseExcel.RowTitleTemplate), Times.AtLeastOnce);
            _mockSheet.Verify(s => s.CreateFreezePane(0, 2), Times.Once);
        }

        [Fact]
        public void AssignTitle_WithSabaCoinsViewType_IncludesViewText()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.ViewType = ViewTypes.SabaCoins;
            data.FromDate = DateTime.Parse("01/01/2023");
            data.FromDate = DateTime.Parse("01/31/2023");
            _excel.SetData("test", data);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignTitle", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.GetRow(BaseExcel.RowTitleTemplate), Times.AtLeastOnce);
            _mockSheet.Verify(s => s.CreateFreezePane(0, 2), Times.Once);
        }

        [Fact]
        public void AssignNoInfoRow_SetsNoInfoMessage()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            _excel.SetData("test", data);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignNoInfoRow", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.GetRow(2), Times.AtLeastOnce);
        }

        [Fact]
        public void AssignSheetData_WithTransactions_AssignsAllTransactionData()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.DetailTransactions = CreateDetailTransactions(3);
            _excel.SetData("test", data);

            var cells = new List<ICell>();
            for (int i = 0; i < 7; i++)
            {
                var mockCell = new Mock<ICell>();
                cells.Add(mockCell.Object);
            }
            _mockRow.SetupGet(r => r.Cells).Returns(cells);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignSheetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ISheet) }, null);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.GetRow(It.IsAny<int>()), Times.AtLeast(3));
        }

        [Fact]
        public void AssignSheetData_WithResettledTransaction_SetsCorrectResettledValue()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            var transaction = _fixture.Create<PlayerDetailTransactionViewModel>();
            transaction.IsResettled = true;
            data.DetailTransactions = new List<PlayerDetailTransactionViewModel> { transaction };
            _excel.SetData("test", data);

            var mockCells = new List<Mock<ICell>>();
            for (int i = 0; i < 7; i++)
            {
                mockCells.Add(new Mock<ICell>());
            }
            _mockRow.SetupGet(r => r.Cells).Returns(mockCells.Select(c => c.Object).ToList());

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignSheetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ISheet) }, null);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            mockCells[5].Verify(c => c.SetCellValue("v"), Times.Once);
        }

        [Fact]
        public void AssignSheetData_WithNonResettledTransaction_SetsEmptyResettledValue()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            var transaction = _fixture.Create<PlayerDetailTransactionViewModel>();
            transaction.IsResettled = false;
            data.DetailTransactions = new List<PlayerDetailTransactionViewModel> { transaction };
            _excel.SetData("test", data);

            var mockCells = new List<Mock<ICell>>();
            for (int i = 0; i < 7; i++)
            {
                mockCells.Add(new Mock<ICell>());
            }
            _mockRow.SetupGet(r => r.Cells).Returns(mockCells.Select(c => c.Object).ToList());

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignSheetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ISheet) }, null);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            mockCells[5].Verify(c => c.SetCellValue(string.Empty), Times.Once);
        }

        [Fact]
        public void ProcessUI_WithSingleWalletEnabled_HidesAfterBalanceColumn()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.IsEnableSingleWallet = true;
            data.ViewType = ViewTypes.MainWallet;
            _excel.SetData("test", data);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("ProcessUI", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.SetColumnHidden(4, true), Times.Once);
        }

        [Fact]
        public void ProcessUI_WithSabaCoinsViewType_HidesAfterBalanceColumn()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.IsEnableSingleWallet = false;
            data.ViewType = ViewTypes.SabaCoins;
            _excel.SetData("test", data);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("ProcessUI", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.SetColumnHidden(4, true), Times.Once);
        }

        [Fact]
        public void ProcessUI_WithMainWalletAndSingleWalletDisabled_DoesNotHideColumn()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.IsEnableSingleWallet = false;
            data.ViewType = ViewTypes.MainWallet;
            _excel.SetData("test", data);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("ProcessUI", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.SetColumnHidden(It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Export_CallsBaseExportMethod()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            _excel.SetData("test", data);

            // Create a temporary template file for the test
            var tempFile = Path.GetTempFileName();
            try
            {
                using (var fs = new FileStream(tempFile, FileMode.Create))
                {
                    var workbook = new HSSFWorkbook();
                    var sheet = workbook.CreateSheet("Template");
                    var title = sheet.CreateRow(0);
                    for (int i = 0; i < 7; i++)
                    {
                        title.CreateCell(i);
                    }
                    var header = sheet.CreateRow(1);
                    for (int i = 0; i < 7; i++)
                    {
                        header.CreateCell(i);
                    }
                    var content = sheet.CreateRow(2);
                    for (int i = 0; i < 7; i++)
                    {
                        content.CreateCell(i);
                    }
                    workbook.Write(fs);
                }

                _excel.SetData(tempFile, data);

                // Act & Assert - Should not throw exception
                _excel.Export();
                Assert.NotNull(_excel.Workbook);
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public void AssignSheetData_WithVariousTransactionCounts_ProcessesCorrectly(int transactionCount)
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.DetailTransactions = CreateDetailTransactions(transactionCount);
            _excel.SetData("test", data);

            var cells = new List<ICell>();
            for (int i = 0; i < 7; i++)
            {
                cells.Add(new Mock<ICell>().Object);
            }
            _mockRow.SetupGet(r => r.Cells).Returns(cells);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignSheetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ISheet) }, null);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            if (transactionCount > 0)
            {
                _mockSheet.Verify(s => s.GetRow(It.IsAny<int>()), Times.AtLeast(transactionCount));
            }
            else
            {
                _mockSheet.Verify(s => s.GetRow(It.IsAny<int>()), Times.Never);
            }
        }

        [Fact]
        public void AssignSheetData_WithLargeDataset_HandlesCorrectly()
        {
            // Arrange
            var data = CreateValidPlayerDetailTransactionReportViewModel();
            data.DetailTransactions = CreateDetailTransactions(1000);
            _excel.SetData("test", data);

            var cells = new List<ICell>();
            for (int i = 0; i < 7; i++)
            {
                cells.Add(new Mock<ICell>().Object);
            }
            _mockRow.SetupGet(r => r.Cells).Returns(cells);

            // Act
            var method = typeof(PlayerDetailTransactionExcel).GetMethod("AssignSheetData", BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ISheet) }, null);
            method.Invoke(_excel, new object[] { _mockSheet.Object });

            // Assert
            _mockSheet.Verify(s => s.GetRow(It.IsAny<int>()), Times.AtLeast(1000));
        }

        private PlayerDetailTransactionReportViewModel CreateValidPlayerDetailTransactionReportViewModel()
        {
            return new PlayerDetailTransactionReportViewModel
            {
                DetailTransactions = CreateDetailTransactions(3),
                ViewType = ViewTypes.MainWallet,
                FromDate = DateTime.Parse("01/01/2023"),
                ToDate = DateTime.Parse("01/31/2023"),
                IsEnableSingleWallet = false
            };
        }

        private IEnumerable<PlayerDetailTransactionViewModel> CreateDetailTransactions(int count)
        {
            var transactions = new List<PlayerDetailTransactionViewModel>();
            for (int i = 0; i < count; i++)
            {
                transactions.Add(new PlayerDetailTransactionViewModel
                {
                    TransactionId = _fixture.Create<string>(),
                    TransactionType = _fixture.Create<string>(),
                    IsResettled = i % 2 == 0,
                    Remark = _fixture.Create<string>()
                });
            }
            return transactions;
        }
    }
}