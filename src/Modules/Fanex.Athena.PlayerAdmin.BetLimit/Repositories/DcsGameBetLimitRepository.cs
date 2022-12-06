using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class DcsGameBetLimitRepository : DCSBetLimitRepositoryBase, IDcsGameBetLimitRepository
    {
        private readonly IDbFactory _db;

        private readonly ILogger<DcsBetLimitRepository> _logger;

        private static readonly IDictionary<int, (int ProductId, int SettingType)> MappingProducts = new Dictionary<int, (int, int)>
        {
            {ProductTypes.Sportsbook, (1, 10) },
            {ProductTypes.NumberGame, (3, 12) }
        };

        public DcsGameBetLimitRepository(IDbFactory dbFactory, ILogger<DcsBetLimitRepository> logger)
        {
            _db = dbFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_Game_BetLimitSettingSelect";
            var mappingProduct = MappingProducts[productId];

            var dataList = new List<PlayerBetLimit>();

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("productid", mappingProduct.ProductId);
                db.SetParameterValue("settingtype", mappingProduct.SettingType);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var playerBetLimit = new PlayerBetLimit();
                        ReadCommonField(reader, playerBetLimit);
                        ReadDCSGameField(reader, playerBetLimit);
                    }
                }
            }
            return dataList;
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_Game_BetLimitUpd";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddUpdateParameter(db, metaModel);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);
                return GetActionResult(db);
            }
        }

        protected override string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            const string XMLPattern = "<ROOT>{0}</ROOT>";
            string XMLElement = productId == ProductTypes.Sportsbook
                ? "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" MaxPayoutPerMatch=\"{5}\"></BetLimit>"
                : "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" MaxPerNumber=\"{5}\"></BetLimit>";

            var xmlElementBuilder = new StringBuilder();
            foreach (var item in setting)
            {
                xmlElementBuilder.Append(productId == ProductTypes.Sportsbook
                    ? string.Format(XMLElement, item.SportType, item.BetType, item.MinBet, item.MaxBet, item.MaxPerMatch, item.MaxPayoutPerMatch)
                    : string.Format(XMLElement, item.SportType, item.BetType, item.MinBet, item.MaxBet, item.MaxPerMatch, item.MaxPerBall)
                );
            }

            return string.Format(XMLPattern, xmlElementBuilder);
        }

        protected override string GetErrorMessage(string xmlErrorMessage)
        {
            if (!string.IsNullOrEmpty(xmlErrorMessage) && xmlErrorMessage.StartsWith("<"))
            {
                try
                {
                    var doc = XDocument.Parse(xmlErrorMessage);
                    return doc.Root.Attribute("Msg").Value;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Invalid xml content: " + xmlErrorMessage);
                }
            }

            return xmlErrorMessage;
        }
        private static void ReadDCSGameField(IDataReader reader, PlayerBetLimit playerBetLimit)
        {
            playerBetLimit.MaxPerMatch = reader.ToDecimal("maxpermatch");
            playerBetLimit.MaxPerMatchMax = reader.ToDecimal("maxpermatchmax");
            playerBetLimit.MaxPayoutPerMatch = reader.ToDecimal("maxpayoutpermatch");
            playerBetLimit.MaxPayoutPerMatchMax = reader.ToDecimal("maxpayoutpermatchmax");
            playerBetLimit.MaxPerBall = reader.ToDecimal("maxpernumber");
            playerBetLimit.MaxPerBallMax = reader.ToDecimal("maxpernumbermax");
        }
    }
}
