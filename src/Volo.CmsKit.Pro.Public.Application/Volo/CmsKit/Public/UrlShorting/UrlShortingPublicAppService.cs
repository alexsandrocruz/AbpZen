using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.Public.UrlShorting;

[RequiresFeature(CmsKitProFeatures.UrlShortingEnable)]
[RequiresGlobalFeature(UrlShortingFeature.Name)]
public class UrlShortingPublicAppService : ApplicationService, IUrlShortingPublicAppService
{
    private readonly IShortenedUrlRepository _shortenedUrlRepository;
    private readonly IDistributedCache<ShortenedUrlCacheItem, string> _shortenedUrlCache;
    private readonly IDistributedCache<List<string>> _regexShortenedUrlCacheKeysCache;
    private readonly static ConcurrentDictionary<string, DateTime> RegexShortenedUrlCacheKeys = new ();
    private const string AllCacheKey = "AllAbpRegexShortenedUrls";
    private readonly UrlShortingOptions _options;

    public UrlShortingPublicAppService(
        IShortenedUrlRepository shortenedUrlRepository,
        IDistributedCache<ShortenedUrlCacheItem, string> shortenedUrlCache, 
        IDistributedCache<List<string>> regexShortenedUrlCacheKeysCache,
        IOptions<UrlShortingOptions> options)
    {
        _shortenedUrlRepository = shortenedUrlRepository;
        _shortenedUrlCache = shortenedUrlCache;
        _regexShortenedUrlCacheKeysCache = regexShortenedUrlCacheKeysCache;
        _options = options.Value;
    }

    public virtual async Task<ShortenedUrlDto> FindBySourceAsync(string source)
    {
        var cacheValue = await FindFromCacheAsync(source);
        if (cacheValue != null)
        {
            return cacheValue.Exists
                ? ObjectMapper.Map<ShortenedUrlCacheItem, ShortenedUrlDto>(cacheValue)
                : null;
        }
        
        var dbValue = await FindFromDbAsync(source);
        await AddToCacheAsync(source, ObjectMapper.Map<ShortenedUrlDto, ShortenedUrlCacheItem>(dbValue));

        return dbValue;
    }

    protected virtual async Task<ShortenedUrlDto> FindFromDbAsync(string source)
    {
        var shortenedUrl = await _shortenedUrlRepository.FindBySourceUrlAsync(source);
        if (shortenedUrl != null)
        {
            return ObjectMapper.Map<ShortenedUrl, ShortenedUrlDto>(shortenedUrl);
        }

        var all = await _shortenedUrlRepository.GetAllRegexAsync();
        await AddCacheManyAsync(all);
            
        var items = all.Where(x => RegexIsMatch(source, x.Source)).ToArray();
        if (items.Length > 1)
        {
            return await _options.OnConflict(new ConflictUrlContext(source, items.Select(x => ObjectMapper.Map<ShortenedUrl, ShortenedUrlDto>(x)).ToArray()));
        }
        return ObjectMapper.Map<ShortenedUrl, ShortenedUrlDto>(items.FirstOrDefault());
    }

    protected virtual async Task<ShortenedUrlCacheItem> FindFromCacheAsync(string source)
    {
        InMemoryCacheRemoveExpired();
        var inMemoryCaches = RegexShortenedUrlCacheKeys.Where(x => RegexIsMatch(source, x.Key)).ToArray();
            
        if (inMemoryCaches.Length <= 1)
        {
            var inMemoryCache = inMemoryCaches.FirstOrDefault().Key;
            var hasInMemoryCache = !inMemoryCache.IsNullOrWhiteSpace();
            var cacheKey = hasInMemoryCache ? inMemoryCache : source;
            var shortenedUrlInDistributedCache = await _shortenedUrlCache.GetAsync(cacheKey);
            if (shortenedUrlInDistributedCache != null)
            {
                if (hasInMemoryCache && !shortenedUrlInDistributedCache.Exists)
                {
                    await RemoveRegexFromCache(inMemoryCache);
                }
                else if (shortenedUrlInDistributedCache.Exists)
                {
                    await AddRegexToCacheAsync(shortenedUrlInDistributedCache);
                }
                
                return shortenedUrlInDistributedCache;
            }
                
            if (hasInMemoryCache)
            {
                await RemoveRegexFromCache(inMemoryCache);
            }
                
            return await FindFromAllRegexCacheAsync(source);
        }


        var distributedCacheItems =
            (
                await _shortenedUrlCache
                    .GetManyAsync(inMemoryCaches.Select(x => x.Key).ToList())
            )
            .Where(x => x.Value != null).ToArray();
        await RemoveRegexManyFromCacheAsync
        (
            distributedCacheItems
                .Where(x => !x.Value.Exists)
                .Select(x => x.Value)
                .ToList()
        );
        if (distributedCacheItems.Length <= 1)
        {
            return distributedCacheItems.FirstOrDefault().Value;
        }

        var conflictUrls = distributedCacheItems.Where(x => x.Value.Exists)
            .Select(x => ObjectMapper.Map<ShortenedUrlCacheItem, ShortenedUrlDto>(x.Value)).ToArray();
        if (conflictUrls.Length <= 1)
        {
            return distributedCacheItems.FirstOrDefault().Value;
        }

        var selected = await _options.OnConflict(new ConflictUrlContext(source, conflictUrls));
        return ObjectMapper.Map<ShortenedUrlDto, ShortenedUrlCacheItem>(selected);
    }

    protected virtual async Task<ShortenedUrlCacheItem> FindFromAllRegexCacheAsync(string source)
    {
        var allRegexCacheKeys = await _regexShortenedUrlCacheKeysCache.GetAsync(AllCacheKey);
        if (allRegexCacheKeys == null || allRegexCacheKeys.Count <= 0)
        {
            return null;
        }
        
        var regexCacheKeys = allRegexCacheKeys.Where(x => RegexIsMatch(source, x)).ToArray();
        
        var cacheItems = (await _shortenedUrlCache.GetManyAsync(regexCacheKeys)).Where(x => x.Value != null).ToArray();
        if (cacheItems.Length > 1)
        {
            var selected = await _options.OnConflict(new ConflictUrlContext(source, cacheItems.Select(x => ObjectMapper.Map<ShortenedUrlCacheItem, ShortenedUrlDto>(x.Value)).ToArray()));
            return ObjectMapper.Map<ShortenedUrlDto, ShortenedUrlCacheItem>(selected);
        }
        
        return cacheItems.FirstOrDefault().Value;
    }

    protected virtual void InMemoryCacheRemoveExpired()
    {
        var now = DateTime.Now;
        RegexShortenedUrlCacheKeys.RemoveAll(x => x.Value < now);
    }
    
    protected virtual async Task AddCacheManyAsync(List<ShortenedUrl> shortenedUrls)
    {
        List<KeyValuePair<string, ShortenedUrlCacheItem>> cacheItems = new();
        foreach (var shortenedUrl in shortenedUrls)
        {
            cacheItems.Add(new KeyValuePair<string, ShortenedUrlCacheItem>(shortenedUrl.Source,
                ObjectMapper.Map<ShortenedUrl, ShortenedUrlCacheItem>(shortenedUrl)));
        }
        
        await AddRegexManyToCacheAsync(shortenedUrls);
        
        await _shortenedUrlCache.SetManyAsync(cacheItems);
    }

    protected virtual async Task AddToCacheAsync(string source, ShortenedUrlCacheItem shortenedUrl)
    {
        if (shortenedUrl == null)
        {
            await _shortenedUrlCache.SetAsync(source,
                new ShortenedUrlCacheItem {Exists = false},
                new DistributedCacheEntryOptions {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                });
            return;
        }
        
        await AddRegexToCacheAsync(shortenedUrl);
        await _shortenedUrlCache.SetAsync(source,
            shortenedUrl,
            new DistributedCacheEntryOptions {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
            });
    }
    
    protected virtual Task AddRegexToCacheAsync(ShortenedUrlCacheItem shortenedUrl)
    {
        if(shortenedUrl == null || !shortenedUrl.IsRegex)
        {
            return Task.CompletedTask;
        }
        
        RegexShortenedUrlCacheKeys.TryAdd(shortenedUrl.Source, DateTime.Now.AddHours(1));
        return AddCacheKeyAsync(shortenedUrl.Source);
    }
    
    protected virtual Task RemoveRegexFromCache(string source)
    {
        RegexShortenedUrlCacheKeys.Remove(source, out _);
        return RemoveCacheKeyAsync(source);
    }
    
    protected virtual Task RemoveRegexManyFromCacheAsync(List<ShortenedUrlCacheItem> shortenedUrls)
    {
        var keys = shortenedUrls.Where(x => x.IsRegex).Select(x => x.Source).ToList();
        foreach (var key in keys)
        {
            RegexShortenedUrlCacheKeys.TryRemove(key, out _);
        }
        return RemoveCacheKeyManyAsync(keys);
    }
    
    protected virtual Task AddRegexManyToCacheAsync(List<ShortenedUrl> shortenedUrls)
    {
        var keys = shortenedUrls.Where(x => x.IsRegex).Select(x => x.Source).ToList();
        foreach (var key in keys)
        {
            RegexShortenedUrlCacheKeys.TryAdd(key, DateTime.Now.AddHours(1));
        }
        return AddCacheKeyManyAsync(keys);
    }
    
    protected virtual async Task AddCacheKeyAsync(string key)
    {
        var oldKeys = await _regexShortenedUrlCacheKeysCache.GetAsync(AllCacheKey) ?? new List<string>();
        oldKeys.AddIfNotContains(key);
        await _regexShortenedUrlCacheKeysCache.SetAsync(AllCacheKey, oldKeys);
    }
    
    protected virtual async Task RemoveCacheKeyAsync(string key)
    {
        var oldKeys = await _regexShortenedUrlCacheKeysCache.GetAsync(AllCacheKey) ?? new List<string>();
        oldKeys.RemoveAll(x => x == key);
        await _regexShortenedUrlCacheKeysCache.SetAsync(AllCacheKey, oldKeys);
    }
    
    protected virtual async Task RemoveCacheKeyManyAsync(List<string> keys)
    {
        var oldKeys = await _regexShortenedUrlCacheKeysCache.GetAsync(AllCacheKey) ?? new List<string>();
        keys.ForEach(x => oldKeys.RemoveAll(y => y == x));
        await _regexShortenedUrlCacheKeysCache.SetAsync(AllCacheKey, oldKeys);
    }
    
    protected virtual async Task AddCacheKeyManyAsync(List<string> keys)
    {
        var oldKeys = await _regexShortenedUrlCacheKeysCache.GetAsync(AllCacheKey) ?? new List<string>();
        keys.ForEach(x => oldKeys.AddIfNotContains(x));
        await _regexShortenedUrlCacheKeysCache.SetAsync(AllCacheKey, oldKeys);
    }
    
    protected virtual bool RegexIsMatch(string source, string target)
    {
        return _options.RegexIgnoreCase
            ? Regex.IsMatch(source, target, RegexOptions.IgnoreCase)
            : Regex.IsMatch(source, target);
    }
}