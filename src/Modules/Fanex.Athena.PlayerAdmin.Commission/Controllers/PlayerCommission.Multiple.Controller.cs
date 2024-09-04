using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.Core.Extensions;

namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    public partial class PlayerCommissionController : Controller
    {
        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        [DecompressModelStateAndMetaModel("query", new[] { "CustIds", "CustNames" })]
        public async Task<ViewResult> MultipleMoveGroup([FromQuery] MultipleMoveGroupQuery query)
        {
            var groups = await _playerCommService.GetAllCommGroups(query);

            var viewModel = new MultipleMoveGroupViewModel(query, groups);

            return View(viewModel);
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        [DecompressModelStateAndMetaModel("query", new[] { "CustIds", "CustNames" })]
        public ViewResult MultipleRemoveFromGroup([FromQuery] MultipleRemoveFromGroupQuery query)
        {
            var viewModel = new MultipleRemoveFromGroupViewModel(query);

            return View(viewModel);
        }

        [HttpGet]
        [ReferrerValidation]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        [DecompressModelStateAndMetaModel("query", new[] { "CustIds", "CustNames" })]
        public async Task<ViewResult> MultipleUpdateGroup([FromQuery] MultipleUpdateGroupQuery query)
        {
            var productAccess = await _licenseeService.GetProductAccess(_contextService.LicenseeId);
            var commValues = await _cache.TryGetOrAddFromCache($"CommSettings_{query.LicenseeId}", ThirtyMinutes,
                () => _playerCommService.InitCommValues(productAccess));

            var viewModel = new MultipleUpdateGroupViewModel(query)
            {
                CommValues = commValues
            };

            return View(viewModel);
        }
    }
}