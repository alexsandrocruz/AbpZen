using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;

namespace Volo.Abp.LanguageManagement.External;

[Dependency(ReplaceServices = true)]
public class ExternalLocalizationStore : 
    IExternalLocalizationStore,
    ITransientDependency
{
    protected ILocalizationResourceRecordRepository LocalizationResourceRecordRepository { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IExternalLocalizationStoreCache Cache { get; }

    public ExternalLocalizationStore(
        IOptions<AbpLocalizationOptions> localizationOptions,
        ILocalizationResourceRecordRepository localizationResourceRecordRepository,
        IExternalLocalizationStoreCache cache)
    {
        LocalizationResourceRecordRepository = localizationResourceRecordRepository;
        Cache = cache;
        LocalizationOptions = localizationOptions.Value;
    }

    public virtual LocalizationResourceBase? GetResourceOrNull(string resourceName)
    {
        var cacheItem = GetResourceCacheItem(resourceName);
        if (!cacheItem.IsAvailable)
        {
            return null;
        }

        return CreateNonTypedLocalizationResource(cacheItem);
    }

    public virtual async Task<LocalizationResourceBase?> GetResourceOrNullAsync(string resourceName)
    {
        var cacheItem = await GetResourceCacheItemAsync(resourceName);
        if (!cacheItem.IsAvailable)
        {
            return null;
        }

        return CreateNonTypedLocalizationResource(cacheItem);
    }

    public virtual async Task<string[]> GetResourceNamesAsync()
    {
        return (await GetResourcesAsync())
            .Where(r => !LocalizationOptions.Resources.ContainsKey(r.ResourceName))
            .Select(r => r.ResourceName)
            .ToArray();
    }

    public virtual async Task<LocalizationResourceBase[]> GetResourcesAsync()
    {
        return (await GetAllResourcesCacheItemAsync())
            .Resources
            .Select(CreateNonTypedLocalizationResource)
            .Cast<LocalizationResourceBase>()
            .ToArray();
    }
    
    protected virtual NonTypedLocalizationResource CreateNonTypedLocalizationResource(
        ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem resourceCacheItem)
    {
        Debug.Assert(resourceCacheItem.IsAvailable, "resourceCacheItem.IsAvailable == true");
        
        var resource = new NonTypedLocalizationResource(
            resourceCacheItem.Name,
            resourceCacheItem.DefaultCulture
        );

        if (resourceCacheItem.BaseResources.Length > 0)
        {
            resource.AddBaseResources(resourceCacheItem.BaseResources);
        }

        resource.Contributors.Add(new ExternalLocalizationResourceContributor());

        return resource;
    }
    
    protected virtual async Task<ExternalLocalizationStoreCache.AllLocalizationResourcesCacheItem> GetAllResourcesCacheItemAsync()
    {
        return await Cache.GetAllResourcesCacheItemAsync(
            async () =>
            {
                var cacheItems = (await LocalizationResourceRecordRepository.GetListAsync())
                    .Where(r => !LocalizationOptions.Resources.ContainsKey(r.Name))
                    .Select(CreateResourceCacheItem)
                    .ToArray();

                return new ExternalLocalizationStoreCache.AllLocalizationResourcesCacheItem { Resources = cacheItems };
            }
        );
    }

    protected virtual ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem GetResourceCacheItem(string resourceName)
    {
        return Cache.GetResourceCacheItem(resourceName, () =>
        {
            var resourceRecord = LocalizationResourceRecordRepository.Find(resourceName);
            return CreateResourceCacheItem(resourceRecord);
        });
    }

    protected virtual async Task<ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem> GetResourceCacheItemAsync(string resourceName)
    {
        return await Cache.GetResourceCacheItemAsync(resourceName, async () =>
        {
            var resourceRecord = await LocalizationResourceRecordRepository.FindAsync(resourceName);
            return CreateResourceCacheItem(resourceRecord);
        });
    }

    protected virtual ExternalLocalizationStoreCache.LocalizationResourceRecordCacheItem CreateResourceCacheItem(LocalizationResourceRecord? resourceRecord)
    {
        return Cache.CreateResourceCacheItem(resourceRecord);
    }
}