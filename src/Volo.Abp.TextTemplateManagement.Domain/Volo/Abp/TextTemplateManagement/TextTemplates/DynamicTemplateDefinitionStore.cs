using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.TextTemplating;
using Volo.Abp.Threading;
using Volo.Abp.TextTemplateManagement;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

[Dependency(ReplaceServices = true)]
public class DynamicTemplateDefinitionStore : IDynamicTemplateDefinitionStore, ITransientDependency
{
    protected ITextTemplateDefinitionRecordRepository TextTemplateRepository { get; }
    protected ITextTemplateDefinitionSerializer TextTemplateDefinitionSerializer { get; }
    protected IDynamicTextTemplateDefinitionStoreInMemoryCache StoreCache { get; }
    protected IDistributedCache DistributedCache { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    public TextTemplateManagementOptions TemplateManagementOptions { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }

    public DynamicTemplateDefinitionStore(
        ITextTemplateDefinitionRecordRepository textTemplateRepository,
        ITextTemplateDefinitionSerializer textTemplateDefinitionSerializer,
        IDynamicTextTemplateDefinitionStoreInMemoryCache storeCache,
        IDistributedCache distributedCache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IOptions<TextTemplateManagementOptions> templateManagementOptions,
        IAbpDistributedLock distributedLock)
    {
        TextTemplateRepository = textTemplateRepository;
        TextTemplateDefinitionSerializer = textTemplateDefinitionSerializer;
        StoreCache = storeCache;
        DistributedCache = distributedCache;
        DistributedLock = distributedLock;
        TemplateManagementOptions = templateManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
    }

    public virtual async Task<TemplateDefinition> GetAsync(string name)
    {
        var template = await GetOrNullAsync(name);
        if (template == null)
        {
            throw new AbpException("Undefined template: " + name);
        }

        return template;
    }

    public virtual async Task<TemplateDefinition> GetOrNullAsync(string name)
    {
        if (!TemplateManagementOptions.IsDynamicTemplateStoreEnabled)
        {
            return null;
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetTemplateOrNull(name);
        }
    }

    public virtual async Task<IReadOnlyList<TemplateDefinition>> GetAllAsync()
    {
        if (!TemplateManagementOptions.IsDynamicTemplateStoreEnabled)
        {
            return Array.Empty<TemplateDefinition>();
        }

        using (await StoreCache.SyncSemaphore.LockAsync())
        {
            await EnsureCacheIsUptoDateAsync();
            return StoreCache.GetTemplates().ToImmutableList();
        }
    }

    protected virtual async Task EnsureCacheIsUptoDateAsync()
    {
        if (StoreCache.LastCheckTime.HasValue &&
            DateTime.Now.Subtract(StoreCache.LastCheckTime.Value).TotalSeconds < 30)
        {
            /* We get the latest template with a small delay for optimization */
            return;
        }

        var stampInDistributedCache = await GetOrSetStampInDistributedCache();

        if (stampInDistributedCache == StoreCache.CacheStamp)
        {
            StoreCache.LastCheckTime = DateTime.Now;
            return;
        }

        await UpdateInMemoryStoreCache();

        StoreCache.CacheStamp = stampInDistributedCache;
        StoreCache.LastCheckTime = DateTime.Now;
    }

    protected virtual async Task UpdateInMemoryStoreCache()
    {
        var templateRecords = await TextTemplateRepository.GetListAsync();
        await StoreCache.FillAsync(templateRecords);
    }

    protected virtual async Task<string> GetOrSetStampInDistributedCache()
    {
        var cacheKey = GetCommonStampCacheKey();

        var stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
        if (stampInDistributedCache != null)
        {
            return stampInDistributedCache;
        }

        await using (var commonLockHandle = await DistributedLock.TryAcquireAsync(GetCommonDistributedLockKey(), TimeSpan.FromMinutes(2)))
        {
            if (commonLockHandle == null)
            {
                /* This request will fail */
                throw new AbpException(
                    "Could not acquire distributed lock for template definition common stamp check!"
                );
            }

            stampInDistributedCache = await DistributedCache.GetStringAsync(cacheKey);
            if (stampInDistributedCache != null)
            {
                return stampInDistributedCache;
            }

            stampInDistributedCache = Guid.NewGuid().ToString();

            await DistributedCache.SetStringAsync(
                cacheKey,
                stampInDistributedCache,
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
                }
            );
        }

        return stampInDistributedCache;
    }

    protected virtual string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryTextTemplateCacheStamp";
    }

    protected virtual string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpTextTemplateUpdateLock";
    }
}
