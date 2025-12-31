using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;

namespace Volo.Abp.LanguageManagement;

public class LanguageListCacheInvalidator : ILocalEventHandler<EntityChangedEventData<Language>>, ITransientDependency
{
    protected IDistributedCache<LanguageListCacheItem> Cache { get; }
    protected IDistributedEventBus DistributedEventBus { get; }

    public LanguageListCacheInvalidator(
        IDistributedCache<LanguageListCacheItem> cache,
        IDistributedEventBus distributedEventBus)
    {
        Cache = cache;
        DistributedEventBus = distributedEventBus;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<Language> eventData)
    {
        await Cache.RemoveAsync(DatabaseLanguageProvider.CacheKey, considerUow: true);
        await DistributedEventBus.PublishAsync(new LanguageChangedEto());
    }
}