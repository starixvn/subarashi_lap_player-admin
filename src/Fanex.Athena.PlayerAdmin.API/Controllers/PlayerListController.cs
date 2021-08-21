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
            return await _playerListService.Search(searchParam);
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true, bindSiteName: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> GetPlayerListById(PlayerListByIdMetaModel metaModel)
        {
            return await _playerListService.GetPlayerListById(metaModel);
        }

        [HttpPost]
        public async Task<ActionResult<LRFFiltersViewModel>> LRFFilters()
        {
            var commGroups = await _playerListService.GetAllCommGroups(_contextService.LicenseeId);

            return new LRFFiltersViewModel
            {
                CustomerClass = await _playerListService.CustomerClass(_contextService.SiteName),
                CommGroups = commGroups
            };
        }
    }
}