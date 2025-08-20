namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Resources;
    using MetaModels;

    public class SingleMoveGroupViewModel
    {
        public SingleMoveGroupViewModel(
              SingleMoveGroupQuery query,
              IEnumerable<PlayerCommissionViewModel> groups,
              PlayerCommissionViewModel currentGroup,
              bool hasCommPermissionWrite)
        {
            CustId = query.CustId;
            Groups = groups.ToList();
            CurrentGroup = currentGroup;
            HasCommPermissionWrite = hasCommPermissionWrite;
            InsertGroupNone();
        }

        public int CustId { get; set; }

        public PlayerCommissionViewModel CurrentGroup { get; set; }

        public IList<PlayerCommissionViewModel> Groups { get; set; }

        public bool HasCommPermissionWrite { get; set; }

        private void InsertGroupNone()
        {
            if (CurrentGroup.GroupCommId != 0)
            {
                return;
            }

            Groups.Insert(0, new PlayerCommissionViewModel
            {
                GroupCommId = 0,
                GroupName = ViewResources.None,
                CommValues = CurrentGroup.CommValues
            });
        }
    }
}