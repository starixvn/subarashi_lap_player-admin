namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    using Data;
    using MetaModels;

    public class MultipleMoveGroupCriteria : BasePlayerCommissionUpdateCriteria
    {
        public MultipleMoveGroupCriteria(MultipleMoveGroupQuery query) : base(query)
        {
            GroupCommId = query.GroupCommId;
        }

        public string GroupCommId { get; set; }

        public override string GetSPName() => "Lrf_CustomerMoveGroupComm";

        public override void SetParameterValues(IObjectDb db)
        {
            base.SetParameterValues(db);

            db.SetParameterValue("GroupDiscountID", GroupCommId);
        }
    }
}