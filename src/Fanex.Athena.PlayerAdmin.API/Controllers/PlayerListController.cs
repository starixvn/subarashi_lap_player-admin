using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
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
        private readonly IDepositSettingService _depositSettingService;
        private readonly IContextService _contextService;

        public PlayerListController(
            IPlayerListService playerListService,
            IDepositSettingService depositSettingService,
            IContextService contextService)
        {
            _playerListService = playerListService;
            _depositSettingService = depositSettingService;
            _contextService = contextService;
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true, bindSiteName: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> Search(PlayerSearchingMetaModel metaModel)
        {
            if (!(await ValidateParameter(metaModel)))
            {
                return CreateInvalidModelStateResponse();
            }

            var result = await _playerListService.Search(metaModel);
            return result;
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true, bindSiteName: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> GetPlayerListById(PlayerListByIdMetaModel metaModel)
        {
            if (!(await ValidateParameter(metaModel)))
            {
                return CreateInvalidModelStateResponse();
            }

            var result = await _playerListService.GetPlayerListById(metaModel);
            return result;
        }

        [HttpPost]
        [AutoBindingMetaModel(bindAgent: true)]
        public async Task<ActionResult<PlayerListResultViewModel>> ArchivedSearch(PlayerArchivedSearchingMetaModel metaModel)
        {
            var result = await _playerListService.ArchivedSearch(metaModel);
            return result;
        }

        private static JsonResult CreateInvalidModelStateResponse()
        {
            return new JsonResult(new
            {
                IsSuccess = false,
                Msg = "Model state is invalid"
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        private async Task<bool> ValidateParameter(PlayerListBaseMetaModel metaModel)
        {
            if (!ValidationHelper.TryParseArray(metaModel.SpecialLevel, out int[] specialLevels))
            {
                return false;
            }

            var commGroups = (await _depositSettingService.GetCommGroupList(_contextService.LicenseeId).ConfigureAwait(false))
                .Select(_ => _.Id);

            if (!string.IsNullOrEmpty(metaModel.CommGroups) && metaModel.CommGroups.Split(',').Any(_ => !commGroups.Contains(_)))
            {
                return false;
            }

            if (specialLevels.Any(_ => _ < 0 && _ > 10))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(metaModel.CustomerClass))
            {
                var riskLevels = (await _depositSettingService.GetRiskLevelList(_contextService.Language).ConfigureAwait(false))
                    .SelectMany(_ => _.CustomerClassList);

                if (metaModel.CustomerClass.Split(',').Any(_ => !riskLevels.Contains(_)))
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(metaModel.OddsGroup))
            {
                var oddsGroups = _contextService.OfferedOddsGroupList;

                if (metaModel.OddsGroup.Split(',').Any(_ => !oddsGroups.Contains(_)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}