namespace Fanex.Athena.PlayerAdmin.Commission.Controllers
{
    using System;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Services.Interfaces;

    public partial class PlayerCommissionController : Controller
    {
        private readonly IContextService _contextService;
        private readonly ILicenseeService _licenseeService;
        private readonly IPlayerCommissionService _playerCommService;
        private readonly IMemoryCache _cache;

        private static readonly TimeSpan FifteenMinutes = TimeSpan.FromMinutes(15);

        public PlayerCommissionController(
            IContextService contextService,
            ILicenseeService licenseeService,
            IPlayerCommissionService playerCommService,
            IMemoryCache cache)
        {
            _contextService = contextService;
            _licenseeService = licenseeService;
            _playerCommService = playerCommService;
            _cache = cache;
        }
    }
}