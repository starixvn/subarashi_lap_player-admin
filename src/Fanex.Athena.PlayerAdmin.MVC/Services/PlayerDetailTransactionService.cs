using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Services
{
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