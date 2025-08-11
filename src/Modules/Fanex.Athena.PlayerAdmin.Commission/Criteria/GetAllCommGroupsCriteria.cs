namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    using Data;
    using MetaModels;

    public class GetAllCommGroupsCriteria
    {
        public GetAllCommGroupsCriteria(BasePlayerCommissionUpdateQuery query)
        {
            LicenseeId = query.LicenseeId;
        }

        public int LicenseeId { get; set; }

        public string SPName => "Lrf_LicenseeGroupCommSel";

        public void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("LicenseeID", LicenseeId);
        }
    }
}