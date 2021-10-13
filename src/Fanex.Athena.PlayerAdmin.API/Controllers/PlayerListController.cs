using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
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

        public PlayerListController(IPlayerListService playerListService)
        {
            _playerListService = playerListService;
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true, bindSiteName: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> Search(PlayerSearchingMetaModel metaModel)
        {
            var result = await _playerListService.Search(metaModel);
            return result;
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true, bindSiteName: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> GetPlayerListById(PlayerListByIdMetaModel metaModel)
        {
            var result = await _playerListService.GetPlayerListById(metaModel);
            return result;
        }
    }
}