using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria
{
    public class UpdateOddsGroupCriteria : BasePlayerBatchUpdateCriteria
    {
        public UpdateOddsGroupCriteria(UpdateOddsGroupApiMetaModel metaModel, string custId)
            : base(metaModel, custId)
        {
            OddsGroupId = metaModel.OddsGroupId;
        }

        public string OddsGroupId { get; set; }

        public override string GetSPName() => "Lrf_CustomerUpdUgroup";

        public override void SetParameterValues(IObjectDb db)
        {
            base.SetParameterValues(db);
            db.SetParameterValue("UserId", UserId);
            db.SetParameterValue("OddsGroup", OddsGroupId);
        }
    }
}