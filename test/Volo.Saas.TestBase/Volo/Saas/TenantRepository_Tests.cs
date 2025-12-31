using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Saas.Tenants;
using Xunit;

namespace Volo.Saas;

public abstract class TenantRepository_Tests<TStartupModule> : SaasTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    public ITenantRepository TenantRepository { get; }
    public SaasTestData TestData { get; }
    public ITenantNormalizer TenantNormalizer { get; }

    protected TenantRepository_Tests()
    {
        TenantRepository = GetRequiredService<ITenantRepository>();
        TestData = GetRequiredService<SaasTestData>();
        TenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task FindByNameAsync()
    {
        var tenant = await TenantRepository.FindByNameAsync(TenantNormalizer.NormalizeName(TestData.FirstTenantName));
        tenant.ShouldNotBeNull();

        tenant = await TenantRepository.FindByNameAsync(TenantNormalizer.NormalizeName("undefined-tenant"));
        tenant.ShouldBeNull();

        tenant = await TenantRepository.FindByNameAsync(TenantNormalizer.NormalizeName(TestData.FirstTenantName), includeDetails: true);
        tenant.ShouldNotBeNull();
        tenant.ConnectionStrings.Count.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task FindAsync()
    {
        var tenantId = (await TenantRepository.FindByNameAsync(TenantNormalizer.NormalizeName(TestData.FirstTenantName))).Id;

        var tenant = await TenantRepository.FindAsync(tenantId);
        tenant.ShouldNotBeNull();

        tenant = await TenantRepository.FindAsync(Guid.NewGuid());
        tenant.ShouldBeNull();

        tenant = await TenantRepository.FindAsync(tenantId, includeDetails: true);
        tenant.ShouldNotBeNull();
        tenant.ConnectionStrings.Count.ShouldBeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task GetListAsync()
    {
        var tenants = await TenantRepository.GetListAsync();
        tenants.ShouldContain(t => t.Name == TestData.FirstTenantName && t.NormalizedName == TenantNormalizer.NormalizeName(TestData.FirstTenantName));
        tenants.ShouldContain(t => t.Name == TestData.SecondTenantName && t.NormalizedName == TenantNormalizer.NormalizeName(TestData.SecondTenantName));
    }

    [Fact]
    public async Task GetListAsync_WithExpirationDateMinParameter()
    {
        var tenants = await TenantRepository.GetListAsync(expirationDateMin: TestData.FirstTenantExpireDate);

        tenants.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetListAsync_WithExpirationDateMaxParameter()
    {
        var tenants = await TenantRepository.GetListAsync(expirationDateMax: TestData.FirstTenantExpireDate.AddDays(2));

        tenants.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetListAsync_WithActivationStateParameter()
    {
        var activationState = TenantActivationState.ActiveWithLimitedTime;
        var tenants = await TenantRepository.GetListAsync(tenantActivationState: activationState);

        tenants.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetListAsync_WithExpirationDateRange_ShouldReturnEmpty()
    {
        var minDate = TestData.FirstTenantExpireDate.AddDays(1);
        var maxDate = TestData.FirstTenantExpireDate.AddDays(2);
        var tenants = await TenantRepository.GetListAsync(expirationDateMin: minDate, expirationDateMax: maxDate);

        tenants.Count.ShouldBe(0);
    }

    [Fact]
    public async Task GetListAsync_WithExpirationDateRange()
    {
        var minDate = TestData.FirstTenantExpireDate.AddDays(-1);
        var maxDate = TestData.FirstTenantExpireDate.AddDays(1);
        var tenants = await TenantRepository.GetListAsync(expirationDateMin: minDate, expirationDateMax: maxDate);

        tenants.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetCountAsync_WithExpirationDateRange()
    {
        var minDate = TestData.FirstTenantExpireDate.AddDays(-1);
        var maxDate = TestData.FirstTenantExpireDate.AddDays(1);
        var count = await TenantRepository.GetCountAsync(expirationDateMin: minDate, expirationDateMax: maxDate);

        count.ShouldBe(1);
    }

    [Fact]
    public async Task GetCountAsync_WithExpirationDateRange_ShouldReturnEmpty()
    {
        var minDate = TestData.FirstTenantExpireDate.AddDays(1);
        var maxDate = TestData.FirstTenantExpireDate.AddDays(2);
        var count = await TenantRepository.GetCountAsync(expirationDateMin: minDate, expirationDateMax: maxDate);
        count.ShouldBe(0);
    }

    [Fact]
    public async Task Should_Eager_Load_Tenant_Collections()
    {
        var role = await TenantRepository.FindByNameAsync(TenantNormalizer.NormalizeName(TestData.FirstTenantName));
        role.ConnectionStrings.ShouldNotBeNull();
        role.ConnectionStrings.Any().ShouldBeTrue();
    }

    [Fact]
    public async Task GetListWithSeparateConnectionStringAsync()
    {
        var tenants = await TenantRepository.GetListWithSeparateConnectionStringAsync();
        tenants.Count.ShouldBeGreaterThanOrEqualTo(1);
        tenants.ShouldContain(t => t.Name == TestData.FirstTenantName && t.NormalizedName == TenantNormalizer.NormalizeName(TestData.FirstTenantName));
    }
}
