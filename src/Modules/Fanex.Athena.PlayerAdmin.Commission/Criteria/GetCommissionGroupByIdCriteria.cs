using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    public class GetCommissionGroupByIdCriteria
    {
        public GetCommissionGroupByIdCriteria(MultipleMoveGroupQuery query)
        {
            LicenseeId = query.LicenseeId;
            GroupCommId = query.GroupCommId;
            Permissions = query.Permissions;
        }

        public int LicenseeId { get; set; }

        public string GroupCommId { get; set; }

        public IDictionary<string, bool> Permissions { get; set; }

        public string GetSPName() => "Lrf_GroupCommGetById";

        public void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("LicenseeID", LicenseeId);
            db.SetParameterValue("GroupCommID", GroupCommId);
        }
    }
}
