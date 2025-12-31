using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using Volo.Saas.Host;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Tenants;
using Xunit;

namespace Volo.Saas.Tenant;

public class TenantAppService_Tests : SaasTenantApplicationTestBase
{
    private readonly ITenantAppService _tenantAppService;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IDistributedCache<TenantConfigurationCacheItem> _cache;
    private readonly ITenantStore _tenantStore;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantNormalizer _tenantNormalizer;

    public TenantAppService_Tests()
    {
        _tenantAppService = GetRequiredService<ITenantAppService>();
        _unitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        _cache = GetRequiredService<IDistributedCache<TenantConfigurationCacheItem>>();
        _tenantStore = GetRequiredService<ITenantStore>();
        _tenantRepository = GetRequiredService<ITenantRepository>();
        _tenantNormalizer = GetRequiredService<ITenantNormalizer>();
    }

    [Fact]
    public async Task Cache_Should_Invalidator_When_Tenant_ConnectionString_Changed()
    {
        var acme = await _tenantRepository.FindByNameAsync(_tenantNormalizer.NormalizeName("acme"));

        // FindAsync will cache tenant.
        await _tenantStore.FindAsync(acme.Id);
        await _tenantStore.FindAsync(acme.NormalizedName);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldNotBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldNotBeNull();

        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            await _tenantAppService.UpdateConnectionStringsAsync(acme.Id, new SaasTenantConnectionStringsDto() {Default = Guid.NewGuid().ToString()});
            await uow.CompleteAsync();
        }

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldBeNull();

        // FindAsync will cache tenant.
        await _tenantStore.FindAsync(acme.Id);
        await _tenantStore.FindAsync(acme.NormalizedName);

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldNotBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldNotBeNull();

        using (var uow = _unitOfWorkManager.Begin(requiresNew: true))
        {
            await _tenantAppService.UpdateConnectionStringsAsync(acme.Id, new SaasTenantConnectionStringsDto());
            await uow.CompleteAsync();
        }

        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(acme.Id, null))).ShouldBeNull();
        (await _cache.GetAsync(TenantConfigurationCacheItem.CalculateCacheKey(null, acme.NormalizedName))).ShouldBeNull();
    }
}
