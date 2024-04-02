using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class PlayerBetLimitRepository : IPlayerBetLimitRepository
    {
        private readonly IDbFactory _dbFactory;
        private static readonly IList<string> SettingStatusList = new List<string> { "LOCAL", "SIT", "UAT" };

        public PlayerBetLimitRepository(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<IEnumerable<PlayerBetLimitProduct>> GetEnableBetLimitProducts(string siteName, string environment)
        {
            const string UspName = "Lrf_LicenseeProductByBetLimitSel";
            var products = new List<PlayerBetLimitProduct>();

            using (var db = _dbFactory.GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeName", siteName);
                db.SetParameterValue("SettingStatus", Convert.ToInt32(SettingStatusList.Contains(environment)));

                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (Convert.ToBoolean(reader["IsEnable"]))
                        {
                            products.Add(new PlayerBetLimitProduct
                            {
                                ProductId = reader.ToInt("ProductID"),
                                ProductCode = reader.ToString("ProductCode")
                            });
                        }
                    }
                }
            }

            return products;
        }
    }
}