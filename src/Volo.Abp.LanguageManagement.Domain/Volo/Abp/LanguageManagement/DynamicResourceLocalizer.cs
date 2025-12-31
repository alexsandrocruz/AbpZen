using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.LanguageManagement;

public class DynamicResourceLocalizer : IDynamicResourceLocalizer, ISingletonDependency
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }
    protected IDistributedCache<LanguageTextCacheItem> Cache { get; }

    public DynamicResourceLocalizer(IServiceScopeFactory serviceScopeFactory, IDistributedCache<LanguageTextCacheItem> cache)
    {
        ServiceScopeFactory = serviceScopeFactory;
        Cache = cache;
    }

    public virtual LocalizedString GetOrNull(LocalizationResourceBase resource, string cultureName, string name)
    {
        var cacheItem = GetCacheItem(resource, cultureName);
        var value = cacheItem.Dictionary.GetOrDefault(name);
        if (value == null)
        {
            return null;
        }

        return new LocalizedString(name, value);
    }

    public virtual void Fill(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var cacheItem = GetCacheItem(resource, cultureName);

        foreach (var item in cacheItem.Dictionary)
        {
            dictionary[item.Key] = new LocalizedString(item.Key, item.Value);
        }
    }

    public async Task FillAsync(LocalizationResourceBase resource, string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var cacheItem = await GetCacheItemAsync(resource, cultureName);

        foreach (var item in cacheItem.Dictionary)
        {
            dictionary[item.Key] = new LocalizedString(item.Key, item.Value);
        }
    }

    protected virtual LanguageTextCacheItem GetCacheItem(LocalizationResourceBase resource, string cultureName)
    {
        return Cache.GetOrAdd(
            LanguageTextCacheItem.CalculateCacheKey(resource.ResourceName, cultureName),
            () => CreateCacheItem(resource, cultureName)
        );
    }
    
    protected virtual Task<LanguageTextCacheItem> GetCacheItemAsync(LocalizationResourceBase resource, string cultureName)
    {
        return Cache.GetOrAddAsync(
            LanguageTextCacheItem.CalculateCacheKey(resource.ResourceName, cultureName),
            () => CreateCacheItemAsync(resource, cultureName)
        );
    }

    protected virtual LanguageTextCacheItem CreateCacheItem(LocalizationResourceBase resource, string cultureName)
    {
        var cacheItem = new LanguageTextCacheItem();

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var texts = scope.ServiceProvider
                .GetRequiredService<ILanguageTextRepository>()
                .GetList(resource.ResourceName, cultureName);

            foreach (var text in texts)
            {
                cacheItem.Dictionary[text.Name] = text.Value;
            }
        }

        return cacheItem;
    }
    
    protected virtual async Task<LanguageTextCacheItem> CreateCacheItemAsync(LocalizationResourceBase resource, string cultureName)
    {
        var cacheItem = new LanguageTextCacheItem();

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var texts = await scope.ServiceProvider
                .GetRequiredService<ILanguageTextRepository>()
                .GetListAsync(resource.ResourceName, cultureName);

            foreach (var text in texts)
            {
                cacheItem.Dictionary[text.Name] = text.Value;
            }
        }

        return cacheItem;
    }
}
