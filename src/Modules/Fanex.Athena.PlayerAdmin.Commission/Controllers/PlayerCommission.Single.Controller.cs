namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using MetaModels;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    public partial class PlayerCommissionController : Controller
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<ViewResult> SingleMoveGroup([FromQuery] SingleMoveGroupQuery query)
        {
            var productAccess = await _licenseeService.GetProductAccess(_contextService.LicenseeId, "CommissionGroups");
            var groups = await _playerCommService.GetAllCommGroups(query, productAccess);
            var currentGroup = await _playerCommService.GetPlayerCommissionGroup(query, productAccess);
            var hasCommPermissionWrite = _contextService.IsInRoles(Roles.SuperAdmin, Roles.CommissionGroup_Write);
            var viewModel = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            return View(viewModel);
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public ViewResult SingleRemoveFromGroup([FromQuery] SingleRemoveFromGroupQuery query)
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
            var productAccess = await _licenseeService.GetProductAccess(_contextService.LicenseeId, "CommissionGroups");
            var groups = await _playerCommService.GetAllCommGroups(query, productAccess);
            var currentGroup = await _playerCommService.GetPlayerCommissionGroup(query, productAccess);
            var viewModel = new SingleMoveGroupViewModel(query, groups, currentGroup, true);

            return View(viewModel);
        }
    }
}