namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    using System.Threading.Tasks;
    using Core.Attributes;
    using Core.Constants;
    using Core.Extensions;
    using MetaModels;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    public partial class PlayerCommissionController : Controller
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        [DecompressModelStateAndMetaModel("query", "CustIds", "CustNames")]
        public async Task<ViewResult> MultipleMoveGroup([FromQuery] MultipleMoveGroupQuery query)
        {
            var productAccess = await _licenseeService.GetProductAccess(_contextService.LicenseeId, "CommissionGroups");
            var groups = await _playerCommService.GetAllCommGroups(query, productAccess);

            var viewModel = new MultipleMoveGroupViewModel(query, groups);

            return View(viewModel);
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        [DecompressModelStateAndMetaModel("query", "CustIds", "CustNames")]
        public ViewResult MultipleRemoveFromGroup([FromQuery] MultipleRemoveFromGroupQuery query)
        {
            var viewModel = new MultipleRemoveFromGroupViewModel(query);

            return View(viewModel);
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        [DecompressModelStateAndMetaModel("query", "CustIds", "CustNames")]
        public async Task<ViewResult> MultipleUpdateGroup([FromQuery] MultipleUpdateGroupQuery query)
        {
            var productAccess = await _licenseeService.GetProductAccess(_contextService.LicenseeId, "CommissionGroups");
            var commValues = await _cache.TryGetOrAddFromCache($"CommSettings_{query.LicenseeId}", FifteenMinutes,
                () => _playerCommService.InitCommValues(productAccess));

            var viewModel = new MultipleUpdateGroupViewModel(query)
            {
                CommValues = commValues
            };

            return View(viewModel);
        }
    }
}