using Fanex.Athena.PlayerAdmin.Core.Models;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    public class PlayerWinLossMetaModel
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

        public int CurrencyId => -1;

        public string CustomerClass { get; set; }

        public string OddsGroup { get; set; }    }
}