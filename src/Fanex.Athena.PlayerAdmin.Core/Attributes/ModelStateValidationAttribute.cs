using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Fanex.Athena.PlayerAdmin.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ModelStateValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;

            if (!modelState.IsValid)
            {
                context.Result = new JsonResult(new
                {
                    IsSuccess = false,
                    Msg = "Model state is invalid"
                })
                { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            base.OnActionExecuting(context);
        }
    }
}