using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace Volo.Abp.LanguageManagement.External;

public class ExternalLocalizationResourceContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => false;

    protected LocalizationResourceBase Resource { get; private set; } = null!;
    protected ILocalizationTextRecordRepository LocalizationTextRecordRepository { get; private set; } = null!;
    protected ILocalizationResourceRecordRepository LocalizationResourceRecordRepository { get; private set; } = null!;
    protected IExternalLocalizationTextCache ExternalLocalizationTextCache { get; private set; } = null!;
    protected IExternalLocalizationStoreCache ExternalLocalizationStoreCache { get; private set; } = null!;
    
    public void Initialize(LocalizationResourceInitializationContext context)
    {
        Resource = context.Resource;
        LocalizationResourceRecordRepository = context
            .ServiceProvider
            .GetRequiredService<ILocalizationResourceRecordRepository>();
        LocalizationTextRecordRepository = context
            .ServiceProvider
            .GetRequiredService<ILocalizationTextRecordRepository>();
        ExternalLocalizationTextCache = context
            .ServiceProvider
            .GetRequiredService<IExternalLocalizationTextCache>();
        ExternalLocalizationStoreCache = context
            .ServiceProvider
            .GetRequiredService<IExternalLocalizationStoreCache>();
    }

    public LocalizedString? GetOrNull(string cultureName, string name)
    {
        var cachedDictionary = GetCachedDictionary(cultureName);
        var value = cachedDictionary.GetOrDefault(name);
        return value != null
            ? new LocalizedString(name, value)
            : null;
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        FillDictionary(dictionary, GetCachedDictionary(cultureName));
    }

    public async Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        var cachedDictionary = await GetCachedDictionaryAsync(cultureName);
        FillDictionary(dictionary, cachedDictionary);
    }

    private Dictionary<string, string> GetCachedDictionary(string cultureName)
    {
        /* We are trying to avoid using AsyncHelper if possible.
         * So, first we try to get the dictionary from the memory cache.
         * Checking from cache doesn't require async operations.
         * This will be a good performance gain in a high load environment.
         * */
        var fromCache = ExternalLocalizationTextCache.TryGetTextsFromCache(Resource.ResourceName, cultureName);
        if (fromCache != null)
        {
            return fromCache;
        }
        
        return AsyncHelper.RunSync(() => GetCachedDictionaryAsync(cultureName));
    }
    
    private async Task<Dictionary<string, string>> GetCachedDictionaryAsync(string cultureName)
    {
        var cachedDictionary = await ExternalLocalizationTextCache.GetTextsAsync(
            Resource.ResourceName,
            cultureName,
            async () =>
            {
                var record = await LocalizationTextRecordRepository.FindAsync(Resource.ResourceName, cultureName);
                if (record == null || record.Value.IsNullOrWhiteSpace())
                {
                    return new Dictionary<string, string>();
                }
                
                return JsonSerializer.Deserialize<Dictionary<string, string>>(record.Value) ?? 
                       new Dictionary<string, string>();
            }
        );
        return cachedDictionary;
    }

    private static void FillDictionary(
        Dictionary<string, LocalizedString> targetDictionary, 
        Dictionary<string, string> sourceDictionary)
    {
        foreach (var cachedItem in sourceDictionary)
        {
            targetDictionary[cachedItem.Key] = new LocalizedString(cachedItem.Key, cachedItem.Value);
        }
    }

    public async Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        var cacheItem = await ExternalLocalizationStoreCache.GetResourceCacheItemAsync(Resource.ResourceName, async () =>
        {
            var resourceRecord = await LocalizationResourceRecordRepository.FindAsync(Resource.ResourceName);
            return ExternalLocalizationStoreCache.CreateResourceCacheItem(resourceRecord);
        });
        
        if (!cacheItem.IsAvailable)
        {
            return Array.Empty<string>();
        }

        return cacheItem.SupportedCultures;
    }
}