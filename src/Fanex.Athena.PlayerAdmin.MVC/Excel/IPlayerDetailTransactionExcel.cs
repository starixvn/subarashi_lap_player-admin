namespace Fanex.Athena.PlayerAdmin.MVC.Excel
{
    using NPOI.SS.UserModel;
    using ViewModels;

    public interface IPlayerDetailTransactionExcel
    {
        IWorkbook Workbook { get; set; }

        void SetData(string templatePath, PlayerDetailTransactionReportViewModel data);

        void Export();
    }
}
