using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    public partial class PlayerCommissionController : Controller
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<ViewResult> SingleMoveGroup([FromQuery] SingleMoveGroupQuery query)
        {
            var groups = await _playerCommService.GetAllCommGroups(query);
            var currentGroup = await _playerCommService.GetPlayerCommissionGroup(query);
            var hasCommPermissionWrite = _contextService.IsInRoles(Roles.SuperAdmin, Roles.CommissionGroup_Write);
            var viewModel = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            return View(viewModel);
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<ViewResult> SingleRemoveFromGroup([FromQuery] SingleRemoveFromGroupQuery query)
        {
            query.CustIds = query.CustId.ToString();
            var viewModel = new MultipleRemoveFromGroupViewModel(query);

            return View(viewModel);
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<ViewResult> SingleUpdateGroup([FromQuery] SingleUpdateGroupQuery query)
        {
            var groups = await _playerCommService.GetAllCommGroups(query);
            var currentGroup = await _playerCommService.GetPlayerCommissionGroup(query);
            var viewModel = new SingleMoveGroupViewModel(query, groups, currentGroup, true);
            return View(viewModel);
        }
    }
}