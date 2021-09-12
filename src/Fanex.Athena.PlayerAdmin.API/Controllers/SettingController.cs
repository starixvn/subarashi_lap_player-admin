using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors(Policy.FromPortalSite)]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PlayerAdmin)]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly IContextService _contextService;

        public SettingController(IContextService contextService)
        {
            _contextService = contextService;
        }

        [HttpPost]
        public async Task<JsonResult> GetSiteSetting([FromServices] ILicenseeService licenseeService)
        {
            var siteName = _contextService.SiteName;
            return new JsonResult(new SiteSettingViewModel
            {
                IsAllowedCustomerComm = _contextService.IsAllowedCustomerComm,
                IsAllowedBA = _contextService.IsEnableBA && (await licenseeService.CheckProductBelongsTo(siteName, ProductTypes.BA)),
                UserLevel = _contextService.UserLevel,
                IsEnableSingleWallet = _contextService.IsEnableSingleWallet
            }, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        [HttpPost]
        public async Task<ActionResult<DepositSettingViewModel>> GetDepositSetting([FromServices] IDepositSettingService depositSettingService)
        {
            var commGroups = depositSettingService.GetAllCommGroups(_contextService.LicenseeId);
            var customerClass = depositSettingService.GetCustomerClassList();

            await Task.WhenAll(commGroups, customerClass);

            return new DepositSettingViewModel
            {
                CustomerClass = await customerClass,
                CommGroups = await commGroups
            };
        }

        [HttpPost]
        public async Task<IActionResult> GetSubsidiarySetting([FromServices] ISubsidiaryService subsidiaryService)
        {
            var subsidiaryViewType = _contextService.SubsidiaryViewType;
            var subsidiaryList = await subsidiaryService.GetSubsidiaryList(_contextService.SiteName, _contextService.AgentList,
                _contextService.WhiteLabelID, subsidiaryViewType);

            return Ok(new
            {
                SubsidiaryViewType = !subsidiaryList.Any() ? SubsidiaryViewType.NotManagement : subsidiaryViewType,
                SubsidiaryList = subsidiaryList
            });
        }
    }
}