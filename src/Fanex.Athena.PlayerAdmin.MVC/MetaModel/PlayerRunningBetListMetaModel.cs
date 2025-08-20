namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    using Core.Models;
    using Fanex.Core.Utils;

    public class PlayerRunningBetListMetaModel : ICustomerIdentityMetaModel, IAgentMetaModel, IShowBetFromIPMetaModel
    {
        private const int PageSizeMaximum = 2000;

        private int _pageSize;

        public int CustId { get; set; }

        public byte SortType { get; set; }

        public byte SortOrder { get; set; } = 1;

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