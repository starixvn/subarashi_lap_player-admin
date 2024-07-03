using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Fanex.Athena.PlayerAdmin.Core.Middlewares
{
    public class PreflightHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PreflightHandler> _logger;

        public PreflightHandler(RequestDelegate next, ILogger<PreflightHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next.Invoke(context);

            // log error preflight
            if (context.Request.Method == HttpMethods.Options && !IsSuccessStatusCode(context.Response.StatusCode))
            {
                var msgInfo = new StringBuilder();
                var response = context.Response;
                var request = context.Request;

                msgInfo.AppendLine("Preflight request is invalid!");
                msgInfo.AppendLine(string.Format("\n- Path: {0}", request.Path));

                msgInfo.AppendLine("\n- Request:");
                foreach (var entry in request.Headers)
                {
                    msgInfo.AppendLine(string.Format("Hearder[{0}]: {1}", entry.Key, entry.Value));
                }

                msgInfo.AppendLine("\n- Respone:");
                msgInfo.AppendLine(string.Format("Status: {0}", response.StatusCode));

                foreach (var entry in response.Headers)
                {
                    msgInfo.AppendLine(string.Format("Hearder[{0}]: {1}", entry.Key, entry.Value));
                }

                _logger.LogError(msgInfo.ToString());
            }
        }

        private static bool IsSuccessStatusCode(int statusCode)
        {
            return statusCode >= StatusCodes.Status200OK && statusCode <= StatusCodes.Status208AlreadyReported;
        }
    }
}