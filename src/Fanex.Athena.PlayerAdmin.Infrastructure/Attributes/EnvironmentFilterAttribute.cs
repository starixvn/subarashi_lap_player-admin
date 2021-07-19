using Fanex.Athena.PlayerAdmin.Infrastructure.Settings;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Attributes
{
    public class EnvironmentFilterAttribute : ActionFilterAttribute
    {
        private readonly string env;
        public EnvironmentFilterAttribute(IOptions<AppSetting> setting)
        {
            env = setting.Value.Environment;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
