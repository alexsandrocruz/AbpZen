using System.Security.Claims;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Saas.Tenants;
using Xunit;

namespace Volo.Saas;

public class EditionClaimsPrincipalContributor_Tests : SaasDomainTestBase
{
    private readonly IAbpClaimsPrincipalFactory _abpClaimsPrincipalFactory;
    private readonly ICurrentTenant _currentTenant;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantNormalizer _tenantNormalizer;

    public EditionClaimsPrincipalContributor_Tests()
    {
        _abpClaimsPrincipalFactory = GetRequiredService<IAbpClaimsPrincipalFactory>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
        _tenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task Should_Add_EditionId_Claim()
    {
        var tenant = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("volosoft"));
        tenant.ShouldNotBeNull();

        using (_currentTenant.Change(tenant.Id))
        {
            var claimsPrincipal = await _abpClaimsPrincipalFactory.CreateAsync();
            claimsPrincipal.Claims.ShouldContain(x => x.Type == AbpClaimTypes.EditionId && x.Value == tenant.GetActiveEditionId().ToString());
        }
    }

    [Fact]
    public async Task Should_Not_Add_EditionId_Claim()
    {
        var tenant = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("acme"));
        tenant.ShouldNotBeNull();

        using (_currentTenant.Change(tenant.Id))
        {
            var claimsPrincipal = await _abpClaimsPrincipalFactory.CreateAsync();
            claimsPrincipal.Claims.ShouldNotContain(x => x.Type == AbpClaimTypes.EditionId);
        }
    }
}
