namespace Fanex.Athena.PlayerAdmin.Core.Policies
{
    using System;
    using System.Linq;
    using Constants;
    using Microsoft.AspNetCore.Authorization;

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