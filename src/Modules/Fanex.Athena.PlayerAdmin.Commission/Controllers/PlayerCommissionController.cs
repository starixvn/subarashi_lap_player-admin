using Fanex.Athena.PlayerAdmin.Commission.Helpers;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Athena.PlayerAdmin.Commission.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    public partial class PlayerCommissionController : Controller
    {
        private readonly IContextService _contextService;
        private readonly ILicenseeService _licenseeService;
        private readonly IPlayerCommissionService _playerCommService;

        public PlayerCommissionController(IContextService contextService, ILicenseeService licenseeService,
            IPlayerCommissionService playerCommService)
        {
            _contextService = contextService;
            _licenseeService = licenseeService;
            _playerCommService = playerCommService;
        }

        private async Task<IDictionary<string, bool>> GetProductPermissions()
        {
            var productAccess = await _licenseeService.GetProductAccess(_contextService.LicenseeId, _contextService.Language);
            var supportedProducts = _licenseeService.GetAllowedProductsByFunction("CommissionGroups");
            return CommissionGroupHelper.GetProductPermissions(supportedProducts, productAccess);
        }

        private async Task AssignSessionToQuery(BasePlayerCommissionUpdateQuery query)
        {
            query.Permissions = await GetProductPermissions();
        }
    }
}