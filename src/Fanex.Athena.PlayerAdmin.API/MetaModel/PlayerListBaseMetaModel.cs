using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerListBaseMetaModel : ICurrencyIdListMetaModel, IAgentMetaModel, IDataTestableMetaModel, ISiteNameMetaModel
    {
        public string SiteName { get; set; }

        public string CurrencyIdList { get; set; }

        public string AgentListXml { get; set; }

        public bool IsExcludedTestData { get; set; }

        public string CustomerClass { get; set; }

        public string CommGroups { get; set; }

        public int IsDeposited { get; set; }
    }
}