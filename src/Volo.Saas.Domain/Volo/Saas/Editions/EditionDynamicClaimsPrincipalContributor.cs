using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Security.Claims;

namespace Volo.Saas.Editions;

public class EditionDynamicClaimsPrincipalContributor : AbpDynamicClaimsPrincipalContributorBase
{
    public async override Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        var userId = identity?.FindUserId();
        var tenantId = identity?.FindTenantId();
        if (userId == null || tenantId == null)
        {
            return;
        }

        var dynamicClaimsCache = context.GetRequiredService<EditionDynamicClaimsPrincipalContributorCache>();
        EditionDynamicClaimCacheItem dynamicClaims;
        try
        {
            dynamicClaims = await dynamicClaimsCache.GetAsync(tenantId.Value);
        }
        catch (EntityNotFoundException e)
        {
            // In case if tenant not found, We force to clear the claims principal.
            context.ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
            var logger = context.GetRequiredService<ILogger<EditionDynamicClaimsPrincipalContributor>>();
            logger.LogWarning(e, $"Tenant not found: {tenantId.Value}");
            return;
        }

        if (dynamicClaims.Claims.IsNullOrEmpty())
        {
            return;
        }

        await AddDynamicClaimsAsync(context, identity, dynamicClaims.Claims);
    }
}
