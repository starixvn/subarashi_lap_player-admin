using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;
using System;

namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    public class PlayerRunningBetListMetaModel : ICustomerIdentityMetaModel, IAgentMetaModel, IShowBetFromIPMetaModel
    {
        private const int PageSizeMaximum = 2000;

        private int _pageSize;

        private string _sortBy;

        private string _sortOrder;

        public int CustId { get; set; }

        public string SortBy
        {
            get { return _sortBy; }
            set
            {
                _sortBy = !string.IsNullOrWhiteSpace(value) && value.Equals("stake", StringComparison.InvariantCultureIgnoreCase)
                    ? value
                    : null;
            }
        }

        public string SortOrder
        {
            get { return _sortOrder; }
            set
            {
                var normalizedValue = value?.Trim();
                _sortOrder = string.Equals(normalizedValue, "asc", StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(normalizedValue, "desc", StringComparison.InvariantCultureIgnoreCase) ?
                    normalizedValue : null;
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value > PageSizeMaximum ? PageSizeMaximum : value;
            }
        }

        public string Currency { get; set; }

        public int CurrencyId { get; set; }

        public string AgentListXml { get; set; }

        public bool IsExcludedTestData { get; set; }

        public int PageIndex { get; set; }

        public bool IsExport { get; set; }

        public bool IsAllowTxIDColumn { get; set; }

        public int SpecialPlatform { get; set; }

        public bool IsShowIp { get; set; }
        public bool IsShowIpVN { get; set; }
    }
}