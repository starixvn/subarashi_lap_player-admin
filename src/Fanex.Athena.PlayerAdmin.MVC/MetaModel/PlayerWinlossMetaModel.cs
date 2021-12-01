using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    public class PlayerWinLossMetaModel : IAgentMetaModel, IValidationMetaModel, IProductOptionsMetaModel, IFunctionKeyMetaModel
    {
        private const int MinimumPageSize = 50;

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

        public IEnumerable<ProductOption> ProductOptions { get; set; }

        public IEnumerable<ProductOption> OriginalProductOptions { get; set; }

        public string AgentListXml { get; set; }

        public bool IsExportExcel { get; set; }

        public bool IsAllowedBA { get; set; }

        public bool HasComm { get; set; }

        public bool SystemIdVisible { get; set; }

        public string ExcelTitle { get; set; }

        public bool IsValid => !string.IsNullOrEmpty(Username);

        public bool IsExcludedTestData { get; set; }

        public string FunctionKey => "CustomerWinLoss";
    }
}