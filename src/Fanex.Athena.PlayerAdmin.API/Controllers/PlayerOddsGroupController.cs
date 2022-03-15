using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.Utilities;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Helpers;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ReferrerValidation]
    [EnableCors(Policy.FromPortalSite)]
    [ApiController]
    public class PlayerOddsGroupController : ControllerBase
    {
        private readonly IPlayerOddsGroupService _playerOddsGroupService;
        private readonly ILicenseeService _licenseeService;
        private readonly ILogger<PlayerOddsGroupController> _logger;

        public PlayerOddsGroupController(
            IPlayerOddsGroupService playerOddsGroupService,
            ILicenseeService licenseeService,
            ILogger<PlayerOddsGroupController> logger)
        {
            _playerOddsGroupService = playerOddsGroupService;
            _licenseeService = licenseeService;
            _logger = logger;
        }

        [HttpPost]
        [ReferrerValidation]
        [BearerAuthorize(Roles.SuperAdmin, Roles.UpdateOddsGroup)]
        [RequestValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindUserId: true, bindClientIP: true, bindLanguage: true, bindSiteName: true)]
        public async Task<IActionResult> Update(OddsGroupUpdatingMetaModel metaModel)
        {

            if (_licenseeService.IsFakeLicensee(metaModel.SiteName) || !IsValidOddsGroup(metaModel.SiteName, metaModel.OddsGroup))
            {
                return Ok(new ResultViewModel
                {
                    Success = false,
                    ErrorMessage = ResourceKeyConstants.FunctionDenied
                });
            }

            try
            {
                return Ok(await _playerOddsGroupService.Update(metaModel));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, ErrorMessageHelper.GenerateMessage(exception, HttpContext));
            }

            return Ok(new ResultViewModel
            {
                Success = false,
                ErrorMessage = ResourceKeyConstants.HttpErrDefault
            });
        }

        private bool IsValidOddsGroup(string siteName, string oddsGroup)
        {
            var oddsGroups = _licenseeService.GetOddsGroups(siteName);
            var isValid = oddsGroups.Any(_ => string.Equals(_, oddsGroup, StringComparison.InvariantCultureIgnoreCase));
            return isValid;
        }
    }
}