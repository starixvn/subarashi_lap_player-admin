using Fanex.Athena.PlayerAdmin.Infrastructure.Helpers;
using Fanex.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Middlewares
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandler(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleError(context, exception);
            }
        }

        private async Task HandleError(HttpContext context, Exception error)
        {
            var errorMsg = ErrorMessageHelper.GenerateMessage(error, context);
            await _logger.ErrorAsync(errorMsg);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}