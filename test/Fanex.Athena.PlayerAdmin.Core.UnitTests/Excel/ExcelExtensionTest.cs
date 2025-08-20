namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Excel
{
    using Core.Excel;
    using Moq;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public class ExcelExtensionTest
    {
        private readonly Mock<ICell> _mockCell;
        private readonly Mock<IRow> _mockRow;
        private readonly Mock<ISheet> _mockSheet;
        private readonly Mock<ICell> _mockSourceCell;
        private readonly Mock<IRow> _mockSourceRow;
        private readonly Mock<ISheet> _mockSourceSheet;

        public ExcelExtensionTest()
        {
            _mockSheet = new Mock<ISheet>();
            _mockSourceSheet = new Mock<ISheet>();
            _mockRow = new Mock<IRow>();
            _mockSourceRow = new Mock<IRow>();
            _mockCell = new Mock<ICell>();
            _mockSourceCell = new Mock<ICell>();
        }

        [Fact]
        public void CopyRow_WithValidParameters_CreatesNewRow()
        {
            // Arrange
            var rowIndex = 5;
            _mockSheet.Setup(x => x.LastRowNum).Returns(3);
            _mockSheet.Setup(x => x.CreateRow(It.IsAny<int>())).Returns(_mockRow.Object);
            _mockSheet.Setup(x => x.GetRow(rowIndex)).Returns(_mockRow.Object);
            _mockSourceSheet.Setup(x => x.GetRow(rowIndex)).Returns(_mockSourceRow.Object);
            _mockSourceRow.Setup(x => x.LastCellNum).Returns(3);
            _mockSourceRow.Setup(x => x.GetCell(It.IsAny<int>())).Returns(_mockSourceCell.Object);
            _mockRow.Setup(x => x.CreateCell(It.IsAny<int>())).Returns(_mockCell.Object);

            // Act
            var result = _mockSheet.Object.CopyRow(_mockSourceSheet.Object, rowIndex);

            // Assert
            Assert.NotNull(result);
            _mockSheet.Verify(x => x.CreateRow(It.IsInRange(4, 5, Range.Inclusive)), Times.AtLeast(1));
        }

        [Fact]
        public void CopyRow_WithSourceAndDestIndexes_CreatesNewRow()
        {
            // Arrange
            var sourceRowIndex = 2;
            var destRowIndex = 5;
            _mockSheet.Setup(x => x.LastRowNum).Returns(3);
            _mockSheet.Setup(x => x.CreateRow(It.IsAny<int>())).Returns(_mockRow.Object);
            _mockSheet.Setup(x => x.GetRow(destRowIndex)).Returns(_mockRow.Object);
            _mockSourceSheet.Setup(x => x.GetRow(sourceRowIndex)).Returns(_mockSourceRow.Object);
            _mockSourceRow.Setup(x => x.LastCellNum).Returns(3);
            _mockSourceRow.Setup(x => x.GetCell(It.IsAny<int>())).Returns(_mockSourceCell.Object);
            _mockRow.Setup(x => x.CreateCell(It.IsAny<int>())).Returns(_mockCell.Object);

            // Act
            var result = _mockSheet.Object.CopyRow(_mockSourceSheet.Object, sourceRowIndex, destRowIndex);

            // Assert
            Assert.NotNull(result);
            _mockSheet.Verify(x => x.CreateRow(It.IsInRange(4, 5, Range.Inclusive)), Times.AtLeast(1));
        }

        [Fact]
        public void CopyRow_WhenDestRowIndexExceedsLastRow_CreatesIntermediateRows()
        {
            // Arrange
            var sourceRowIndex = 1;
            var destRowIndex = 10;
            _mockSheet.Setup(x => x.LastRowNum).Returns(3);
            _mockSheet.Setup(x => x.CreateRow(It.IsAny<int>())).Returns(_mockRow.Object);
            _mockSheet.Setup(x => x.GetRow(destRowIndex)).Returns(_mockRow.Object);
            _mockSourceSheet.Setup(x => x.GetRow(sourceRowIndex)).Returns(_mockSourceRow.Object);
            _mockSourceRow.Setup(x => x.LastCellNum).Returns(2);
            _mockSourceRow.Setup(x => x.GetCell(It.IsAny<int>())).Returns(_mockSourceCell.Object);
            _mockRow.Setup(x => x.CreateCell(It.IsAny<int>())).Returns(_mockCell.Object);

            // Act
            var result = _mockSheet.Object.CopyRow(_mockSourceSheet.Object, sourceRowIndex, destRowIndex);

            // Assert
            Assert.NotNull(result);
            // Should create rows 4 through 10 (inclusive)
            _mockSheet.Verify(x => x.CreateRow(It.IsInRange(4, 10, Range.Inclusive)), Times.Exactly(7));
        }

        [Fact]
        public void CopyRow_WithNullSourceRow_ReturnsNewRow()
        {
            // Arrange
            var rowIndex = 5;
            _mockSheet.Setup(x => x.LastRowNum).Returns(3);
            _mockSheet.Setup(x => x.CreateRow(It.IsAny<int>())).Returns(_mockRow.Object);
            _mockSheet.Setup(x => x.GetRow(rowIndex)).Returns(_mockRow.Object);
            _mockSourceSheet.Setup(x => x.GetRow(rowIndex)).Returns((IRow)null);

            // Act
            var result = _mockSheet.Object.CopyRow(_mockSourceSheet.Object, rowIndex);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_mockRow.Object, result);
        }

        [Fact]
        public void CopyRow_WhenDestRowExists_ReturnsExistingRow()
        {
            // Arrange
            var rowIndex = 2;
            var existingRow = new Mock<IRow>();
            _mockSheet.Setup(x => x.LastRowNum).Returns(5);
            _mockSheet.Setup(x => x.GetRow(rowIndex)).Returns(existingRow.Object);
            _mockSourceSheet.Setup(x => x.GetRow(rowIndex)).Returns(_mockSourceRow.Object);
            _mockSourceRow.Setup(x => x.LastCellNum).Returns(2);
            _mockSourceRow.Setup(x => x.GetCell(It.IsAny<int>())).Returns(_mockSourceCell.Object);
            existingRow.Setup(x => x.CreateCell(It.IsAny<int>())).Returns(_mockCell.Object);

            // Act
            var result = _mockSheet.Object.CopyRow(_mockSourceSheet.Object, rowIndex);

            // Assert
            Assert.Equal(existingRow.Object, result);
        }

        [Fact]
        public void CopyCell_WithValidSourceCell_CopiesAllProperties()
        {
            // Arrange
            var mockCellStyle = new Mock<ICellStyle>();
            var mockComment = new Mock<IComment>();
            var mockHyperlink = new Mock<IHyperlink>();

            _mockSourceCell.Setup(x => x.CellStyle).Returns(mockCellStyle.Object);
            _mockSourceCell.Setup(x => x.CellComment).Returns(mockComment.Object);
            _mockSourceCell.Setup(x => x.Hyperlink).Returns(mockHyperlink.Object);
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.String);
            _mockSourceCell.Setup(x => x.RichStringCellValue).Returns(new Mock<IRichTextString>().Object);
            _mockCell.Setup(x => x.CellComment).Returns(mockComment.Object);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            _mockCell.VerifySet(x => x.CellStyle = mockCellStyle.Object, Times.Once);
            _mockCell.VerifySet(x => x.Hyperlink = mockHyperlink.Object, Times.Once);
            _mockCell.Verify(x => x.SetCellType(CellType.String), Times.Once);
        }

        [Fact]
        public void CopyCell_WithNullSourceCell_ReturnsNull()
        {
            // Act
            var result = _mockSheet.Object.CopyCell(null, _mockCell.Object);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(CellType.String)]
        [InlineData(CellType.Numeric)]
        [InlineData(CellType.Boolean)]
        [InlineData(CellType.Error)]
        [InlineData(CellType.Blank)]
        public void CopyCell_WithDifferentCellTypes_HandlesCorrectly(CellType cellType)
        {
            // Arrange
            _mockSourceCell.Setup(x => x.CellType).Returns(cellType);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);

            switch (cellType)
            {
                case CellType.String:
                    var richText = new Mock<IRichTextString>();
                    _mockSourceCell.Setup(x => x.RichStringCellValue).Returns(richText.Object);
                    break;
                case CellType.Numeric:
                    _mockSourceCell.Setup(x => x.NumericCellValue).Returns(123.45);
                    break;
                case CellType.Boolean:
                    _mockSourceCell.Setup(x => x.BooleanCellValue).Returns(true);
                    break;
                case CellType.Error:
                    _mockSourceCell.Setup(x => x.ErrorCellValue).Returns(1);
                    break;
                case CellType.Blank:
                    _mockSourceCell.Setup(x => x.StringCellValue).Returns(string.Empty);
                    break;
            }

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            _mockCell.Verify(x => x.SetCellType(cellType), Times.Once);
        }

        [Fact]
        public void CopyCell_CopiesStringValue_WhenCellTypeIsString()
        {
            // Arrange
            var richTextValue = new Mock<IRichTextString>();
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.String);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);
            _mockSourceCell.Setup(x => x.RichStringCellValue).Returns(richTextValue.Object);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            _mockCell.Verify(x => x.SetCellValue(richTextValue.Object), Times.Once);
        }

        [Fact]
        public void CopyCell_CopiesNumericValue_WhenCellTypeIsNumeric()
        {
            // Arrange
            const double numericValue = 123.45;
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.Numeric);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);
            _mockSourceCell.Setup(x => x.NumericCellValue).Returns(numericValue);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            _mockCell.Verify(x => x.SetCellValue(numericValue), Times.Once);
        }

        [Fact]
        public void CopyCell_CopiesBooleanValue_WhenCellTypeIsBoolean()
        {
            // Arrange
            const bool booleanValue = true;
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.Boolean);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);
            _mockSourceCell.Setup(x => x.BooleanCellValue).Returns(booleanValue);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            _mockCell.Verify(x => x.SetCellValue(booleanValue), Times.Once);
        }

        [Fact]
        public void CopyCell_CopiesErrorValue_WhenCellTypeIsError()
        {
            // Arrange
            const byte errorValue = 15;
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.Error);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);
            _mockSourceCell.Setup(x => x.ErrorCellValue).Returns(errorValue);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            _mockCell.Verify(x => x.SetCellErrorValue(errorValue), Times.Once);
        }

        [Fact]
        public void CopyCell_CopiesStringValue_WhenCellTypeIsBlank()
        {
            // Arrange
            const string stringValue = "";
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.Blank);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);
            _mockSourceCell.Setup(x => x.StringCellValue).Returns(stringValue);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            _mockCell.Verify(x => x.SetCellValue(stringValue), Times.Once);
        }

        [Fact]
        public void CopyCell_HandlesMissingComment_Gracefully()
        {
            // Arrange
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.String);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);
            _mockSourceCell.Setup(x => x.CellComment).Returns((IComment)null);
            _mockSourceCell.Setup(x => x.RichStringCellValue).Returns(new Mock<IRichTextString>().Object);
            _mockCell.Setup(x => x.CellComment).Returns((IComment)null);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            // Should not try to set comment when both source and dest are null
        }

        [Fact]
        public void CopyCell_HandlesMissingHyperlink_Gracefully()
        {
            // Arrange
            _mockSourceCell.Setup(x => x.CellType).Returns(CellType.String);
            _mockSourceCell.Setup(x => x.CellStyle).Returns(new Mock<ICellStyle>().Object);
            _mockSourceCell.Setup(x => x.Hyperlink).Returns((IHyperlink)null);
            _mockSourceCell.Setup(x => x.RichStringCellValue).Returns(new Mock<IRichTextString>().Object);

            // Act
            var result = _mockSheet.Object.CopyCell(_mockSourceCell.Object, _mockCell.Object);

            // Assert
            Assert.NotNull(result);
            // Should not try to set hyperlink when source is null
        }

        [Fact]
        public void MergeCols_CallsCorrectMergeCellsMethod()
        {
            // Arrange
            var firstCol = 0;
            var lastCol = 5;
            var rowIndex = 3;
            var expectedResult = 42;


            _mockSheet.Setup(x => x.AddMergedRegion(It.IsAny<CellRangeAddress>()))
                .Returns(expectedResult);

            // Act
            var result = _mockSheet.Object.MergeCols(firstCol, lastCol, rowIndex);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockSheet.Verify(x => x.AddMergedRegion(It.Is<CellRangeAddress>(range => range.FirstRow == rowIndex &&
                                                                                      range.LastRow == rowIndex &&
                                                                                      range.FirstColumn == firstCol &&
                                                                                      range.LastColumn == lastCol)), Times.Once);
        }

        [Fact]
        public void MergeCells_CreatesCorrectCellRangeAddress()
        {
            // Arrange
            var firstRow = 1;
            var firstCol = 2;
            var lastRow = 3;
            var lastCol = 4;
            var expectedResult = 99;


            _mockSheet.Setup(x => x.AddMergedRegion(It.IsAny<CellRangeAddress>()))
                .Returns(expectedResult);

            // Act
            var result = _mockSheet.Object.MergeCells(firstRow, firstCol, lastRow, lastCol);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockSheet.Verify(x => x.AddMergedRegion(It.Is<CellRangeAddress>(range => range.FirstRow == firstRow &&
                                                                                      range.LastRow == lastRow &&
                                                                                      range.FirstColumn == firstCol &&
                                                                                      range.LastColumn == lastCol)), Times.Once);
        }

        [Fact]
        public void HideColumn_ClearsContentAndHidesColumns()
        {
            // Arrange
            var columnsToHide = new[] { 1, 3, 5 };
            var firstRowNum = 0;
            var lastRowNum = 10;
            var mockCellToHide = new Mock<ICell>();

            _mockSheet.Setup(x => x.FirstRowNum).Returns(firstRowNum);
            _mockSheet.Setup(x => x.LastRowNum).Returns(lastRowNum);
            _mockSheet.Setup(x => x.GetRow(It.IsAny<int>()))
                .Returns(_mockRow.Object);
            _mockRow.Setup(x => x.GetCell(It.IsAny<int>()))
                .Returns(mockCellToHide.Object);

            // Act
            _mockSheet.Object.HideColumn(columnsToHide);

            // Assert
            foreach (var columnIndex in columnsToHide)
            {
                _mockSheet.Verify(x => x.SetColumnWidth(columnIndex, 0), Times.Once);
                _mockSheet.Verify(x => x.SetColumnHidden(columnIndex, true), Times.Once);
            }

            // Verify cells are cleared
            var expectedRows = lastRowNum - firstRowNum + 1;
            foreach (var col in columnsToHide)
            {
                _mockRow.Verify(x => x.GetCell(It.Is<int>(c => c == col)), Times.Exactly(expectedRows));
            }

            mockCellToHide.Verify(x => x.SetCellValue(string.Empty), Times.Exactly(expectedRows * columnsToHide.Length));
        }

        [Fact]
        public void GetCell_ReturnsCorrectCell()
        {
            // Arrange
            var rowIndex = 5;
            var colIndex = 3;
            var mockReturnedRow = new Mock<IRow>();
            var mockReturnedCell = new Mock<ICell>();

            _mockSheet.Setup(x => x.GetRow(rowIndex)).Returns(mockReturnedRow.Object);
            mockReturnedRow.Setup(x => x.GetCell(colIndex)).Returns(mockReturnedCell.Object);

            // Act
            var result = _mockSheet.Object.GetCell(rowIndex, colIndex);

            // Assert
            Assert.Equal(mockReturnedCell.Object, result);
            _mockSheet.Verify(x => x.GetRow(rowIndex), Times.Once);
            mockReturnedRow.Verify(x => x.GetCell(colIndex), Times.Once);
        }
    }
}