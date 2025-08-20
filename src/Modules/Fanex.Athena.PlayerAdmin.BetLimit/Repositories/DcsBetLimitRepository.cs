namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Constants;
    using Core.Extensions;
    using Core.Helpers;
    using Core.Models;
    using Core.Repositories;
    using DTO;
    using Interfaces;
    using MetaModel;
    using Microsoft.Extensions.Logging;

    public class DcsBetLimitRepository : BaseDcsBetLimitRepository, IDcsBetLimitRepository
    {
        private const int MemberRoleId = 1;

        private const int ProductStatusSettingType = 4;

        private const string UpdateStatusXmlFormat =
            "<ROOT><ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" IsDisabledDownline=\"0\" /></ROOT>";

        private static readonly IDictionary<int, string[]> ProductSportList = new Dictionary<int, string[]>
        {
            [ProductTypes.VirtualSports] = new[] { "9908" },
            [ProductTypes.Lottery] = new[] { "220" },
            [ProductTypes.Sportsbooks2] = new[] { "43" },
            [ProductTypes.TG] = new[] { "222" },
            [ProductTypes.I4D] = new[] { "223" },
            [ProductTypes.SLT] = new[] { "169" },
            [ProductTypes.AP] = new[] { "172" },
            [ProductTypes.APM] = new[] { "174" },
            [ProductTypes.BTC] = new[] { "175" }
        };

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
            { ProductTypes.MXG, 237 },
            { ProductTypes.JIG, 158 },
            { ProductTypes.YB, 159 },
            { ProductTypes.MGRNG, 155 },
            { ProductTypes.SEA, 156 },
            { ProductTypes.WE, 228 },
            { ProductTypes.L22, 227 },
            { ProductTypes.UUSL, 216 },
            { ProductTypes.FS, 230 },
            { ProductTypes.FAC, 146 },
            { ProductTypes.NS, 229 },
            { ProductTypes.ON, 138 },
            { ProductTypes.PTRNG, 148 },
            { ProductTypes.GPIRNG, 145 },
            { ProductTypes.GPILC, 144 },
            { ProductTypes.PS, 143 },
            { ProductTypes.ASK, 130 },
            { ProductTypes.FG, 131 },
            { ProductTypes.HOT, 139 },
            { ProductTypes.MGLC, 124 },
            { ProductTypes.PTLC, 129 },
            { ProductTypes.RGRNG, 123 },
            { ProductTypes.WLC, 136 },
            { ProductTypes.CPRNG, 122 },
            { ProductTypes.AIG, 121 },
            { ProductTypes.GPILT, 132 },
            { ProductTypes.ODG, 128 }
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
            { ProductTypes.MXG, 5401 },
            { ProductTypes.JIG, 5100 },
            { ProductTypes.YB, 3151 },
            { ProductTypes.MGRNG, 8400 },
            { ProductTypes.SEA, 8900 },
            { ProductTypes.WE, 10300 },
            { ProductTypes.L22, 7601 },
            { ProductTypes.UUSL, 10000 },
            { ProductTypes.FS, 11100 },
            { ProductTypes.FAC, 11300 },
            { ProductTypes.NS, 10450 },
            { ProductTypes.ON, 11850 },
            { ProductTypes.PTRNG, 12500 },
            { ProductTypes.GPIRNG, 11400 },
            { ProductTypes.GPILC, 11500 },
            { ProductTypes.PS, 11600  },
            { ProductTypes.ASK, 15000 },
            { ProductTypes.FG, 15200 },
            { ProductTypes.HOT, 11900 },
            { ProductTypes.MGLC, 13900 },
            { ProductTypes.PTLC, 15600 },
            { ProductTypes.RGRNG, 15800 },
            { ProductTypes.WLC, 12000 },
            { ProductTypes.CPRNG, 16100 },
            { ProductTypes.AIG, 16200 },
            { ProductTypes.GPILT, 13800  },
            { ProductTypes.ODG, 15400  }
        };

        public DcsBetLimitRepository(IDbFactory dbFactory, ILogger<DcsBetLimitRepository> logger)
            : base(dbFactory, logger)
        {
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_BetLimitSettingSelect";
            return await ExecuteGetSettingQuery(UspName, custId, productId);
        }

        protected override PlayerBetLimit CreatePlayerBetLimitFromReader(IDataReader reader)
        {
            return new PlayerBetLimit
            {
                MinBet = reader.ToDecimal("minbet"),
                BetValueMin = reader.ToDecimal("minbetmin"),
                MaxBet = reader.ToDecimal("maxbet"),
                BetValueMax = reader.ToDecimal("maxbetmax"),
                MaxPerMatch = reader.ToDecimal("maxpermatch"),
                MaxPerMatchMax = reader.ToDecimal("maxpermatchmax"),
                SportType = reader.ToString("sporttype"),
                BetType = reader.ToInt("bettype")
            };
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_BetLimitUpd";
            return await ExecuteUpdateSettingQuery(UspName, metaModel);
        }

        public async Task<PlayerBetLimitProductSetting> GetStatusAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_StatusSettingSelect";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("ProductId", productId);
                db.SetParameterValue("SettingType", ProductStatusSettingType);
                db.SetParameterValue("RoleID", MemberRoleId);

                using var reader = await db.ExecuteReaderAsync();

                if (reader.Read())
                {
                    return new PlayerBetLimitProductSetting
                    {
                        SportType = reader.ToInt("SportType"),
                        BetType = reader.ToInt("BetType"),
                        IsDisabled = reader.ToInt("IsDisabled") == 0,
                    };
                }
            }

            return new PlayerBetLimitProductSetting();
        }

        public async Task<IResultInfo> UpdateStatusAsync(PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_DisableStatUpd";

            using (var db = _db.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("userid", metaModel.UserId);
                db.SetParameterValue("IsDisabled",
                    BuildProductStatusXml(metaModel.ProductId, metaModel.Status, metaModel.SportType, metaModel.BetType));
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

        protected override string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            var isHasValue = ProductSportList.TryGetValue(productId, out var sports);

            var filteredSettings = setting.Where(item => !isHasValue || sports.Contains(item.SportType));
            
            var elements = filteredSettings.Select(item => 
                XmlParameterBuilder.BuildBetLimitElement(item.SportType, item.BetType, item.MinBet, item.MaxBet, item.MaxPerMatch));

            return XmlParameterBuilder.BuildRootXml(XmlParameterBuilder.CombineElements(elements));
        }

        private static string BuildProductStatusXml(int productId, bool status, int sportType, int betType)
        {
            var finalSportType = ProductSportTypeMap.TryGetValue(productId, out var mappedSportType)
                ? mappedSportType
                : sportType;

            var finalBetType = ProductBetTypeMap.TryGetValue(productId, out var mappedBetType)
                ? mappedBetType
                : betType;

            return string.Format(UpdateStatusXmlFormat, finalSportType, finalBetType, status ? 0 : 1);
        }
    }
}