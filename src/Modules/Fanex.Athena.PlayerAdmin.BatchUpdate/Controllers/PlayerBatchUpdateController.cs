using Fanex.Athena.PlayerAdmin.BatchUpdate.Services;
using Microsoft.AspNetCore.Mvc;

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