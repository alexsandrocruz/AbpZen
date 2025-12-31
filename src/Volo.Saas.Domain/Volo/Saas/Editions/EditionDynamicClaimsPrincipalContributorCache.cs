using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Saas.Tenants;

namespace Volo.Saas.Editions;

public class EditionDynamicClaimsPrincipalContributorCache : ITransientDependency
{
    public ILogger<EditionDynamicClaimsPrincipalContributorCache> Logger { get; set; }

    protected IDistributedCache<EditionDynamicClaimCacheItem> DynamicClaimCache { get; }
    protected IOptions<EditionDynamicClaimsPrincipalContributorCacheOptions> CacheOptions { get; }
    protected ITenantRepository TenantRepository { get; }

    public EditionDynamicClaimsPrincipalContributorCache(
        IDistributedCache<EditionDynamicClaimCacheItem> dynamicClaimCache,
        IOptions<EditionDynamicClaimsPrincipalContributorCacheOptions> cacheOptions,
        ITenantRepository tenantRepository)
    {
        Logger = NullLogger<EditionDynamicClaimsPrincipalContributorCache>.Instance;

        DynamicClaimCache = dynamicClaimCache;
        CacheOptions = cacheOptions;
        TenantRepository = tenantRepository;
    }

    public virtual async Task<EditionDynamicClaimCacheItem> GetAsync(Guid tenantId)
    {
        Logger.LogDebug($"Get dynamic claims cache for tenant: {tenantId}");

        return await DynamicClaimCache.GetOrAddAsync(EditionDynamicClaimCacheItem.CalculateCacheKey(tenantId), async () =>
        {
            Logger.LogDebug($"Filling dynamic edition claim cache for tenant: {tenantId}");

            var tenant = await TenantRepository.GetAsync(tenantId);
            var editionId = tenant.GetActiveEditionId();
            return new EditionDynamicClaimCacheItem(new AbpDynamicClaim(AbpClaimTypes.EditionId, editionId != null ? editionId.ToString() : null));
        }, () => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheOptions.Value.CacheAbsoluteExpiration
        });
    }
}
