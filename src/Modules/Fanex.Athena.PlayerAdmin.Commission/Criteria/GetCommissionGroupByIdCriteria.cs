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
        }

        public int LicenseeId { get; set; }

        public string GroupCommId { get; set; }

        public string SPName => "Lrf_GroupCommGetById";

        public void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("LicenseeID", LicenseeId);
            db.SetParameterValue("GroupDiscountID", GroupCommId);
        }
    }
}