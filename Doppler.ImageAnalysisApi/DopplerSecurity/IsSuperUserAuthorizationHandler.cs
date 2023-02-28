using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Doppler.ImageAnalysisApi.DopplerSecurity
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
