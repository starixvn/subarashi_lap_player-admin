namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Excel
{
    using System.Reflection;
    using Core.Excel;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class BaseExcelTests
    {
        [Fact]
        public void GetNumberOfSheets_ComputesCorrectly()
        {
            var method = typeof(BaseExcel).GetMethod("GetNumberOfSheets", BindingFlags.NonPublic | BindingFlags.Static);
            var sheets1 = (int)method.Invoke(null, new object[] { 100, 50 });
            var sheets2 = (int)method.Invoke(null, new object[] { 101, 50 });
            Assert.Equal(2, sheets1);
            Assert.Equal(3, sheets2);
        }

        [Fact]
        public void CloneAndRemoveSheet_Works()
        {
            var wb = new HSSFWorkbook();
            var original = wb.CreateSheet("Template");
            original.CreateRow(0).CreateCell(0).SetCellValue("x");

            var sheet = BaseExcel.CloneSheet(wb, "Template", "Data");
            Assert.Equal("Data", wb.GetSheetName(wb.GetSheetIndex(sheet)));

            BaseExcel.RemoveSheet(wb, "Template");
            Assert.Null(wb.GetSheet("Template"));
        }

        [Fact]
        public void ClearTemplate_RemovesRows()
        {
            var wb = new HSSFWorkbook();
            var sheet = wb.CreateSheet("Sheet1");
            for (int i = 0; i < 3; i++)
            {
                sheet.CreateRow(i).CreateCell(0).SetCellValue(i);
            }

            var method = typeof(BaseExcel).GetMethod("ClearTemplate", BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { sheet, 1 });

            Assert.NotNull(sheet.GetRow(0));
            Assert.Null(sheet.GetRow(1));
            Assert.Null(sheet.GetRow(2));
        }

        [Fact]
        public void SetTextRowHeader_SetsCellValues()
        {
            var wb = new HSSFWorkbook();
            var sheet = wb.CreateSheet("Template");
            var row = sheet.CreateRow(0);
            row.CreateCell(0);
            row.CreateCell(1);
            var method = typeof(BaseExcel).GetMethod("SetTextRowHeader", BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { row, new[] { "A", "B" } });
            Assert.Equal("A", row.GetCell(0).StringCellValue);
            Assert.Equal("B", row.GetCell(1).StringCellValue);
        }

        [Fact]
        public void AssignData_CallsExpectedMethods_BasedOnTotalRows()
        {
            var wb = new HSSFWorkbook();
            wb.CreateSheet("Template");
            var excel = new InstrumentationExcel();
            excel.SetWorkbook(wb);
            excel.CallSetData("ignored.xls", 0, 100);
            excel.RunAssignData();
            Assert.True(excel.HeaderCalled);
            Assert.True(excel.TitleCalled);
            Assert.True(excel.NoInfoCalled);
            Assert.True(excel.ProcessUICalled);
            Assert.NotNull(wb.GetSheet("Data"));

            var wb2 = new HSSFWorkbook();
            wb2.CreateSheet("Template");
            var excel2 = new InstrumentationExcel();
            excel2.SetWorkbook(wb2);
            excel2.CallSetData("ignored.xls", 120, 100);
            excel2.RunAssignData();
            Assert.True(excel2.HeaderCalled);
            Assert.True(excel2.TitleCalled);
            Assert.False(excel2.NoInfoCalled);
            Assert.True(excel2.SheetDataCalled);
            Assert.True(excel2.ProcessUICalled);
            Assert.NotNull(wb2.GetSheet("Data"));
        }

        [Fact]
        public void AssignDataMultiSheet_CreatesExpectedSheets_AndCallsSheetData()
        {
            var wb = new HSSFWorkbook();
            wb.CreateSheet("Template");
            var excel = new InstrumentationExcel();
            excel.SetWorkbook(wb);
            excel.CallSetData("ignored.xls", 120, 50);
            excel.RunAssignDataMultiSheet();
            Assert.NotNull(wb.GetSheet("Data1"));
            Assert.NotNull(wb.GetSheet("Data2"));
            Assert.NotNull(wb.GetSheet("Data3"));
            Assert.Equal(new[] { 0, 1, 2 }, excel.PageIndexes.ToArray());
        }

        [Fact]
        public void AssignDataMultiSheet_WithZeroTotalRows_CreatesDataSheetAndCallsNoInfo()
        {
            // Arrange
            var wb = new HSSFWorkbook();
            wb.CreateSheet("Template");
            var excel = new InstrumentationExcel();
            excel.SetWorkbook(wb);
            excel.CallSetData("ignored.xls", 0, 50);

            // Act
            excel.RunAssignDataMultiSheet();

            // Assert
            Assert.NotNull(wb.GetSheet("Data"));
            Assert.True(excel.TitleCalled);
            Assert.True(excel.HeaderCalled);
            Assert.True(excel.NoInfoCalled);
            Assert.True(excel.ProcessUICalled);
            Assert.False(excel.SheetDataCalled);
        }

        [Fact]
        public void SetData_SetsPropertiesCorrectly()
        {
            // Arrange
            var excel = new InstrumentationExcel();

            // Act
            excel.CallSetData("test.xls", 100, 25);

            // Assert
            Assert.Equal(100, excel.GetTotalRows());
            Assert.Equal(25, excel.GetPageSize());
        }

        [Theory]
        [InlineData(0, BaseExcel.MaxRowCount)]
        [InlineData(1, 1)]
        [InlineData(BaseExcel.MaxRowCount, BaseExcel.MaxRowCount)]
        [InlineData(BaseExcel.MaxRowCount + 1, BaseExcel.MaxRowCount)]
        public void PageSize_BoundaryValues_HandledCorrectly(int inputPageSize, int expectedPageSize)
        {
            // Arrange
            var excel = new InstrumentationExcel();

            // Act
            excel.CallSetData("test.xls", 1000, inputPageSize);

            // Assert
            Assert.Equal(expectedPageSize, excel.GetPageSize());
        }

        [Fact]
        public void NumberOfSheets_CalculatesCorrectly()
        {
            // Arrange
            var excel = new InstrumentationExcel();

            // Act
            excel.CallSetData("test.xls", 125, 50);

            // Assert
            Assert.Equal(3, excel.GetNumberOfSheets());
        }

        [Fact]
        public void IsMultiSheet_DefaultsToFalse()
        {
            // Arrange
            var excel = new InstrumentationExcel();

            // Act & Assert
            Assert.False(excel.GetIsMultiSheet());
        }

        [Fact]
        public void IsMultiSheet_CanBeSetToTrue()
        {
            // Arrange
            var excel = new InstrumentationExcel();

            // Act
            excel.SetIsMultiSheet(true);

            // Assert
            Assert.True(excel.GetIsMultiSheet());
        }

        [Fact]
        public void RemoveSheet_WithNonExistentSheet_DoesNotThrow()
        {
            // Arrange
            var wb = new HSSFWorkbook();
            wb.CreateSheet("ExistingSheet");

            // Act & Assert - Should not throw
            BaseExcel.RemoveSheet(wb, "NonExistentSheet");
            Assert.NotNull(wb.GetSheet("ExistingSheet"));
        }

        [Fact]
        public void Constants_HaveExpectedValues()
        {
            // Assert
            Assert.Equal(1, BaseExcel.RowHeaderTemplate);
            Assert.Equal(0, BaseExcel.RowTitleTemplate);
            Assert.Equal(30000, BaseExcel.MaxRowCount);
            Assert.Equal("Template", BaseExcel.TemplateSheetName);
            Assert.Equal("MM/dd/yyyy", BaseExcel.FormatDate);
        }

        private sealed class InstrumentationExcel : BaseExcel
        {
            public bool HeaderCalled { get; private set; }
            public bool TitleCalled { get; private set; }
            public bool NoInfoCalled { get; private set; }
            public bool SheetDataCalled { get; private set; }
            public bool ProcessUICalled { get; private set; }
            public List<int> PageIndexes { get; } = new List<int>();

            public void SetWorkbook(IWorkbook wb)
            {
                Workbook = wb;
            }

            public void CallSetData(string templatePath, int totalRows, int pageSize)
            {
                SetData(templatePath, totalRows, pageSize);
            }

            public void RunAssignData()
            {
                AssignData();
            }

            public void RunAssignDataMultiSheet()
            {
                IsMultiSheet = true;
                AssignDataMultiSheet();
            }

            public int GetTotalRows()
            {
                return TotalRows;
            }

            public int GetPageSize()
            {
                return PageSize;
            }

            public int GetNumberOfSheets()
            {
                return NumberOfSheets;
            }

            public bool GetIsMultiSheet()
            {
                return IsMultiSheet;
            }

            public void SetIsMultiSheet(bool value)
            {
                IsMultiSheet = value;
            }

            protected override void AssignTitle(ISheet sheet)
            {
                TitleCalled = true;
            }

            protected override void AssignHeader(ISheet sheet)
            {
                HeaderCalled = true;
            }

            protected override void AssignNoInfoRow(ISheet sheet)
            {
                NoInfoCalled = true;
            }

            protected override void AssignSheetData(ISheet sheet)
            {
                SheetDataCalled = true;
            }

            protected override void AssignSheetData(ISheet sheet, int pageIndex)
            {
                SheetDataCalled = true;
                PageIndexes.Add(pageIndex);
            }

            protected override void ProcessUI(ISheet sheet)
            {
                ProcessUICalled = true;
            }
        }
    }
}