using Fanex.Athena.PlayerAdmin.Core.Constants;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Core.Policies
{
    public static class SubsidiaryPolicy
    {
        public static void NotAllowSubsidiaryPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(Policy.NotAllowSubsidiary, policy => policy.RequireAssertion(context => context.User.Identity.IsAuthenticated && !context.User.Claims.First(claim =>
            {
                const string type = "userlevel";
                return claim.Type.Equals(type, StringComparison.OrdinalIgnoreCase);
            }).Value.Equals(UserLevelType.Subsidiary.ToString())));
        }
    }
}