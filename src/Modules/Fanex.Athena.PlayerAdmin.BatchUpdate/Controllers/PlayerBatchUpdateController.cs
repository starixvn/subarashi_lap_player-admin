using Fanex.Athena.PlayerAdmin.BatchUpdate.Services;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers
{
    public partial class PlayerBatchUpdateController : Controller
    {
        private readonly IPlayerBatchUpdateService _service;

        public PlayerBatchUpdateController(IPlayerBatchUpdateService service)
        {
            _service = service;
        }
    }
}