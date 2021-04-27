using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    public partial class PlayerCommissionController : Controller
    {
        [HttpGet]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<ViewResult> MultipleMoveGroup([FromQuery] MultipleMoveGroupQuery query)
        {
            await AssignSessionToQuery(query);

            var groups = await _playerCommService.GetAllCommGroups(query);

            var viewModel = new MultipleMoveGroupViewModel(query, groups);

            return View(viewModel);
        }

        [HttpGet]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<ViewResult> MultipleRemoveFromGroup([FromQuery] MultipleRemoveFromGroupQuery query)
        {
            await AssignSessionToQuery(query);
            var viewModel = new MultipleRemoveFromGroupViewModel(query);

            return View(viewModel);
        }

        [HttpGet]
        [CookieAuthorize(Roles.SuperAdmin, Roles.CommissionGroup_Write, Policy = Policy.NotAllowSubsidiary)]
        [AutoBindingMetaModel(bindLicenseeId: true, bindUsername: true, bindClientIP: true, bindLanguage: true, bindUserId: true)]
        public async Task<ViewResult> MultipleUpdateGroup([FromQuery] MultipleUpdateGroupQuery query)
        {
            await AssignSessionToQuery(query);
            var viewModel = new MultipleUpdateGroupViewModel(query);
            return View(viewModel);
        }
    }
}