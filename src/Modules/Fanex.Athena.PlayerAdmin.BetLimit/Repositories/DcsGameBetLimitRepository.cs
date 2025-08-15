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

    public class DcsGameBetLimitRepository : BaseDcsBetLimitRepository, IDcsGameBetLimitRepository
    {
        private static readonly IDictionary<int, (int ProductId, int SettingType)> MappingProducts = new Dictionary<int, (int, int)>
        {
            {ProductTypes.Sportsbook, (1, 10) },
            {ProductTypes.NumberGame, (3, 12) }
        };

        public DcsGameBetLimitRepository(IDbFactory dbFactory, ILogger<DcsBetLimitRepository> logger)
            : base(dbFactory, logger)
        {
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_Game_BetLimitSettingSelect";
            var mappingProduct = MappingProducts[productId];
            return await ExecuteGetSettingQuery(UspName, custId, mappingProduct.ProductId, mappingProduct.SettingType);
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
                MaxPayoutPerMatch = reader.ToDecimal("maxpayoutpermatch"),
                MaxPayoutPerMatchMax = reader.ToDecimal("maxpayoutpermatchmax"),
                MaxPerBall = reader.ToDecimal("maxpernumber"),
                MaxPerBallMax = reader.ToDecimal("maxpernumbermax"),
                SportType = reader.ToString("sporttype"),
                BetType = reader.ToInt("bettype")
            };
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_Game_BetLimitUpd";
            return await ExecuteUpdateSettingQuery(UspName, metaModel);
        }

        protected override string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            var elements = setting.Select(item => productId == ProductTypes.Sportsbook
                ? XmlParameterBuilder.BuildGameBetLimitElement(item.SportType, item.BetType, item.MinBet, item.MaxBet, item.MaxPerMatch, item.MaxPayoutPerMatch)
                : XmlParameterBuilder.BuildNumberGameBetLimitElement(item.SportType, item.BetType, item.MinBet, item.MaxBet, item.MaxPerMatch, item.MaxPerBall));

            return XmlParameterBuilder.BuildRootXml(XmlParameterBuilder.CombineElements(elements));
        }
    }
}