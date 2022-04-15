using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [CookieAuthorize]
    public class UserGuideController : Controller
    {
        private readonly IContextService _contextService;
        public UserGuideController(IContextService contextService)
        {
            _contextService = contextService;
        }

        [HttpGet]
        [Route("UserGuide/Document/{name}")]
        [ReferrerValidation]
        public ActionResult Index(string name)
        {
            return View(name as object);
        }
    }
}
