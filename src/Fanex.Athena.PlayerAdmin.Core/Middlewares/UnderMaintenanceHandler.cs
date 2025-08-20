namespace Fanex.Athena.PlayerAdmin.Core.Middlewares
{
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using ONEbook.UM.Services;

    public class UnderMaintenanceHandler
    {
        private const string AjaxRequestMsgFormat = "System is currently unavailable due to scheduled maintenance. This downtime will last from {0} to {1} (HK Time GMT +8)";

        private const string HtmlRequestMsgFormat = "<div style='text-align:center;padding-top:10%'>System is currently unavailable due to scheduled maintenance." +
            "<br /> This downtime will last from <b>{0}</b> &rarr; <b>{1}</b> (HK Time GMT +8). Thanks for your patience!</div>";

        private readonly RequestDelegate _next;
        private readonly IMaintenanceService _maintenanceService;

        public UnderMaintenanceHandler(RequestDelegate next, IMaintenanceService maintenanceService)
        {
            _next = next;
            _maintenanceService = maintenanceService;
        }

        public async Task Invoke(HttpContext context)
        {
            var isUM = await _maintenanceService.IsUnderMaintenanceAsync();
            if (isUM)
            {
                var umTime = await _maintenanceService.GetUnderMaintenanceTimeAsync();
                string msg;
                string contentType;
                if (context.Request.IsAjaxRequest())
                {
                    contentType = "application/json";
                    msg = JsonConvert.SerializeObject(new
                    {
                        msg = string.Format(AjaxRequestMsgFormat, umTime.From, umTime.To),
                        isSuccess = false
                    });
                }
                else
                {
                    contentType = "text/html";
                    msg = string.Format(HtmlRequestMsgFormat, umTime.From, umTime.To);
                }

                context.Response.ContentType = contentType;
                await context.Response.WriteAsync(msg);
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}