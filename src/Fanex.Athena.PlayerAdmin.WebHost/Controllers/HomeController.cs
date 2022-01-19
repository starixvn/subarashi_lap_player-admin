using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.WebHost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace Fanex.Athena.PlayerAdmin.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
                UserId = contextService.UserId,
                UserName = contextService.Username,
                UserLevel = contextService.UserLevel,
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