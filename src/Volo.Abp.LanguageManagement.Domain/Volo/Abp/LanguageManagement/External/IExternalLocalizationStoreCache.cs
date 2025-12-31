using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.LanguageManagement.External;

public interface IExternalLocalizationStoreCache
{
    Task<ExternalLocalizationStoreCache.AllLocalizationResourcesCacheItem> GetAllResourcesCacheItemAsync(
        Func<Task<ExternalLocalizationStoreCache.AllLocalizationResourcesCacheItem>> factory
    );

    ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem GetResourceCacheItem(
        string resourceName,
        Func<ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem> factory);

    Task<ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem> GetResourceCacheItemAsync(
        string resourceName,
        Func<Task<ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem>> factory);

    ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem CreateResourceCacheItem(LocalizationResourceRecord? resourceRecord);
    
    Task InvalidateAsync(IEnumerable<string> changedResourceNames);
}