using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Resources;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FunctionUMAttribute : TypeFilterAttribute
    {
        public FunctionUMAttribute(FunctionUMType functionUMtype)
            : base(typeof(FunctionUMImplAttribute))
        {
            Arguments = new object[] { functionUMtype };
            Order = -1;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class FunctionUMImplAttribute : ActionFilterAttribute
    {
        private readonly FunctionUMType _functionUMType;
        private readonly IFunctionUMService _umService;

        public FunctionUMImplAttribute(IFunctionUMService umService, FunctionUMType functionUMtype)
        {
            _functionUMType = functionUMtype;
            _umService = umService;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            const int UMCode = 999999;
            (bool isUM, string timeFrom, string timeTo) = _umService.GetUnderMaintenanceInfo(_functionUMType);
            if (isUM)
            {
                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    context.Result = new JsonResult(new ResultInfo { Message = string.Format(ViewResources.FunctionUMMessage, timeFrom, timeTo), Code = UMCode })
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
            }
            else
            {
                await next();
            }
        }
    }
}
