namespace Fanex.Athena.PlayerAdmin.API.Controllers
{
    using System.Net.Mime;
    using Core.Attributes;
    using Core.Constants;
    using Core.Services;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.FileProviders;

    [Route("api/[controller]/[action]")]
    [EnableCors(Policy.FromPortalSite)]
    [BearerAuthorize(Roles.SuperAdmin, Roles.PlayerAdmin)]
    [ApiController]
    public class UserGuideController : ControllerBase
    {
        private readonly IContextService _contextService;
        private const string DefaultLanguage = "en-US";

        public UserGuideController(IContextService contextService)
        {
            _contextService = contextService;
        }

        [HttpPost]
        public IActionResult OddsGroupPdf([FromServices] IFileProvider file, string fileName)
        {
            var language = _contextService.Language;
            var fileInfo = GetDefaultFilePathIfNotExist(file, language, fileName);

            return File(fileInfo.CreateReadStream(), MediaTypeNames.Application.Pdf, fileName);
        }

        private static IFileInfo GetDefaultFilePathIfNotExist(IFileProvider file, string language, string fileName)
        {
            const string filePath = "App_Data/UserGuide/pdf/{0}/{1}";
            var fileInfo = file.GetFileInfo(string.Format(filePath, language, fileName));

            if (fileInfo.Exists)
            {
                return fileInfo;
            }

            return file.GetFileInfo(string.Format(filePath, DefaultLanguage, fileName));
        }
    }
}