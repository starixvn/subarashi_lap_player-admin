namespace Fanex.Athena.PlayerAdmin.MVC.Repositories.Interfaces
{
    using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
    using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
    using System.Threading.Tasks;

    public interface IPlayerDetailTransactionRepository
    {
        Task<PlayerDetailTransactionReportViewModel> GetDetailTransactions(PlayerDetailTransactionsMetaModel criteria);
    }
}