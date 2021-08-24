using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ReferrerValidation]
    [EnableCors(Policy.FromPortalSite)]
    [ApiController]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PlayerAdmin)]
    public class PlayerListController : ControllerBase
    {
        private readonly IPlayerListService _playerListService;
        private readonly IContextService _contextService;

        public PlayerListController(IPlayerListService playerListService, IContextService contextService)
        {
            _playerListService = playerListService;
            _contextService = contextService;
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true, bindSiteName: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> Search(PlayerSearchingMetaModel searchParam)
        {
            var result = await _playerListService.Search(searchParam);
            return result;
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true, bindSiteName: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> GetPlayerListById(PlayerListByIdMetaModel metaModel)
        {
            var result = await _playerListService.GetPlayerListById(metaModel);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<LRFFiltersViewModel>> LRFFilters()
        {
            var commGroups = _playerListService.GetAllCommGroups(_contextService.LicenseeId);
            var customerClass = _playerListService.GetCustomerClassList();

            await Task.WhenAll(commGroups, customerClass);

            return new LRFFiltersViewModel
            {
                CustomerClass = await customerClass,
                CommGroups = await commGroups
            };
        }
    }
}