using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Authentication;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity.Session;

public class IdentitySessionDynamicClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
{
    public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (identity == null)
        {
            return;
        }

        var userId = identity.FindUserId();
        if (userId == null)
        {
            return;
        }

        var logger = context.ServiceProvider.GetRequiredService<ILogger<IdentitySessionDynamicClaimsPrincipalContributor>>();
        var logout = false;

        var sessionId = identity.FindSessionId();
        if (sessionId == null)
        {
            logger.LogWarning($"SessionId({sessionId}) claim not found for user: {userId}, log out.");
            logout = true;
        }
        else
        {
            var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();
            var identitySessionChecker = context.ServiceProvider.GetRequiredService<IdentitySessionChecker>();
            using (currentTenant.Change(identity.FindTenantId()))
            {
                if (!await identitySessionChecker.IsValidateAsync(sessionId))
                {
                    logger.LogWarning($"SessionId({sessionId}) not valid for user: {userId}, log out.");
                    logout = true;
                }
            }
        }

        if (logout)
        {
            // Log out
            await context.ServiceProvider.GetRequiredService<IdentityDynamicClaimsPrincipalContributorCache>().ClearAsync(userId.Value);
            context.ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

            logger.LogWarning("The token is no longer valid because the user's session expired.");

            var tokenUnauthorizedErrorInfo = context.ServiceProvider.GetRequiredService<AbpAspNetCoreTokenUnauthorizedErrorInfo>();
            tokenUnauthorizedErrorInfo.Error = AbpExceptionHandlingConsts.InvalidToken;
            tokenUnauthorizedErrorInfo.ErrorDescription = AbpExceptionHandlingConsts.SessionExpired;
        }
    }
}
