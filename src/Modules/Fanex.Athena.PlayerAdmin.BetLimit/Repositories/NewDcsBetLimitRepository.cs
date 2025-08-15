namespace Fanex.Athena.PlayerAdmin.BetLimit.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Constants;
    using Core.Helpers;
    using Core.Models;
    using Core.Repositories;
    using DTO;
    using Interfaces;
    using MetaModel;
    using Microsoft.Extensions.Logging;

    public class NewDcsBetLimitRepository : BaseDcsBetLimitRepository, INewDcsBetLimitRepository
    {
        private static readonly IDictionary<int, string[]> ProductSportList = new Dictionary<int, string[]>
        {
            [ProductTypes.VS3] = new[] { "231" }
        };

        public NewDcsBetLimitRepository(IDbFactory dbFactory, ILogger<DcsBetLimitRepository> logger)
            : base(dbFactory, logger)
        {
        }

        public async Task<IEnumerable<PlayerBetLimit>> GetSettingAsync(int custId, int productId)
        {
            const string UspName = "Acc_Dep_CustSetting_NewBetLimitSettingSelect";
            return await ExecuteGetSettingQuery(UspName, custId, productId);
        }

        public async Task<IResultInfo> UpdateSettingAsync(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            const string UspName = "Acc_Dep_CustSetting_NewBetLimitUpd";
            return await ExecuteUpdateSettingQuery(UspName, metaModel);
        }

        protected override string BuildBetLimitParameter(int productId, IEnumerable<PlayerBetLimitMetaModel> setting)
        {
            var sports = ProductSportList[productId];
            var applySettings = setting.Where(e => sports.Contains(e.SportType));

            var elements = applySettings.Select(item => 
                XmlParameterBuilder.BuildNewBetLimitElement(item.SportType, item.BetType, item.MinBet, item.MaxBet));

            return XmlParameterBuilder.BuildRootXml(XmlParameterBuilder.CombineElements(elements));
        }
    }
}