namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Core.Constants;
    using Core.Extensions;
    using Core.Models;
    using Core.Repositories;
    using Data;
    using DTO;
    using Interfaces;
    using MetaModel;
    using Microsoft.Extensions.Logging;

    public class NewDcsBetLimitRepository : INewDcsBetLimitRepository
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

            var dataList = new List<PlayerBetLimit>();

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("productid", productId);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new PlayerBetLimit
                        {
                            MinBet = reader.ToDecimal("minbet"),
                            BetValueMin = reader.ToDecimal("minbetmin"),
                            MaxBet = reader.ToDecimal("maxbet"),
                            BetValueMax = reader.ToDecimal("maxbetmax"),
                            SportType = reader.ToString("sporttype"),
                            BetType = reader.ToInt("bettype")
                        });
                    }
                }
            }

            return dataList;
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_NewBetLimitUpd";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddUpdateParameter(db, metaModel);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = GetErrorMessage(Converter.ToString(errorMessage))
                };
            }
        }

        private static void AddUpdateParameter(IObjectDb db, PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            db.SetParameterValue("custid", metaModel.CustId);
            db.SetParameterValue("userid", metaModel.UserId);
            db.SetParameterValue("ip", metaModel.Ip);
            db.SetParameterValue("betlimit", BuildBetLimitParameter(metaModel.ProductId, metaModel.Setting));
            db.SetParameterValue("errcode", null);
            db.SetParameterValue("errmsg", null);
        }

        private static string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            const string XMLPattern = "<ROOT>{0}</ROOT>";
            const string XMLElement = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" ></BetLimit>";

            var xmlElementBuilder = new StringBuilder();
            var sports = ProductSportList[productId];
            var applySettings = setting.Where(e => sports.Contains(e.SportType));

            foreach (var item in applySettings)
            {
                xmlElementBuilder.Append(string.Format(XMLElement, item.SportType, item.BetType, item.MinBet, item.MaxBet));
            }

            return string.Format(XMLPattern, xmlElementBuilder);
        }

        private string GetErrorMessage(string xmlErrorMessage)
        {
            if (!string.IsNullOrEmpty(xmlErrorMessage) && xmlErrorMessage.StartsWith('<'))
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