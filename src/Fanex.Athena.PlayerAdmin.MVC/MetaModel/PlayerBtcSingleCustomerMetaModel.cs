using System;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;

namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    public class PlayerBtcSingleCustomerMetaModel : IAgentMetaModel, IShowBetFromIPMetaModel, IProductCodeListMetaModel, IFunctionKeyMetaModel, IValidationMetaModel
    {
        private const int DefaultPageSize = 50;

        private string _baseCurrency = string.Empty;

        private string _userName;

        public PlayerBtcSingleCustomerMetaModel()
        {
            PageIndex = 1;
            PageSize = DefaultPageSize;
        }

        public string ProductCodeList { get; set; }

        public string OriginalProductCodeList { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string AgentListXml { get; set; }

        public bool IsExcludedTestData { get; set; }

        public string UserName
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

        public string Status { get; set; }

        public string LiveStatus { get; set; }

        public string SportListXML { get; set; }

        public string SportIds { get; set; }

        public string BetTypeIds { get; set; }

        public int CashOutMode { get; set; }

        public string Platform { get; set; }

        public bool IsExport { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool HasPlayerCom { get; set; }

        public string BaseCurrency
        {
            get
            {
                return _baseCurrency;
            }

            set
            {
                _baseCurrency = value == null
                    ? string.Empty
                    : value.ToLowerInvariant();
            }
        }

        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public bool IsAllowTxIDColumn { get; set; }
        public bool IsShowIp { get; set; }
        public bool IsShowIpVN { get; set; }

        public int SpecialPlatform { get; set; }

        public bool IsConvertAll { get; set; } = true;

        public bool IsValid => !string.IsNullOrEmpty(UserName);

        public string FunctionKey => "BettingTicketCenter";
    }
}