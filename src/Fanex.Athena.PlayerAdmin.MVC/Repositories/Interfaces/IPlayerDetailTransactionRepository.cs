namespace Fanex.Athena.PlayerAdmin.MVC.Repositories.Interfaces
{
    using System.Threading.Tasks;
    using MetaModel;
    using ViewModels;

    public interface IPlayerDetailTransactionRepository
    {
        Task<PlayerDetailTransactionReportViewModel> GetDetailTransactions(PlayerDetailTransactionsMetaModel criteria);
    }
}