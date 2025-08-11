namespace Fanex.Athena.PlayerAdmin.Core.Attributes
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Models;
    using Resources;
    using Services;
    using Settings;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class FunctionUMAttribute : TypeFilterAttribute
    {
        public FunctionUMAttribute(FunctionUMType functionUMtype)
            : base(typeof(FunctionUMImplAttribute))
        {
            Arguments = new object[] { functionUMtype };
            Order = -1;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
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
