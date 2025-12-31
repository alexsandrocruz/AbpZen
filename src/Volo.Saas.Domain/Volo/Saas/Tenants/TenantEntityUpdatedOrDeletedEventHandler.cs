using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;
using Volo.Saas.Editions;

namespace Volo.Saas.Tenants;

public class TenantEntityUpdatedOrDeletedEventHandler:
    ILocalEventHandler<EntityUpdatedEventData<Tenant>>,
    ILocalEventHandler<EntityDeletedEventData<Tenant>>,
    ITransientDependency
{
    public ILogger<TenantEntityUpdatedOrDeletedEventHandler> Logger { get; set; }

    private readonly IDistributedCache<EditionDynamicClaimCacheItem> _dynamicClaimCache;

    public TenantEntityUpdatedOrDeletedEventHandler(IDistributedCache<EditionDynamicClaimCacheItem> dynamicClaimCache)
    {
        Logger = NullLogger<TenantEntityUpdatedOrDeletedEventHandler>.Instance;

        _dynamicClaimCache = dynamicClaimCache;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityUpdatedEventData<Tenant> eventData)
    {
        await RemoveDynamicClaimCacheAsync(eventData.Entity.Id);
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<Tenant> eventData)
    {
        await RemoveDynamicClaimCacheAsync(eventData.Entity.Id);
    }

    protected virtual async Task RemoveDynamicClaimCacheAsync(Guid tenantId)
    {
        Logger.LogDebug($"Remove dynamic edition claim cache for tenant: {tenantId}");
        await _dynamicClaimCache.RemoveAsync(EditionDynamicClaimCacheItem.CalculateCacheKey(tenantId));
    }
}
