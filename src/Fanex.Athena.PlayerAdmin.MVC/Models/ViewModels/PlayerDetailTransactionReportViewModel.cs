using System;

namespace Fanex.Athena.PlayerAdmin.MVC.Models
{
    public class PlayerDetailTransactionReportViewModel
    {
        private const int MaximumMonth = 3;
        public const int DefaultPageSize = 500;
        public const int DefaultPageIndex = 1;

        public PlayerDetailTransactionReportViewModel()
        {
            var today = DateTime.Today;
            MinDate = DateTime.Now.AddMonths(-MaximumMonth);
            FromDate = new DateTime(today.Year, today.Month, 1);
            ToDate = today;
            PageSize = DefaultPageSize;
            PageIndex = DefaultPageIndex;
        }

        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRow { get; set; }

        public string FromDateStr => FromDate.ToString("MM/dd/yyyy");

        public string ToDateStr => ToDate.ToString("MM/dd/yyyy");
    }
}