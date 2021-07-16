using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria
{
    public class UpdateBAStatusCriteria : BasePlayerBatchUpdateCriteria
    {
        public UpdateBAStatusCriteria(UpdateBAStatusApiMetaModel metaModel, string custId) : base(metaModel, custId)
        {
            BAStatusId = metaModel.BAStatusId;
        }

        public string BAStatusId { get; set; }

        public override string GetSPName() => "Lrf_CustomerUpdMemberBAStatus";

        public override void SetParameterValues(IObjectDb db)
        {
            base.SetParameterValues(db);
            db.SetParameterValue("LicenseeName", SiteName);
            db.SetParameterValue("ModifyBy", UserId);
            db.SetParameterValue("IsBA", BAStatusId == "1");
            db.SetParameterValue("OddsGroup", null);
        }
    }
}