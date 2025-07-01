using Fanex.Athena.PlayerAdmin.Core.Excel;
using Moq;
using NPOI.SS.UserModel;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Excel
{
    public class BaseExcelTests
    {
        [Fact]
        public void CloneSheet_ShouldCloneAndRenameSheet()
        {
            // Arrange
            var workbookMock = new Mock<IWorkbook>();
            var sheetMock = new Mock<ISheet>();
            var sourceSheetName = "Template";
            var newSheetName = "Data";
            var sourceSheetIndex = 0;
            var newSheetIndex = 1;

            workbookMock.Setup(w => w.GetSheetIndex(sourceSheetName)).Returns(sourceSheetIndex);
            workbookMock.Setup(w => w.CloneSheet(sourceSheetIndex)).Returns(sheetMock.Object);
            workbookMock.Setup(w => w.GetSheetIndex(sheetMock.Object)).Returns(newSheetIndex);

            // Act
            var result = BaseExcel.CloneSheet(workbookMock.Object, sourceSheetName, newSheetName);

            // Assert
            workbookMock.Verify(w => w.SetSheetName(newSheetIndex, newSheetName), Times.Once);
            Assert.Equal(sheetMock.Object, result);
        }

        [Fact]
        public void RemoveSheet_SheetExists_ShouldRemoveSheet()
        {
            // Arrange
            var workbookMock = new Mock<IWorkbook>();
            var sheetMock = new Mock<ISheet>();
            var sheetName = "Template";
            var sheetIndex = 0;

            workbookMock.Setup(w => w.GetSheet(sheetName)).Returns(sheetMock.Object);
            workbookMock.Setup(w => w.GetSheetIndex(sheetMock.Object)).Returns(sheetIndex);

            // Act
            BaseExcel.RemoveSheet(workbookMock.Object, sheetName);

            // Assert
            workbookMock.Verify(w => w.RemoveSheetAt(sheetIndex), Times.Once);
        }

        [Fact]
        public void RemoveSheet_SheetDoesNotExist_ShouldNotRemoveSheet()
        {
            // Arrange
            var workbookMock = new Mock<IWorkbook>();
            var sheetName = "NonExistentSheet";

            workbookMock.Setup(w => w.GetSheet(sheetName)).Returns((ISheet)null);

            // Act
            BaseExcel.RemoveSheet(workbookMock.Object, sheetName);

            // Assert
            workbookMock.Verify(w => w.RemoveSheetAt(It.IsAny<int>()), Times.Never);
        }
    }

    public class TestExcel : BaseExcel
    {
        protected override void AssignHeader(ISheet sheet) { }
        protected override void AssignTitle(ISheet sheet) { }

        public int CallGetNumberOfSheets(int numberOfItems, int pageSize)
        {
            return GetNumberOfSheets(numberOfItems, pageSize);
        }

        public void CallSetTextRowHeader(IRow row, System.Collections.Generic.IEnumerable<string> headerTextList)
        {
            SetTextRowHeader(row, headerTextList);
        }

        public void CallClearTemplate(ISheet sheet, int startRowIndex)
        {
            ClearTemplate(sheet, startRowIndex);
        }
    }

    public class TestExcelTests
    {
        private readonly TestExcel _testExcel = new TestExcel();

        [Theory]
        [InlineData(10, 5, 2)]
        [InlineData(11, 5, 3)]
        [InlineData(10, 2, 5)]
        public void GetNumberOfSheets_ShouldReturnCorrectNumber(int numberOfItems, int pageSize, int expected)
        {
            // Act
            var result = _testExcel.CallGetNumberOfSheets(numberOfItems, pageSize);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetTextRowHeader_ShouldSetCellValues()
        {
            // Arrange
            var rowMock = new Mock<IRow>();
            var cell1Mock = new Mock<ICell>();
            var cell2Mock = new Mock<ICell>();
            var headerTextList = new[] { "Header1", "Header2" };

            var cells = new System.Collections.Generic.List<ICell> { cell1Mock.Object, cell2Mock.Object };
            rowMock.Setup(r => r.Cells).Returns(cells);

            // Act
            _testExcel.CallSetTextRowHeader(rowMock.Object, headerTextList);

            // Assert
            cell1Mock.Verify(c => c.SetCellValue("Header1"), Times.Once);
            cell2Mock.Verify(c => c.SetCellValue("Header2"), Times.Once);
        }

        [Fact]
        public void ClearTemplate_ShouldRemoveRows()
        {
            // Arrange
            var sheetMock = new Mock<ISheet>();
            var row1Mock = new Mock<IRow>();
            var row2Mock = new Mock<IRow>();
            var startRowIndex = 1;

            sheetMock.SetupGet(s => s.LastRowNum).Returns(2);
            sheetMock.Setup(s => s.GetRow(1)).Returns(row1Mock.Object);
            sheetMock.Setup(s => s.GetRow(2)).Returns(row2Mock.Object);

            // Act
            _testExcel.CallClearTemplate(sheetMock.Object, startRowIndex);

            // Assert
            sheetMock.Verify(s => s.RemoveRow(row1Mock.Object), Times.Once);
            sheetMock.Verify(s => s.RemoveRow(row2Mock.Object), Times.Once);
        }
    }
} 