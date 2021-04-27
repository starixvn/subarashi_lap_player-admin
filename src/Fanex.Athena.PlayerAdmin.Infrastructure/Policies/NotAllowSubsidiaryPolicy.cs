using Fanex.Athena.PlayerAdmin.Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Fanex.Athena.PlayerAdmin.Infrastructure.Policies
{
    public static class SubsidiaryPolicy
    {
        public static void NotAllowSubsidiaryPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(Policy.NotAllowSubsidiary, policy => policy.RequireAssertion(context => context.User.HasClaim(claim =>
            {
                const string type = "userlevel";
                const int subsidiaryLevel = 2;
                return claim.Type.Equals(type, StringComparison.OrdinalIgnoreCase) && !claim.Equals(subsidiaryLevel);
            })));
        }
    }
}