using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Excel
{
    public abstract class BaseExcel
    {
        public const int RowHeaderTemplate = 1;
        public const int RowTitleTemplate = 0;
        public const int MaxRowCount = 30000;

        public const string TemplateSheetName = "Template";

        public const string FormatDate = "MM/dd/yyyy";

        protected const int ColumnIndex = 0;

        private string _templatePath;

        private int _totalRows;

        private int _pageSize;

        protected void SetData(string templatePath, int totalRows, int pageSize = 0)
        {
            _templatePath = templatePath;
            _totalRows = totalRows;
            PageSize = pageSize;
        }

        protected bool IsMultiSheet { get; set; }

        protected int TotalRows
        {
            get
            {
                return _totalRows;
            }
        }

        protected int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value == 0 || value > MaxRowCount) ? MaxRowCount : value;
            }
        }

        protected virtual ISheet TemplateSheet { get; private set; }

        public virtual IWorkbook Workbook { get; set; }

        protected virtual int NumberOfSheets
        {
            get
            {
                return GetNumberOfSheets(_totalRows, PageSize);
            }
        }

        public virtual void Export()
        {
            LoadTemplate();

            if (IsMultiSheet)
            {
                AssignDataMultiSheet();
            }
            else
            {
                AssignData();
            }

            RemoveTemplate();
        }

        protected static void ClearTemplate(ISheet sheet, int startRowIndex)
        {
            int lastRowNum = sheet.LastRowNum;
            for (int i = startRowIndex; i <= lastRowNum; i++)
            {
                sheet.RemoveRow(sheet.GetRow(i));
            }
        }

        protected static int GetNumberOfSheets(int numberOfItems, int pageSize)
        {
            return numberOfItems % pageSize == 0 ? numberOfItems / pageSize : (numberOfItems / pageSize) + 1;
        }

        protected static void SetTextRowHeader(IRow row, IEnumerable<string> headerTextList)
        {
            for (int i = 0; i < headerTextList.Count(); i++)
            {
                row.Cells[i].SetCellValue(headerTextList.ElementAt(i));
            }
        }

        protected virtual void AssignData()
        {
            var sheet = CloneSheet(Workbook, TemplateSheetName, "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (_totalRows == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUI(sheet);
        }

        protected virtual void AssignDataMultiSheet()
        {
            ISheet sheet;

            if (_totalRows == 0)
            {
                sheet = CloneSheet(Workbook, TemplateSheetName, "Data");
                AssignTitle(sheet);
                AssignHeader(sheet);
                AssignNoInfoRow(sheet);
                ProcessUI(sheet);
            }
            else
            {
                for (int i = 0; i < NumberOfSheets; i++)
                {
                    sheet = CloneSheet(Workbook, TemplateSheetName, "Data" + (i + 1).ToString());
                    AssignTitle(sheet);
                    AssignHeader(sheet);
                    AssignSheetData(sheet, i);
                    ProcessUI(sheet);
                }
            }
        }

        protected abstract void AssignTitle(ISheet sheet);

        protected abstract void AssignHeader(ISheet sheet);

        protected virtual void ProcessUI(ISheet sheet)
        {
        }

        protected virtual void AssignNoInfoRow(ISheet sheet)
        {
        }

        protected virtual void AssignSheetData(ISheet sheet)
        {
        }

        protected virtual void AssignSheetData(ISheet sheet, int pageIndex)
        {
        }

        protected virtual void LoadTemplate()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(_templatePath, FileMode.Open, FileAccess.Read);
                Workbook = new HSSFWorkbook(stream);
                TemplateSheet = Workbook.GetSheet(TemplateSheetName);
                var defaultStyle = Workbook.CreateCellStyle();
                defaultStyle.ShrinkToFit = true;
                defaultStyle.WrapText = true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        protected virtual void RemoveTemplate()
        {
            RemoveSheet(Workbook, TemplateSheetName);
        }

        public static ISheet CloneSheet(IWorkbook workbook, string sourceSheetName, string newSheetName)
        {
            var sheetIndex = workbook.GetSheetIndex(sourceSheetName);
            var newSheet = workbook.CloneSheet(sheetIndex);
            var newSheetIndex = workbook.GetSheetIndex(newSheet);
            workbook.SetSheetName(newSheetIndex, newSheetName);
            return newSheet;
        }

        public static void RemoveSheet(IWorkbook workbook, string sheetName)
        {
            var sheet = workbook.GetSheet(sheetName);

            if (sheet != null)
            {
                var sheetIndex = workbook.GetSheetIndex(sheet);
                workbook.RemoveSheetAt(sheetIndex);
            }
        }
    }
}