using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Fanex.Athena.PlayerAdmin.MVC.Excel
{
    public static class ExcelExtensions
    {
        public static IRow CopyRow(this ISheet sheet, ISheet sourceSheet, int rowIndex)
        {
            var sourceRowIndex = rowIndex;
            var destRowIndex = sourceRowIndex;

            return CopyRow(sheet, sourceSheet, sourceRowIndex, destRowIndex);
        }

        public static IRow CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowIndex, int destRowIndex)
        {
            if (sheet.LastRowNum < destRowIndex)
            {
                for (var i = sheet.LastRowNum + 1; i <= destRowIndex; i++)
                {
                    sheet.CreateRow(i);
                }
            }

            // Get the source / new row
            var newRow = sheet.GetRow(destRowIndex);
            newRow = newRow ?? sheet.CreateRow(destRowIndex);

            var sourceRow = sourceSheet.GetRow(sourceRowIndex);
            if (sourceRow != null)
            {
                // Loop through source columns to add to new row
                for (int i = 0; i < sourceRow.LastCellNum; i++)
                {
                    // Grab a copy of the old/new cell
                    var oldCell = sourceRow.GetCell(i);
                    var newCell = newRow.CreateCell(i);

                    sheet.CopyCell(oldCell, newCell);
                }
            }

            return newRow;
        }

        public static ICell CopyCell(this ISheet sheet, ICell sourceCell, ICell destCell)
        {
            if (sourceCell == null)
            {
                return null;
            }

            destCell.CellStyle = sourceCell.CellStyle;

            if (destCell.CellComment != null)
            {
                destCell.CellComment = sourceCell.CellComment;
            }

            if (sourceCell.Hyperlink != null)
            {
                destCell.Hyperlink = sourceCell.Hyperlink;
            }

            destCell.SetCellType(sourceCell.CellType);

            switch (sourceCell.CellType)
            {
                case CellType.Blank:
                    destCell.SetCellValue(sourceCell.StringCellValue);
                    break;

                case CellType.Boolean:
                    destCell.SetCellValue(sourceCell.BooleanCellValue);
                    break;

                case CellType.Error:
                    destCell.SetCellErrorValue(sourceCell.ErrorCellValue);
                    break;

                case CellType.Numeric:
                    destCell.SetCellValue(sourceCell.NumericCellValue);
                    break;

                case CellType.String:
                    destCell.SetCellValue(sourceCell.RichStringCellValue);
                    break;

                default:
                    destCell.SetCellValue(sourceCell.StringCellValue);
                    break;
            }

            return destCell;
        }

        public static void SetTextRowValue(this ISheet sheet, ISheet sourceSheet, int rowIndex, int columnCount, string value)
        {
            var sourceRowIndex = rowIndex;
            var destRowIndex = rowIndex;

            SetTextRowValue(sheet, sourceSheet, sourceRowIndex, destRowIndex, columnCount, value);
        }

        public static void SetTextRowValue(
           this ISheet sheet,
           ISheet sourceSheet,
           int sourceRowIndex,
           int destRowIndex,
           int columnCount,
           string value,
           int cellContentIndex = 0)
        {
            var row = sheet.CopyRow(sourceSheet, sourceRowIndex, destRowIndex);
            sheet.MergeCols(0, columnCount - 1, destRowIndex);
            row.Cells[cellContentIndex].SetCellValue(value);
        }

        public static int MergeCols(this ISheet sheet, int firstCol, int lastCol, int rowIndex)
        {
            var firstRow = rowIndex;
            var lastRow = firstRow;
            return MergeCells(sheet, firstRow, firstCol, lastRow, lastCol);
        }

        public static int MergeCells(this ISheet sheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            var rangeAddress = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            return sheet.AddMergedRegion(rangeAddress);
        }
    }
}
