using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using NPOI.SS.UserModel;

namespace Fanex.Athena.PlayerAdmin.MVC.Excel
{
    public interface IPlayerDetailTransactionExcel
    {
        IWorkbook Workbook { get; set; }

        void SetData(string templatePath, PlayerDetailTransactionReportViewModel data);

        void Export();
    }
}
