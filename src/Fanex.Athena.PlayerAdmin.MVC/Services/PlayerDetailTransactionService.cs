namespace Fanex.Athena.PlayerAdmin.MVC.Services
{
    using System.Threading.Tasks;
    using Interfaces;
    using MetaModel;
    using Repositories.Interfaces;
    using ViewModels;

    public class PlayerDetailTransactionService : IPlayerDetailTransactionService
    {
        private readonly IPlayerDetailTransactionRepository _repository;

        public PlayerDetailTransactionService(IPlayerDetailTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PlayerDetailTransactionReportViewModel> GetDetailTransactionReport(PlayerDetailTransactionsMetaModel criteria)
        {
            return await _repository.GetDetailTransactions(criteria);
        }
    }
}