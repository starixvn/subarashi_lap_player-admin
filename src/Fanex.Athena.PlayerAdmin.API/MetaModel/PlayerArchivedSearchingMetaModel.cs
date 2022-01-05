using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerArchivedSearchingMetaModel : ICurrencyIdListMetaModel, IAgentMetaModel, ISubsidiarySearchMetaModel
    {
        public byte SearchOption { get; set; }
        public string UserName { get; set; }
        public string SubsidiaryList { get; set; }
        public bool SubsidiarySearch { get; set; }
        public string CurrencyIdList { get; set; } = string.Empty; //All Currency
        public string AgentListXml { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
