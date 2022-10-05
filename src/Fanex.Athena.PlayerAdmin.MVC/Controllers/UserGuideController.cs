using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{

    [AllowAnonymous]
    public class UserGuideController : Controller
    {
        private readonly IContextService _contextService;
        public UserGuideController(IContextService contextService)
        {
            _contextService = contextService;
        }

        [HttpGet]
        [Route("UserGuide/Document/{name}/{culture}")]
        public ActionResult Index(string name, string culture)
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture, true);
            CultureInfo.CurrentUICulture = new CultureInfo(culture, true);
            return View(name as object);
        }
    }
}
