using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncKeyedLock;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.LanguageManagement.External;

public class ExternalLocalizationTextCache : IExternalLocalizationTextCache, ISingletonDependency
{
    protected IDistributedCache<ExternalLocalizationTextCacheItem> TextsDistributedCache { get; }
    protected IDistributedCache<ExternalLocalizationTextCacheStampItem> StampsDistributedCache { get; }
    protected ConcurrentDictionary<string, MemoryCacheItem> MemoryCache { get; } = new();
    protected IAbpDistributedLock DistributedLock { get; }

    private readonly AsyncKeyedLocker<string> _localLocker = new(o =>
    {
        o.PoolSize = 20;
        o.PoolInitialFill = 1;
    });

    public ExternalLocalizationTextCache(
        IDistributedCache<ExternalLocalizationTextCacheItem> textsDistributedCache,
        IDistributedCache<ExternalLocalizationTextCacheStampItem> stampsDistributedCache,
        IAbpDistributedLock distributedLock)
    {
        TextsDistributedCache = textsDistributedCache;
        StampsDistributedCache = stampsDistributedCache;
        DistributedLock = distributedLock;
    }

    public virtual Dictionary<string, string>? TryGetTextsFromCache(string resourceName, string cultureName)
    {
        var textsCacheKey = GetTextsCacheKey(resourceName, cultureName);
        var memoryCacheItem = MemoryCache.GetOrDefault(textsCacheKey);
        if (memoryCacheItem != null && !IsCacheItemOutdated(memoryCacheItem))
        {
            /* No need to that frequently check it, we can go with the in-memory data */
            return memoryCacheItem.Texts;
        }

        return null;
    }

    public virtual async Task<Dictionary<string, string>> GetTextsAsync(
        string resourceName,
        string cultureName,
        Func<Task<Dictionary<string, string>>> factory)
    {
        var textsCacheKey = GetTextsCacheKey(resourceName, cultureName);
        var memoryCacheItem = MemoryCache.GetOrDefault(textsCacheKey);
        if (memoryCacheItem != null && !IsCacheItemOutdated(memoryCacheItem))
        {
            /* No need to that frequently check it, we can go with the in-memory data */
            return memoryCacheItem.Texts;
        }

        /* Using a local lock to ensure multiple threads doesn't do the same work
         * (accessing to distributed cache unnecessarily) */
        using (await _localLocker.LockAsync(textsCacheKey))
        {
            /* Double-check (maybe another thread updated the memoryCacheItem) */
            memoryCacheItem = MemoryCache.GetOrDefault(textsCacheKey);
            if (memoryCacheItem != null && !IsCacheItemOutdated(memoryCacheItem))
            {
                return memoryCacheItem.Texts;
            }

            var cacheStamp = await GetStampAsync(resourceName, cultureName);

            if (memoryCacheItem != null &&
                memoryCacheItem.CacheStamp == cacheStamp)
            {
                /* Distributed cache was not changed, so we can continue to use the in-memory data */
                memoryCacheItem.LastCheckTime = DateTime.Now;
                return memoryCacheItem.Texts;
            }

            var textsCacheItem = await TextsDistributedCache.GetAsync(textsCacheKey);
            if (textsCacheItem != null)
            {
                /* Updating in-memory data from the distributed cache and returning it */

                MemoryCache[textsCacheKey] = new MemoryCacheItem(
                    textsCacheItem.Dictionary,
                    cacheStamp
                );

                return textsCacheItem.Dictionary;
            }

            /* We are using DistributedLock to ensure only one process
             * is performing the database query and updating the distributed cache.
             */

            await using (var handle = await DistributedLock.TryAcquireAsync(
                             textsCacheKey + "_TextsLock", TimeSpan.FromMinutes(1))) //TODO: Configurable
            {
                if (handle == null)
                {
                    /* This request will fail */
                    throw new AbpException(
                        "Could not acquire distributed lock for getting localization items: " + textsCacheKey
                    );
                }

                /* Refresh the stamp and text since it might be changed in the distributed cache
                 * while we were waiting for the lock.
                 */

                cacheStamp = await GetStampAsync(resourceName, cultureName);
                textsCacheItem = await TextsDistributedCache.GetAsync(textsCacheKey);

                if (textsCacheItem != null)
                {
                    /* Another process filled the distributed cache, we can use it */

                    MemoryCache[textsCacheKey] = new MemoryCacheItem(
                        textsCacheItem.Dictionary,
                        cacheStamp
                    );

                    return textsCacheItem.Dictionary;
                }

                /* Now, it is our own responsibility to update the distributed cache */

                textsCacheItem = new ExternalLocalizationTextCacheItem(await factory());

                await TextsDistributedCache.SetAsync(
                    textsCacheKey,
                    textsCacheItem,
                    CreateDistributedCacheEntryOptions()
                );

                var cacheStampItem = ExternalLocalizationTextCacheStampItem.Create();
                await StampsDistributedCache.SetAsync(
                    GetStampCacheKey(resourceName, cultureName),
                    cacheStampItem
                );

                MemoryCache[textsCacheKey] = new MemoryCacheItem(
                    textsCacheItem.Dictionary,
                    cacheStampItem.Stamp
                );

                return textsCacheItem.Dictionary;
            }
        }
    }

    private static bool IsCacheItemOutdated(MemoryCacheItem memoryCacheItem)
    {
        return DateTime.Now.Subtract(memoryCacheItem.LastCheckTime).TotalSeconds >= 30; //TODO: Configurable
    }

    private async Task<string> GetStampAsync(string resourceName, string cultureName)
    {
        var stampCacheKey = GetStampCacheKey(resourceName, cultureName);
        var stampInDistributedCache = await StampsDistributedCache.GetAsync(stampCacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache.Stamp;
        }

        await using (var handle = await DistributedLock.TryAcquireAsync(
                         stampCacheKey + "_StampLock",
                         TimeSpan.FromMinutes(1))) //TODO: Configurable
        {
            if (handle == null)
            {
                /* This request will fail */
                throw new AbpException(
                    "Could not acquire distributed lock for updating localization stamp: " + stampCacheKey
                );
            }

            stampInDistributedCache = await StampsDistributedCache.GetAsync(stampCacheKey);
            if (stampInDistributedCache != null)
            {
                return stampInDistributedCache.Stamp;
            }

            stampInDistributedCache = ExternalLocalizationTextCacheStampItem.Create();

            await StampsDistributedCache.SetAsync(
                stampCacheKey,
                stampInDistributedCache,
                CreateDistributedCacheEntryOptions()
            );
        }

        return stampInDistributedCache.Stamp;
    }

    public virtual async Task InvalidateAsync(string resourceName, string cultureName)
    {
        var textsCacheKey = ExternalLocalizationTextCacheItem.GetCacheKey(resourceName, cultureName);
        await using (await DistributedLock.TryAcquireAsync(
                         textsCacheKey + "_TextsLock", TimeSpan.FromSeconds(10))) //TODO: Configurable
        {
            /* Getting the handle is not so critical at that point,
             * we can still invalidate the cache item even if we can not get the lock.
             */

            await TextsDistributedCache.RemoveAsync(textsCacheKey);

            await StampsDistributedCache.SetAsync(
                ExternalLocalizationTextCacheStampItem.GetCacheKey(resourceName, cultureName),
                ExternalLocalizationTextCacheStampItem.Create()
            );
        }
    }

    protected virtual DistributedCacheEntryOptions CreateDistributedCacheEntryOptions()
    {
        return new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) //TODO: Configurable
        };
    }

    protected virtual string GetTextsCacheKey(string resourceName, string cultureName)
    {
        return ExternalLocalizationTextCacheItem
            .GetCacheKey(resourceName, cultureName);
    }

    protected virtual string GetStampCacheKey(string resourceName, string cultureName)
    {
        return ExternalLocalizationTextCacheStampItem
            .GetCacheKey(resourceName, cultureName);
    }

    [Serializable]
    [IgnoreMultiTenancy]
    [CacheName("AbpExternalLocalizationTexts")]
    public class ExternalLocalizationTextCacheItem
    {
        public Dictionary<string, string> Dictionary { get; set; }

        public ExternalLocalizationTextCacheItem()
        {
        }

        public ExternalLocalizationTextCacheItem(Dictionary<string, string> dictionary)
        {
            Dictionary = Check.NotNull(dictionary, nameof(dictionary));
        }

        public static string GetCacheKey(string resourceName, string cultureName)
        {
            return resourceName + ":" + cultureName;
        }
    }

    [Serializable]
    [IgnoreMultiTenancy]
    [CacheName("AbpExternalLocalizationTextCacheStamps")]
    public class ExternalLocalizationTextCacheStampItem
    {
        public string Stamp { get; set; }

        public ExternalLocalizationTextCacheStampItem()
        {
        }

        public ExternalLocalizationTextCacheStampItem(string stamp)
        {
            Stamp = stamp;
        }

        public static ExternalLocalizationTextCacheStampItem Create()
        {
            return new ExternalLocalizationTextCacheStampItem(Guid.NewGuid().ToString());
        }

        public static string GetCacheKey(string resourceName, string cultureName)
        {
            return resourceName + ":" + cultureName;
        }
    }

    public class MemoryCacheItem
    {
        public Dictionary<string, string> Texts { get; }

        public string CacheStamp { get; }

        public DateTime LastCheckTime { get; set; }

        public MemoryCacheItem(Dictionary<string, string> texts, string cacheStamp)
        {
            Texts = texts;
            CacheStamp = cacheStamp;
            LastCheckTime = DateTime.Now;
        }
    }
}