using Microsoft.AspNetCore.Authorization;

namespace Doppler.ImageAnalyzer.Api.DopplerSecurity
{
    public partial class IsSuperUserAuthorizationHandler : AuthorizationHandler<DopplerAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DopplerAuthorizationRequirement requirement)
        {
            if (requirement.AllowSuperUser && IsSuperUser(context))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private static bool IsSuperUser(AuthorizationHandlerContext context)
        {
            if (!context.User.HasClaim(c => c.Type.Equals(DopplerSecurityDefaults.SuperUserJwtKey, StringComparison.Ordinal)))
            {
                return false;
            }

            var isSuperUser = bool.Parse(context.User.FindFirst(c => c.Type.Equals(DopplerSecurityDefaults.SuperUserJwtKey, StringComparison.Ordinal)).Value);
            return isSuperUser;
        }
    }
}
