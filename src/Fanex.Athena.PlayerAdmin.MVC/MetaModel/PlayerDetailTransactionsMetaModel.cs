using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Core.Utils;
using Fanex.Data;
using System;

namespace Fanex.Athena.PlayerAdmin.MVC.MetaModel
{
    public class PlayerDetailTransactionsMetaModel : ICustomerIdentityMetalModel, IValidationMetaModel
    {
        public string SPName => "LAP_Rpt_DetailTransaction_Mem";

        private const int MaximumMonth = 3;

        public int CustId { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int CurrencyId { get; set; }

        public int? PageSize { get; set; }

        public int? PageIndex { get; set; }

        public string TransactionId { get; set; }

        public bool IsResettled { get; set; }

        public DateTime MinDate => DateTime.Now.AddMonths(-MaximumMonth);

        public bool IsValid => ToDate >= FromDate
            && FromDate >= ToDate.AddMonths(-MaximumMonth);

        public void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("CustId", CustId);
            db.SetParameterValue("FromDate", FromDate);
            db.SetParameterValue("ToDate", ToDate);
            db.SetParameterValue("PageSize", PageSize);
            db.SetParameterValue("PageIndex", PageIndex);
            db.SetParameterValue("TotalRow", 0);

            db.SetParameterValue("TransactionId", TransactionId);
            db.SetParameterValue("IsResettled", IsResettled);
        }
    }
}