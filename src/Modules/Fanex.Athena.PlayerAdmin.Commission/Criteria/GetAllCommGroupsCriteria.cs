using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    public class GetAllCommGroupsCriteria
    {
        public GetAllCommGroupsCriteria(BasePlayerCommissionUpdateQuery query)
        {
            LicenseeId = query.LicenseeId;
            Permissions = query.Permissions;
        }

        public int LicenseeId { get; set; }

        public IDictionary<string, bool> Permissions { get; set; }

        public string SPName => "Lrf_LicenseeGroupCommSel";

        public void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("LicenseeID", LicenseeId);
        }
    }
}
