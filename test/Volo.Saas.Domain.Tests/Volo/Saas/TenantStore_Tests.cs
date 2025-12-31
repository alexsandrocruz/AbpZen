using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Saas.Tenants;
using Xunit;

namespace Volo.Saas;

public class TenantStore_Tests : SaasDomainTestBase
{
    private readonly ITenantStore _tenantStore;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantNormalizer _tenantNormalizer;

    public TenantStore_Tests()
    {
        _tenantStore = GetRequiredService<ITenantStore>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
        _tenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task FindAsyncByName()
    {
        var acme = await _tenantStore.FindAsync(_tenantNormalizer.NormalizeName("acme"));
        acme.ShouldNotBeNull();
        acme.Name.ShouldBe("acme");
    }

    [Fact]
    public async Task FindAsyncById()
    {
        var acme = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("acme"));
        acme.ShouldNotBeNull();

        (await _tenantStore.FindAsync(acme.Id)).ShouldNotBeNull();
    }

    [Fact]
    public async Task GetListAsync()
    {
        var tenants = await _tenantRepository.GetListAsync();

        tenants.ShouldNotBeNull();
        tenants.Count.ShouldBe(3);
    }
}
