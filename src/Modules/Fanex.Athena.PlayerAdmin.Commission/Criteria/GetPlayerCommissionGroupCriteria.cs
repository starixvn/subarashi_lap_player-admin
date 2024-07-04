using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    public class GetPlayerCommissionGroupCriteria
    {
        public GetPlayerCommissionGroupCriteria(SingleMoveGroupQuery query)
        {
            CustId = query.CustId;
        }

        public int CustId { get; set; }

        public string SPName => "Lrf_LicenseeCustCommSel";

        public void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("CustId", CustId);
        }
    }
}