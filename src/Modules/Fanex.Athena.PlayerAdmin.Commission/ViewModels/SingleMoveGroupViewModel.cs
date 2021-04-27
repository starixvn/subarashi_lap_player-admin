using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;

namespace Fanex.Athena.PlayerAdmin.Commission.ViewModels
{
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
            ProductPermissions = query.Permissions;
            HasCommPermissionWrite = hasCommPermissionWrite;
            InsertGroupNone();
        }

        public int CustId { get; set; }

        public PlayerCommissionViewModel CurrentGroup { get; set; }

        public IList<PlayerCommissionViewModel> Groups { get; set; }

        public bool HasCommPermissionWrite { get; set; }

        public IDictionary<string, bool> ProductPermissions { get; }

        private void InsertGroupNone()
        {
            if (CurrentGroup.GroupCommId != 0)
            {
                return;
            }

            if (CurrentGroup.CommValues == null)
            {
                CurrentGroup.InitDefaultValue(ProductPermissions);
            }

            Groups.Insert(0, new PlayerCommissionViewModel
            {
                GroupCommId = 0,
                GroupName = "None",
                CommValues = CurrentGroup.CommValues
            });
        }
    }
}
