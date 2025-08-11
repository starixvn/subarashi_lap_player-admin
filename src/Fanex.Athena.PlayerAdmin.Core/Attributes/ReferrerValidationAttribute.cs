namespace Fanex.Athena.PlayerAdmin.Core.Attributes
{
    using System;
    using System.Linq;
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;
    using Settings;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ReferrerValidationAttribute : TypeFilterAttribute
    {
        public ReferrerValidationAttribute()
            : base(typeof(ReferrerValidationImplAttribute))
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ReferrerValidationImplAttribute : ActionFilterAttribute
    {
        private const string ErrorMsg = "Client is invalid";
        private readonly AppSetting _setting;

        public ReferrerValidationImplAttribute(IOptions<AppSetting> setting)
        {
            _setting = setting.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (!IsValidRequest(context.HttpContext.Request, out string referrerUrl))
            {
                context.HandleInvalidRequest($"{ErrorMsg} : {referrerUrl}");
            }
        }

        private bool IsValidRequest(HttpRequest request, out string referrerUrl)
        {
            referrerUrl = string.Empty;

            if (!_setting.AllowedCrossDomains.IsPresent())
            {
                return true;
            }

            if (request.Headers.TryGetValue(HeaderNames.Referer, out StringValues refererValue))
            {
                referrerUrl = refererValue.FirstOrDefault();
            }

            if (string.IsNullOrWhiteSpace(referrerUrl))
            {
                return false;
            }

            var clientDomain = new Uri(referrerUrl).Authority;
            var isValid = request.Host.Value.Equals(clientDomain, StringComparison.OrdinalIgnoreCase) ||
                _setting.AllowedCrossDomains.Any(allowedCrossDomain => clientDomain.EndsWith(allowedCrossDomain, StringComparison.OrdinalIgnoreCase));
            return isValid;
        }
    }
}