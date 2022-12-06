using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class NewDcsBetLimitRepository : DCSBetLimitRepositoryBase, INewDcsBetLimitRepository
    {
        private readonly IDbFactory _db;

        private readonly ILogger<DcsBetLimitRepository> _logger;

        private static readonly IDictionary<int, string[]> ProductSportList = new Dictionary<int, string[]>
        {
            [ProductTypes.VS3] = new[] { "231" }
        };

        public NewDcsBetLimitRepository(IDbFactory dbFactory, ILogger<DcsBetLimitRepository> logger)
        {
            _db = dbFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_NewBetLimitSettingSelect";
            using (var db = _db.GetObjectDb(UspName))
            {
                var dataList = new List<PlayerBetLimit>();

                db.SetParameterValue("custid", custId);
                db.SetParameterValue("productid", productId);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var playerBetLimit = new PlayerBetLimit();
                        ReadCommonField(reader, playerBetLimit);
                        dataList.Add(playerBetLimit);
                    }
                }
                return dataList;
            }
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_NewBetLimitUpd";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddUpdateParameter(db, metaModel);

                await db.ExecuteNonQueryAsync();
                return GetActionResult(db);
            }
        }

        protected override string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            const string XMLPattern = "<ROOT>{0}</ROOT>";
            const string XMLElement = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" ></BetLimit>";

            var xmlElementBuilder = new StringBuilder();
            var sports = ProductSportList[productId];

            foreach (var item in setting)
            {
                if (sports.Contains(item.SportType))
                {
                    xmlElementBuilder.Append(string.Format(XMLElement, item.SportType, item.BetType, item.MinBet, item.MaxBet));
                }
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
    }
}