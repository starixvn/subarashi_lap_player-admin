namespace Fanex.Athena.PlayerAdmin.Core.Attributes
{
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    [AttributeUsage(AttributeTargets.Method)]
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