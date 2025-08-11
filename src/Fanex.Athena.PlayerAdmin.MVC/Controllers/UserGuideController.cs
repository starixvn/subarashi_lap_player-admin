namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    using System.Globalization;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    public class UserGuideController : Controller
    {
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
