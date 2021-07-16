using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Filters;
using Fanex.Athena.PlayerAdmin.Infrastructure.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors(Policy.FromPortalSite)]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PlayerAdmin)]
    [ApiController]
    public class SiteSettingController : ControllerBase
    {
        private readonly ILicenseeService _licenseeService;
        private readonly IContextService _contextService;

        public SiteSettingController(ILicenseeService licenseeService, IContextService contextService)
        {
            _licenseeService = licenseeService;
            _contextService = contextService;
        }

        [HttpPost]
        public async Task<SiteSettingViewModel> GetSetting()
        {
            var siteName = _contextService.SiteName;
            return new SiteSettingViewModel
            {
                IsAllowedCustomerComm = _contextService.IsAllowedCustomerComm,
                IsAllowedBA = _contextService.IsEnableBA && (await _licenseeService.CheckProductBelongsTo(siteName, ProductType.BA))
            };
        }
    }
}