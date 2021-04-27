using Fanex.Athena.PlayerAdmin.Infrastructure.Extensions;
using Fanex.Athena.PlayerAdmin.Infrastructure.Repositories;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Repositories
{
    public class PlayerDetailTransactionRepository : IPlayerDetailTransactionRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IResourceService _resourceService;

        public PlayerDetailTransactionRepository(IDbFactory dbFactory, IResourceService resourceService)
        {
            _dbFactory = dbFactory;
            _resourceService = resourceService;
        }

        public async Task<PlayerDetailTransactionReportViewModel> GetDetailTransactions(PlayerDetailTransactionsMetaModel criteria)
        {
            var transactions = new List<PlayerDetailTransactionViewModel>();
            var result = new PlayerDetailTransactionReportViewModel(criteria, null);

            using (var db = _dbFactory.GetObjectDb(criteria.SPName))
            {
                criteria.SetParameterValues(db);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var trans = new PlayerDetailTransactionViewModel
                        {
                            TransactionId = reader.ToString("transid"),
                            BetType = reader.ToInt("bettype"),
                            TransactionTime = reader.ToDateTime("transtime"),
                            Remark = reader.ToString("transremark"),
                            BalanceChange = reader.ToDecimal("changedAmt"),
                            AfterBalance = reader.ToDecimal("totalAmt"),
                            IsResettled = reader.ToBool("IsResettled")
                        };

                        trans.TransactionType = _resourceService.GetBettype(trans.BetType);
                        transactions.Add(trans);
                    }
                }

                var output = db.GetOutputParams();
                result.DetailTransactions = transactions;
                result.TotalRow = Converter.ToInt(output["TotalRow"]);
            }

            return result;
        }
    }
}