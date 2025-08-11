namespace Fanex.Athena.PlayerAdmin.Core.Extensions
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public static class ControllerExtensions
    {
        public static FromJsonString ToJsonResult(this ControllerBase _, string json)
            => new FromJsonString(json);
    }

    public class FromJsonString : IActionResult
    {
        private readonly string _jsonContent;

        public FromJsonString(string json)
        {
            _jsonContent = json;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.OK;
            await response.WriteAsync(_jsonContent);
        }
    }
}
