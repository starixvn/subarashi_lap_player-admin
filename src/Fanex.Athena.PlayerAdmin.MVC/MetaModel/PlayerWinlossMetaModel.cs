using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using System;

namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    public class PlayerWinLossMetaModel
    {
        private const int MinimumPageSize = 100;

        private string _userName;

        public string Username
        {
            get
            {
                return _userName;
            }

            set
            {
                _userName = value?.Trim();
            }
        }

        public int PageSize { get; set; } = MinimumPageSize;

        public int PageIndex { get; set; } = 1;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CustomerClass { get; set; }

        public string OddsGroup { get; set; }

        public GroupByMode GroupMode { get; set; }

        public BreakdownMode BreakdownMode { get; set; }

        public string ProductCodes { get; set; }

        public string SportIds { get; set; }

        public string BetTypeIds { get; set; }

        public string PlatformIds { get; set; }

        public string StatusIds { get; set; }

        public string LiveStatusIds { get; set; }

        public string BaseCurrency { get; set; }

        public bool IsVipOnly { get; set; }

        public string SearchUserName { get; set; }

        public int SearchMode { get; set; }

        public bool IsExcludedTestData { get; set; } = true;

        public bool IsViewCashOutOnly { get; set; } 

        public bool SubsidiarySearch { get; set; }

        public string CurrencyIdList { get; set; }

        public string CurrencyCodes { get; set; }

        public int SortBy { get; set; }

        public int SortOrder { get; set; }

        public bool IsShowInOrginalCurrency { get; set; }
    }
}