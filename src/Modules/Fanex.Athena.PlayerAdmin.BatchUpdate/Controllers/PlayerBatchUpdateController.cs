namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services;

    public partial class PlayerBatchUpdateController : Controller
    {
        private readonly IPlayerBatchUpdateService _service;

        public PlayerBatchUpdateController(IPlayerBatchUpdateService service)
        {
            _service = service;
        }
    }
}