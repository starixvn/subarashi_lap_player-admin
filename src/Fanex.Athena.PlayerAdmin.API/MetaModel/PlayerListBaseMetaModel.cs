namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    using Core.Models;
    using Fanex.Core.Utils;

    public class PlayerListBaseMetaModel : ICurrencyIdListMetaModel, IAgentMetaModel, ISiteNameMetaModel
    {
        public string SiteName { get; set; }

        public string OddsGroup { get; set; }

        public string SpecialLevel { get; set; }

        public string CurrencyIdList { get; set; }

        public string AgentListXml { get; set; }

        public string CustomerClass { get; set; }

        public string CommGroups { get; set; }

        public int IsDeposited { get; set; }
    }
}