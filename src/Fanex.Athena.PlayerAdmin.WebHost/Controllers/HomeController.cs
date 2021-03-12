using Fanex.Athena.PlayerAdmin.WebHost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult Menu()
        {
            return View();
        }


        [Authorize(AuthenticationSchemes = "Cookies")]
        public JsonResult Info()
        {
            var user = HttpContext.User;
            return new JsonResult(new
            {
                sub = user.Claims.FirstOrDefault(_ => _.Type == "sub")?.Value,
                name = user.Identity.Name,
                claims = user.Claims.Select(_ => new { type = _.Type, value = _.Value })
            });
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "")]
        public JsonResult InfoNoRole()
        {
            var user = HttpContext.User;
            return new JsonResult(new
            {
                sub = user.Claims.FirstOrDefault(_ => _.Type == "sub")?.Value,
                name = user.Identity.Name,
                claims = user.Claims.Select(_ => new { type = _.Type, value = _.Value })
            });
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}