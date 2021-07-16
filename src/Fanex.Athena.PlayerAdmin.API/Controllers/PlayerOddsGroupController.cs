using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.Utilities;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Fanex.Athena.PlayerAdmin.Infrastructure.Helpers;
using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Fanex.Logging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        private readonly ILogger _logger;

        public PlayerOddsGroupController(
            IPlayerOddsGroupService playerOddsGroupService,
            ILicenseeService licenseeService,
            ILogger logger)
        {
            _playerOddsGroupService = playerOddsGroupService;
            _licenseeService = licenseeService;
            _logger = logger;
        }

        [HttpPost]
        [ReferrerValidation]
        [BearerAuthorize(Roles.SuperAdmin, Roles.UpdateOddsGroup)]
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindUserId: true, bindClientIP: true, bindLanguage: true, bindSiteName: true)]
        public async Task<IActionResult> Update(OddsGroupUpdatingMetaModel metaModel)
        {
            if (_licenseeService.IsFakeLicensee(metaModel.SiteName))
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
                await _logger.ErrorAsync(ErrorMessageHelper.GenerateMessage(exception, HttpContext));
            }

            return Ok(new ResultViewModel
            {
                Success = false,
                ErrorMessage = ResourceKeyConstants.HttpErrDefault
            });
        }
    }
}