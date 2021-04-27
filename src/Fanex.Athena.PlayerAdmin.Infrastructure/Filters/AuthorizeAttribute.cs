using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Filters
{
    public sealed class CookieAuthorizeAttribute : AuthorizeAttribute
    {
        public CookieAuthorizeAttribute(params string[] roles)
        {
            if (roles.Any())
            {
                Roles = string.Join(",", roles);
            }
            AuthenticationSchemes = "Cookies";
        }
    }

    public sealed class BearerAuthorizeAttribute : AuthorizeAttribute
    {
        public BearerAuthorizeAttribute(params string[] roles)
        {
            if (roles.Any())
            {
                Roles = string.Join(",", roles);
            }
            AuthenticationSchemes = "Bearer";
        }
    }
}