using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    public class DcsBetLimitRepository : IDcsBetLimitRepository
    {
        private readonly IDbFactory _db;

        private readonly ILogger<DcsBetLimitRepository> _logger;

        private const int MemberRoleId = 1;

        private const int ProductStatusSettingType = 4;

        private static readonly IDictionary<int, string[]> ProductSportList = new Dictionary<int, string[]>
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

        private static readonly IDictionary<int, int> ProductSportTypeMap = new Dictionary<int, int>
        {
            { ProductTypes.CG, 167 },
            { ProductTypes.Exchange, 221 },
            { ProductTypes.SLT, 169 },
            { ProductTypes.I4D, 223 },
            { ProductTypes.AllBet, 211 },
            { ProductTypes.Voidbridge, 212 },
            { ProductTypes.PP, 248 },
            { ProductTypes.SXG, 243 },
            { ProductTypes.GDG, 236 },
            { ProductTypes.HB, 238 },
            { ProductTypes.JK, 234 },
            { ProductTypes.SAG, 244 },
            { ProductTypes.PGS, 232 },
            { ProductTypes.WM, 233 },
            { ProductTypes.SG, 247 },
            { ProductTypes.VRG, 245 },
            { ProductTypes.CC, 165 },
            { ProductTypes.SBC, 200 },
            { ProductTypes.Lottery, 220 },
            { ProductTypes.TG, 222 },
            { ProductTypes.VGA, 171 },
            { ProductTypes.AP, 172 },
            { ProductTypes.APM, 174 },
            { ProductTypes.SabaCasino, 208 },
            { ProductTypes.BTC, 175 },
            { ProductTypes.FGGL, 176 },
            { ProductTypes.MXG, 237 }
        };

        private static readonly IDictionary<int, int> ProductBetTypeMap = new Dictionary<int, int>
        {
            { ProductTypes.CG, 7301 },
            { ProductTypes.Exchange, 8001 },
            { ProductTypes.SLT, 3611 },
            { ProductTypes.I4D, 5001 },
            { ProductTypes.AllBet, 2401 },
            { ProductTypes.Voidbridge, 4001 },
            { ProductTypes.PP, 4101 },
            { ProductTypes.SXG, 3101 },
            { ProductTypes.GDG, 5701 },
            { ProductTypes.HB, 7001 },
            { ProductTypes.JK, 6401 },
            { ProductTypes.SAG, 3001 },
            { ProductTypes.PGS, 6001 },
            { ProductTypes.WM, 3401 },
            { ProductTypes.SG, 4301 },
            { ProductTypes.VRG, 4601 },
            { ProductTypes.CC, 1901 },
            { ProductTypes.SBC, 1611 },
            { ProductTypes.Lottery, 1021 },
            { ProductTypes.TG, 4901 },
            { ProductTypes.VGA, 3801 },
            { ProductTypes.AP, 7701 },
            { ProductTypes.APM, 7801 },
            { ProductTypes.SabaCasino, 2101 },
            { ProductTypes.BTC, 7901 },
            { ProductTypes.FGGL, 8201 },
            { ProductTypes.MXG, 5401 }
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
                        dataList.Add(new PlayerBetLimit
                        {
                            MinBet = reader.ToDecimal("minbet"),
                            BetValueMin = reader.ToDecimal("minbetmin"),
                            MaxBet = reader.ToDecimal("maxbet"),
                            BetValueMax = reader.ToDecimal("maxbetmax"),
                            MaxPerMatch = reader.ToDecimal("maxpermatch"),
                            MaxPerMatchMax = reader.ToDecimal("maxpermatchmax"),
                            Id = reader.ToString("sporttype"),
                            BetType = reader.ToInt("bettype")
                        });
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

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = GetErrorMessage(Converter.ToString(errorMessage))
                };
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
            const string XMLElement = "<BetLimit SportType=\"{0}\" BetType=\"{1}\" MinBet=\"{2}\" MaxBet=\"{3}\" MaxPerMatch=\"{4}\" ></BetLimit>";

            var xmlElementBuilder = new StringBuilder();
            var sports = ProductSportList[productId];

            foreach (var item in setting)
            {
                if (sports.Contains(item.Id))
                {
                    xmlElementBuilder.Append(string.Format(XMLElement, item.Id, item.BetType, item.MinBet, item.MaxBet, item.MaxPerMatch));
                }
            }

            return string.Format(XMLPattern, xmlElementBuilder);
        }

        private string GetErrorMessage(string xmlErrorMessage)
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
           => string.Format(UpdateStatusXmlFormat, ProductSportTypeMap[productId], ProductBetTypeMap[productId], status ? 0 : 1);
    }
}