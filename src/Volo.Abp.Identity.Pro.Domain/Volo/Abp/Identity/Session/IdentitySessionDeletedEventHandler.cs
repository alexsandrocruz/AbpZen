using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity;

public class IdentitySessionDeletedEventHandler : ILocalEventHandler<EntityDeletedEventData<IdentitySession>>, ITransientDependency
{
    protected IDistributedCache<IdentitySessionCacheItem> Cache { get; }

    public IdentitySessionDeletedEventHandler(IDistributedCache<IdentitySessionCacheItem> cache)
    {
        Cache = cache;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEventData<IdentitySession> eventData)
    {
        await Cache.RemoveAsync(eventData.Entity.SessionId);
    }
}
