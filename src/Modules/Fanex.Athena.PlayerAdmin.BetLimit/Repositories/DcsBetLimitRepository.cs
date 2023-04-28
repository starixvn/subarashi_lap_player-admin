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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class DcsBetLimitRepository : DCSBetLimitRepositoryBase, IDcsBetLimitRepository
    {
        private readonly IDbFactory _db;

        private readonly ILogger<DcsBetLimitRepository> _logger;

        private const int MemberRoleId = 1;

        private const int ProductStatusSettingType = 4;

        private static readonly IReadOnlyDictionary<int, string[]> ProductSportList = new Dictionary<int, string[]>
        {
            [ProductTypes.VirtualSports] = new[] { "180", "181", "182", "183", "184", "185", "186" },
            [ProductTypes.VirtualSports2] = new[] { "190", "191", "192", "193", "199" },
            [ProductTypes.Lottery] = new[] { "220" },
            [ProductTypes.Sportsbooks2] = new[] { "43" },
            [ProductTypes.TG] = new[] { "222" },
            [ProductTypes.I4D] = new[] { "223" },
            [ProductTypes.SLT] = new[] { "169" },
            [ProductTypes.AP] = new[] { "172" },
            [ProductTypes.APM] = new[] { "174" },
            [ProductTypes.BTC] = new[] { "175" }
        };

        private const string UpdateStatusXmlFormat = "<ROOT><ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" IsDisabledDownline=\"0\" /></ROOT>";

        private static readonly IReadOnlyDictionary<int, (int SportType, int BetType)> ProductMaping = new Dictionary<int, (int,int)>
        {
            { ProductTypes.CG, (167, 7301) },
            { ProductTypes.Exchange, (221, 8001) },
            { ProductTypes.SLT, (169, 3611) },
            { ProductTypes.I4D, (223, 5001) },
            { ProductTypes.AllBet, (211, 2401) },
            { ProductTypes.Voidbridge, (212, 4001) },
            { ProductTypes.PP, (248, 4101) },
            { ProductTypes.SXG, (243, 3101) },
            { ProductTypes.HB, (238, 7001) },
            { ProductTypes.JK, (234, 6401) },
            { ProductTypes.SAG, (244, 3001) },
            { ProductTypes.PGS, (232, 6001) },
            { ProductTypes.WM, (233, 3401) },
            { ProductTypes.SG, (247, 4301) },
            { ProductTypes.VRG, (245, 4601) },
            { ProductTypes.CC, (165, 1901) },
            { ProductTypes.SBC, (200, 1611) },
            { ProductTypes.Lottery, (220, 1021) },
            { ProductTypes.TG, (222, 4901) },
            { ProductTypes.VGA, (171, 3801) },
            { ProductTypes.AP, (172, 7701) },
            { ProductTypes.APM, (174, 7801) },
            { ProductTypes.SabaCasino, (208, 2101) },
            { ProductTypes.BTC, (175, 7901) },
            { ProductTypes.FGGL, (176, 8201) },
            { ProductTypes.MXG, (237, 5401) },
            { ProductTypes.JIG, (158, 5100) }
        };

        public DcsBetLimitRepository(IDbFactory dbFactory, ILogger<DcsBetLimitRepository> logger)
        {
            _db = dbFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_BetLimitSettingSelect";
            var dataList = new List<PlayerBetLimit>();
            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("productid", productId);
                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var playerBetLimit = new PlayerBetLimit();
                        ReadCommonField(reader, playerBetLimit);
                        playerBetLimit.MaxPerMatch = reader.ToDecimal("maxpermatch");
                        playerBetLimit.MaxPerMatchMax = reader.ToDecimal("maxpermatchmax");
                        dataList.Add(playerBetLimit);
                    }
                }
            }

            return dataList;
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_BetLimitUpd";

            using (var db = _db.GetObjectDb(UspName))
            {
                AddUpdateParameter(db, metaModel);
                await db.ExecuteNonQueryAsync().ConfigureAwait(false);
                return GetActionResult(db);
            }
        }

        public async Task<bool> GetStatusAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_StatusSettingSelect";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("ProductId", productId);
                db.SetParameterValue("SettingType", ProductStatusSettingType);
                db.SetParameterValue("RoleID", MemberRoleId);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return reader.ToInt("IsDisabled") == 0;
                    }
                }
            }

            return false;
        }

        public async Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_DisableStatUpd";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("userid", metaModel.UserId);
                db.SetParameterValue("IsDisabled", BuildProductStatusXml(metaModel.ProductId, metaModel.Status));
                db.SetParameterValue("ip", metaModel.Ip);
                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync();

                return GetActionResult(db);
            }
        }

        protected override string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            const string XMLPattern = "<ROOT>{0}</ROOT>";
            const string XMLElement = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" ></BetLimit>";

            var xmlElementBuilder = new StringBuilder();

            var isHasValue = ProductSportList.TryGetValue(productId, out var sports);

            foreach (var item in setting)
            {
                if (!isHasValue || sports.Contains(item.SportType))
                {
                    xmlElementBuilder.Append(string.Format(XMLElement, item.SportType, item.BetType, item.MinBet, item.MaxBet, item.MaxPerMatch));
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

        private static string BuildProductStatusXml(int productId, bool status)
           => string.Format(UpdateStatusXmlFormat, ProductMaping[productId].SportType, ProductMaping[productId].BetType, status ? 0 : 1);
    }
}