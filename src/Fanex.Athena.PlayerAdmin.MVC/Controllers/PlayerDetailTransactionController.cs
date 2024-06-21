using Fanex.Athena.PlayerAdmin.Core.Attributes;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.MVC.Excel;
using Fanex.Athena.PlayerAdmin.MVC.MetaModel;
using Fanex.Athena.PlayerAdmin.MVC.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.Controllers
{
    [CookieAuthorize(Roles.SuperAdmin, Roles.DetailTransaction)]
    public class PlayerDetailTransactionController : Controller
    {
        private readonly IPlayerDetailTransactionService _playerDetailTransactionService;
        private readonly IContextService _contextService;

        public PlayerDetailTransactionController(IPlayerDetailTransactionService playerDetailTransactionService, IContextService contextService)
        {
            _playerDetailTransactionService = playerDetailTransactionService;
            _contextService = contextService;
        }

        [HttpGet]
        [ReferrerValidation]
        public ViewResult Index(int custId, int currencyId)
        {
            var viewModel = new PlayerDetailTransactionReportViewModel
            {
                CustId = custId,
                CurrencyId = currencyId,
                IsEnableSingleWallet = _contextService.IsEnableSingleWallet,
                IsShowSabaCoins = _contextService.IsShowSabaCoins
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestValidation(validateCustId: true)]
        public async Task<IActionResult> GetDetailTransactionReport([FromBody] PlayerDetailTransactionsMetaModel criteria)
        {
            criteria.IsEnableSingleWallet = _contextService.IsEnableSingleWallet;
            var data = await _playerDetailTransactionService.GetDetailTransactionReport(criteria);
            return Ok(data);
        }

        [HttpGet]
        [ReferrerValidation]
        [RequestValidation(validateCustId: true)]
        public async Task<FileResult> ExportDetailTransactionReport(
            [FromServices] IFileProvider file,
            [FromServices] IPlayerDetailTransactionExcel excelService,
            [FromQuery] PlayerDetailTransactionsMetaModel criteria)
        {
            criteria.PageSize = null;
            criteria.PageIndex = null;
            criteria.IsEnableSingleWallet = _contextService.IsEnableSingleWallet;

            var data = await _playerDetailTransactionService.GetDetailTransactionReport(criteria);
            data.IsEnableSingleWallet = _contextService.IsEnableSingleWallet;

            var templatePath = file.GetFileInfo("/App_Data/ExcelTemplate/DetailTransaction.xls").PhysicalPath;
            excelService.SetData(templatePath, data);
            excelService.Export();

            var stream = new MemoryStream();
            excelService.Workbook.Write(stream);

            return new FileContentResult(stream.ToArray(), "application/octet-stream") { FileDownloadName = "DetailTransaction.xls" };
        }
    }
}