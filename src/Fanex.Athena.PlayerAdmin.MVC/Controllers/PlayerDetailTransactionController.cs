using Fanex.Athena.PlayerAdmin.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{ 
    public class PlayerDetailTransactionController : Controller
    {
        public PlayerDetailTransactionController()
        {
        }

        [HttpGet]
        [Authorize]
        public ViewResult Index(int custId, int currencyId)
        {
            var viewModel = new PlayerDetailTransactionReportViewModel { CustId = custId, CurrencyId = currencyId };

            return View("~/Views/PlayerDetailTransaction/Index.cshtml", viewModel);
        }
    }
}
