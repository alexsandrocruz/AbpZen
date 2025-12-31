using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Saas.Tenants;

namespace Volo.Saas.Editions;

public class EditionClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
{
    public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
    {
        var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
        if (identity != null)
        {
            var currentTenant = context.ServiceProvider.GetRequiredService<ICurrentTenant>();
            if (currentTenant.Id != null)
            {
                var tenantRepository = context.ServiceProvider.GetRequiredService<ITenantRepository>();
                var tenant = await tenantRepository.FindAsync(currentTenant.Id.Value);
                if (tenant?.GetActiveEditionId() != null)
                {
                    identity.AddOrReplace(new Claim(AbpClaimTypes.EditionId, tenant.GetActiveEditionId().ToString()));
                }
            }
        }
    }
}
