using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Fanex.Athena.PlayerAdmin.Infrastructure.Attributes;
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

        public PlayerDetailTransactionController(IPlayerDetailTransactionService playerDetailTransactionService)
        {
            _playerDetailTransactionService = playerDetailTransactionService;
        }

        [HttpGet]
        [ReferrerValidation]
        public ViewResult Index(int custId, int currencyId)
        {
            var viewModel = new PlayerDetailTransactionReportViewModel { CustId = custId, CurrencyId = currencyId };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestValidation(validateCustId: true)]
        public async Task<IActionResult> GetDetailTransactionReport([FromBody] PlayerDetailTransactionsMetaModel criteria)
        {
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
            var data = await _playerDetailTransactionService.GetDetailTransactionReport(criteria);

            var templatePath = file.GetFileInfo("/App_Data/ExcelTemplate/DetailTransaction.xls").PhysicalPath;
            excelService.SetData(templatePath, data);
            excelService.Export();

            var stream = new MemoryStream();
            excelService.Workbook.Write(stream);

            return new FileContentResult(stream.ToArray(), "application/octet-stream") { FileDownloadName = "DetailTransaction.xls" };
        }
    }
}