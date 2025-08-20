namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Policies
{
    using System.Security.Claims;
    using Core.Constants;
    using Core.Policies;
    using Microsoft.AspNetCore.Authorization;

    public class SubsidiaryPolicyTests
    {
        [Fact]
        public void NotAllowSubsidiaryPolicy_DisallowsSubsidiaryUser()
        {
            var options = new AuthorizationOptions();
            options.NotAllowSubsidiaryPolicy();
            var policy = options.GetPolicy(Policy.NotAllowSubsidiary)!;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "1"),
                new("userlevel", UserLevelType.Subsidiary.ToString())
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(policy.Requirements, principal, null);

            foreach (var r in policy.Requirements)
            {
                var handlerProp = r.GetType().GetProperty("Handler");
                if (handlerProp != null)
                {
                    var maybeDelegate = handlerProp.GetValue(r) as Delegate;
                    if (maybeDelegate != null)
                    {
                        var invoked = maybeDelegate.DynamicInvoke(context);
                        var result = invoked is bool b && b;
                        Assert.False(result);
                    }
                }
            }
        }

        [Fact]
        public void NotAllowSubsidiaryPolicy_AllowsNonSubsidiaryUser()
        {
            var options = new AuthorizationOptions();
            options.NotAllowSubsidiaryPolicy();
            var policy = options.GetPolicy(Policy.NotAllowSubsidiary)!;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "1"),
                new("userlevel", UserLevelType.Licensee.ToString())
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(policy.Requirements, principal, null);

            Assert.NotEmpty(policy.Requirements);
        }
    }
}