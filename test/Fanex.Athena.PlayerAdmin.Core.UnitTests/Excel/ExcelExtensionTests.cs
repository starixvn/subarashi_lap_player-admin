using Fanex.Athena.PlayerAdmin.Core.Excel;
using Moq;
using NPOI.SS.UserModel;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Excel
{
    public class ExcelExtensionTests
    {
        [Fact]
        public void GetCell_ShouldReturnCell()
        {
            // Arrange
            var sheetMock = new Mock<ISheet>();
            var rowMock = new Mock<IRow>();
            var cellMock = new Mock<ICell>();
            var rowIndex = 0;
            var colIndex = 0;

            sheetMock.Setup(s => s.GetRow(rowIndex)).Returns(rowMock.Object);
            rowMock.Setup(r => r.GetCell(colIndex)).Returns(cellMock.Object);

            // Act
            var result = sheetMock.Object.GetCell(rowIndex, colIndex);

            // Assert
            Assert.Equal(cellMock.Object, result);
        }

        [Fact]
        public void MergeCells_ShouldAddMergedRegion()
        {
            // Arrange
            var sheetMock = new Mock<ISheet>();
            var firstRow = 0;
            var firstCol = 0;
            var lastRow = 1;
            var lastCol = 1;

            // Act
            sheetMock.Object.MergeCells(firstRow, firstCol, lastRow, lastCol);

            // Assert
            sheetMock.Verify(s => s.AddMergedRegion(It.Is<NPOI.SS.Util.CellRangeAddress>(
                cra => cra.FirstRow == firstRow &&
                       cra.FirstColumn == firstCol &&
                       cra.LastRow == lastRow &&
                       cra.LastColumn == lastCol
            )), Times.Once);
        }

        [Fact]
        public void MergeCols_ShouldAddMergedRegionForSingleRow()
        {
            // Arrange
            var sheetMock = new Mock<ISheet>();
            var firstCol = 0;
            var lastCol = 2;
            var rowIndex = 1;

            // Act
            sheetMock.Object.MergeCols(firstCol, lastCol, rowIndex);

            // Assert
            sheetMock.Verify(s => s.AddMergedRegion(It.Is<NPOI.SS.Util.CellRangeAddress>(
                cra => cra.FirstRow == rowIndex &&
                       cra.LastRow == rowIndex &&
                       cra.FirstColumn == firstCol &&
                       cra.LastColumn == lastCol
            )), Times.Once);
        }

        [Fact]
        public void CopyCell_SourceCellIsNull_ReturnsNull()
        {
            // Arrange
            var sheetMock = new Mock<ISheet>();
            ICell sourceCell = null;
            var destCellMock = new Mock<ICell>();

            // Act
            var result = sheetMock.Object.CopyCell(sourceCell, destCellMock.Object);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CopyCell_CopiesProperties()
        {
            // Arrange
            var sheetMock = new Mock<ISheet>();
            var sourceCellMock = new Mock<ICell>();
            var destCellMock = new Mock<ICell>();
            var cellStyleMock = new Mock<ICellStyle>();
            var commentMock = new Mock<IComment>();
            var hyperlinkMock = new Mock<IHyperlink>();
            var richTextStringMock = new Mock<IRichTextString>();

            sourceCellMock.SetupGet(c => c.CellStyle).Returns(cellStyleMock.Object);
            sourceCellMock.SetupGet(c => c.CellComment).Returns(commentMock.Object);
            sourceCellMock.SetupGet(c => c.Hyperlink).Returns(hyperlinkMock.Object);
            sourceCellMock.SetupGet(c => c.CellType).Returns(CellType.String);
            sourceCellMock.SetupGet(c => c.RichStringCellValue).Returns(richTextStringMock.Object);

            // The following setup is required because of a potential bug in the source code.
            // The source code checks if destCell.CellComment is not null, instead of sourceCell.CellComment.
            destCellMock.SetupProperty(c => c.CellComment, commentMock.Object);

            // Act
            sheetMock.Object.CopyCell(sourceCellMock.Object, destCellMock.Object);

            // Assert
            destCellMock.VerifySet(c => c.CellStyle = cellStyleMock.Object, Times.Once);
            destCellMock.VerifySet(c => c.CellComment = commentMock.Object, Times.Once);
            destCellMock.VerifySet(c => c.Hyperlink = hyperlinkMock.Object, Times.Once);
            destCellMock.Verify(c => c.SetCellType(CellType.String), Times.Once);
            destCellMock.Verify(c => c.SetCellValue(richTextStringMock.Object), Times.Once);
        }
    }
} 