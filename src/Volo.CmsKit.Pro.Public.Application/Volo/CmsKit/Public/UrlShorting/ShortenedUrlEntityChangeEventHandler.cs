using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.UrlShorting;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Public.UrlShorting
{
    public class ShortenedUrlEntityChangeEventHandler :
        ILocalEventHandler<EntityCreatedEventData<ShortenedUrl>>,
        ILocalEventHandler<EntityUpdatedEventData<ShortenedUrl>>,
        ILocalEventHandler<EntityDeletedEventData<ShortenedUrl>>,
        ITransientDependency
    {
        private readonly IDistributedCache<ShortenedUrlCacheItem, string> _shortenedUrlCache;

        public ShortenedUrlEntityChangeEventHandler(IDistributedCache<ShortenedUrlCacheItem, string> shortenedUrlCache)
        {
            _shortenedUrlCache = shortenedUrlCache;
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<ShortenedUrl> eventData)
        {
            await RemoveFromCache(eventData.Entity);
        }

        public async Task HandleEventAsync(EntityDeletedEventData<ShortenedUrl> eventData)
        {
            await RemoveFromCache(eventData.Entity);
        }

        public async Task HandleEventAsync(EntityCreatedEventData<ShortenedUrl> eventData)
        {
            await RemoveFromCache(eventData.Entity);
        }

        private async Task RemoveFromCache(ShortenedUrl shortenedUrl)
        {
            await _shortenedUrlCache.RemoveAsync(
                shortenedUrl.Source
                );
        }
    }
}
