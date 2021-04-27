namespace Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
    using Fanex.Athena.PlayerAdmin.MVC.ViewModels;

    public interface IPlayerDetailTransactionService
    {
        Task<PlayerDetailTransactionReportViewModel> GetDetailTransactionReport(PlayerDetailTransactionsMetaModel criteria);
    }
}
