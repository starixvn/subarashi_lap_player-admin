using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria
{
    public class UpdateSpecialLevelCriteria : BasePlayerBatchUpdateCriteria
    {
        public UpdateSpecialLevelCriteria(UpdateSpecialLevelApiMetaModel metaModel, string custId)
            : base(metaModel, custId)
        {
            SpecialLevelId = metaModel.SpecialLevelId;
        }

        public string SpecialLevelId { get; set; }

        public override string GetSPName() => "Lrf_CustomerUpdSpecialLevel";

        public override void SetParameterValues(IObjectDb db)
        {
            base.SetParameterValues(db);
            db.SetParameterValue("UserId", UserId);
            db.SetParameterValue("SpecialLevel", SpecialLevelId);
        }
    }
}