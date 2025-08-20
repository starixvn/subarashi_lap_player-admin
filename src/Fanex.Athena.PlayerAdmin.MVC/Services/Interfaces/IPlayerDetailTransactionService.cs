namespace Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces
{
    using System.Threading.Tasks;
    using MetaModel;
    using ViewModels;

    public interface IPlayerDetailTransactionService
    {
        Task<PlayerDetailTransactionReportViewModel> GetDetailTransactionReport(PlayerDetailTransactionsMetaModel criteria);
    }
}
