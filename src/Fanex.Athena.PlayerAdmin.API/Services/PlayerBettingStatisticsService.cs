using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Data;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services
{
    public class PlayerBettingStatisticsService : IPlayerBettingStatisticsService
    {
        private readonly IDbFactory _dbFactory;

        public PlayerBettingStatisticsService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Task<PlayerBettingStatistics> GetPlayerBettingStatistics(PlayerBettingStatisticsMetaModel metaModel)
        {
            const string SpKey = "Lrf_PlayerBettingStatistics";

            using (var objectDb = _dbFactory.GetObjectDb(SpKey))
            {
                AssignParam(objectDb, metaModel);

                return ReadData(objectDb);
            }
        }

        private static void AssignParam(IObjectDb objectDb, PlayerBettingStatisticsMetaModel metaModel)
        {
            objectDb.SetParameterValue("CustID", metaModel.CustId);
            objectDb.SetParameterValue("FromDate", metaModel.FromDate);
            objectDb.SetParameterValue("ToDate", metaModel.ToDate);
            objectDb.SetParameterValue("MoveBetsDay", metaModel.MoveBetsDay);
            objectDb.SetParameterValue("BalanceUpDay", metaModel.BalanceUpDay);
            objectDb.SetParameterValue("ProductList", metaModel.ProductCodeList);
        }

        private static async Task<PlayerBettingStatistics> ReadData(IObjectDb objectDb)
        {
            var result = new PlayerBettingStatistics();
            using (var reader = await objectDb.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    result.WonTicketCount = Convert.ToInt32(reader["Won"]);
                    result.LoseTicketCount = Convert.ToInt32(reader["Lose"]);
                    result.DrawTicketCount = Convert.ToInt32(reader["Draw"]);
                    result.CancelledTicketCount = Convert.ToInt32(reader["Cancelled"]);

                    if (reader.NextResult() && reader.Read())
                    {
                        result.Margin = Convert.ToDecimal(reader["Margin"]);
                        result.AverageBetSize = Convert.ToDecimal(reader["AverageBetSize"]);
                        result.Winnings = Convert.ToDecimal(reader["Winnings"]);
                        result.Loss = Convert.ToDecimal(reader["Loss"]);
                    }
                }
            }

            return result;
        }
    }
}