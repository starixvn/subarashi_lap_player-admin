using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;

namespace Fanex.Athena.PlayerAdmin.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        public static bool IsAjaxRequest(this HttpRequest request)
             => request.Headers.TryGetValue(RequestedWithHeader, out StringValues headerValue)
                && string.Equals(headerValue.FirstOrDefault(), XmlHttpRequest, StringComparison.OrdinalIgnoreCase);

        public static bool IsStaticRequest(this HttpRequest request)
             => request.Path.Value?.Contains(".") ?? false;

        public static void HandleInvalidRequest(this ActionExecutingContext actionContext, string errorMsg)
        {
            IActionResult actionResult;
            if (actionContext.HttpContext.Request.IsAjaxRequest())
            {
                actionResult = new JsonResult(new { Message = errorMsg, IsSuccess = false, Code = 1 })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                };
            }
            else
            {
                actionResult = new RedirectResult("/Home/Error?code=400");
                actionContext.HttpContext.Response.Headers.Add("error-msg", errorMsg);
            }
            actionContext.Result = actionResult;
        }
    }
}