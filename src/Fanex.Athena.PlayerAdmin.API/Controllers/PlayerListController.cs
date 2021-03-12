using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    public class PlayerListController : ControllerBase
    {

        public PlayerListController()
        {
        }

        [HttpGet]
        public Task<JsonResult> Search1()
        {
            var subClaim = HttpContext.User?.Claims.FirstOrDefault(_ => _.Type == "sub");
            return Task.FromResult(new JsonResult(new
            {
                sub = subClaim?.Value
            }));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "super_admin,pa_playerlist")]
        public Task<JsonResult> Search()
        {
            var user = HttpContext.User;
            return Task.FromResult(new JsonResult(new
            {
                sub = user.Claims.FirstOrDefault(_ => _.Type == "sub")?.Value,
                name = user.Identity.Name,
                claims = user.Claims.Select(_ => new { type = _.Type, value = _.Value })
            }));
        }
    }
}
