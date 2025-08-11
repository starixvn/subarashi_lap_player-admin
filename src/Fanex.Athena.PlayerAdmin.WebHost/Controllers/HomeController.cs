namespace Fanex.Athena.PlayerAdmin.WebHost.Controllers
{
    using System.Diagnostics;
    using Core.Attributes;
    using Core.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Hosting;
    using Models;

    public class HomeController : Controller
    {
        [CookieAuthorize]
        public IActionResult Index([FromServices] IWebHostEnvironment hostingEnvironment)
        {
            if (!hostingEnvironment.IsDevelopment())
            {
                return RedirectToAction("AccessDenied");
            }

            return View();
        }

        [HttpGet]
        [CookieAuthorize]
        public IActionResult UserInfo([FromServices] IContextService contextService)
        {
            return Json(new
            {
                contextService.UserId,
                UserName = contextService.Username,
                contextService.UserLevel,
                IsMaster = contextService.IsMasterLicensee,
                Whitelabel = contextService.WhiteLabelID
            });
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code = 500)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Code = code });
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}