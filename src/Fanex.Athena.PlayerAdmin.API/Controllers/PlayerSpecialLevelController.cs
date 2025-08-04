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

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ReferrerValidation]
    [EnableCors(Policy.FromPortalSite)]
    [ApiController]
    public class PlayerSpecialLevelController : ControllerBase
    {
        private readonly IPlayerSpecialLevelService _playerSpecialLevelService;
        private readonly IContextService _contextService;
        private readonly ILogger<PlayerSpecialLevelController> _logger;

        public PlayerSpecialLevelController(
            IPlayerSpecialLevelService playerSpecialLevelService,
            IContextService contextService,
            ILogger<PlayerSpecialLevelController> logger)
        {
            _playerSpecialLevelService = playerSpecialLevelService;
            _contextService = contextService;
            _logger = logger;
        }

        [HttpPost]
        [BearerAuthorize(Roles.SuperAdmin, Roles.UpdateSpecialLevel)]
        [RequestValidation(validateCustId: true, validateClientIP: true)]
        [AutoBindingMetaModel(bindUserId: true, bindClientIP: true, bindLanguage: true, bindSiteName: true)]
        public async Task<IActionResult> Update(SpecialLevelUpdatingMetaModel metaModel)
        {
            if (_contextService.IsFakeLicensee)
            {
                return Ok(new ResultViewModel
                {
                    Success = false,
                    ErrorMessage = ResourceKeyConstants.FunctionDenied
                });
            }

            try
            {
                return Ok(await _playerSpecialLevelService.Update(metaModel));
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, ErrorMessageHelper.GenerateMessage(exception, HttpContext));
            }

            return Ok(new ResultViewModel
            {
                Success = false,
                ErrorMessage = ResourceKeyConstants.FunctionDenied
            });
        }
    }
}