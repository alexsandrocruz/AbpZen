using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Localization.External;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.LanguageManagement.External;

public class ExternalLocalizationSaver : IExternalLocalizationSaver, ITransientDependency
{
    public ILogger<ExternalLocalizationSaver> Logger { get; set; }
    protected ILocalizationResourceRecordRepository LocalizationResourceRecordRepository { get; }
    protected ILocalizationTextRecordRepository LocalizationTextRecordRepository { get; }
    protected AbpLocalizationOptions LocalizationOptions { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }
    protected ILanguageProvider LanguageProvider { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IExternalLocalizationTextCache ExternalLocalizationTextCache { get; }
    protected IExternalLocalizationStore ExternalLocalizationStore { get; }
    protected IExternalLocalizationStoreCache ExternalLocalizationStoreCache { get; }
    protected IDistributedCache<ResourceHashCacheItem> HashCache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    public ExternalLocalizationSaver(
        IOptions<AbpLocalizationOptions> localizationOptions,
        ILocalizationResourceRecordRepository localizationResourceRecordRepository,
        IStringLocalizerFactory stringLocalizerFactory,
        ILocalizationTextRecordRepository localizationTextRecordRepository,
        IGuidGenerator guidGenerator,
        ILanguageProvider languageProvider,
        IExternalLocalizationTextCache externalLocalizationTextCache,
        IExternalLocalizationStore externalLocalizationStore,
        IDistributedCache<ResourceHashCacheItem> hashCache,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IExternalLocalizationStoreCache externalLocalizationStoreCache,
        IUnitOfWorkManager unitOfWorkManager)
    {
        LocalizationResourceRecordRepository = localizationResourceRecordRepository;
        StringLocalizerFactory = stringLocalizerFactory;
        LocalizationTextRecordRepository = localizationTextRecordRepository;
        GuidGenerator = guidGenerator;
        LanguageProvider = languageProvider;
        ExternalLocalizationTextCache = externalLocalizationTextCache;
        ExternalLocalizationStore = externalLocalizationStore;
        HashCache = hashCache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        ExternalLocalizationStoreCache = externalLocalizationStoreCache;
        UnitOfWorkManager = unitOfWorkManager;
        CacheOptions = cacheOptions.Value;
        LocalizationOptions = localizationOptions.Value;

        Logger = NullLogger<ExternalLocalizationSaver>.Instance;
    }

    public async Task SaveAsync()
    {
        Logger.LogDebug("Waiting to acquire the distributed lock for saving external localizations...");

        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );

        if (applicationLockHandle == null)
        {
            /* Another application instance is already doing it */
            return;
        }

        Logger.LogInformation("Saving external localizations...");

        await using var commonLockHandle = await DistributedLock.TryAcquireAsync(
            GetCommonDistributedLockKey(),
            TimeSpan.FromMinutes(5)
        );

        if (commonLockHandle == null)
        {
            throw new AbpException("Could not acquire distributed lock for saving external localizations!");
        }

        using (var unitOfWork = UnitOfWorkManager.Begin(isTransactional: true, requiresNew: true))
        {
            try
            {
                var context = await CreateSaveContextAsync();

                var resourceRecords = await LocalizationResourceRecordRepository.GetListAsync();
                var textRecords = await LocalizationTextRecordRepository.GetListAsync();
                foreach (var resource in context.Resources)
                {
                    await UpdateResourceChangesAsync(context, resource, resourceRecords, textRecords);
                }

                await UpdateDatabaseAsync(context);
                await InvalidateTextCacheAsync(context);
                await InvalidateResourceCacheAsync(context);
                await SetResourcesHashInCacheAsync(context);
            }
            catch
            {
                try
                {
                    await unitOfWork.RollbackAsync();
                }
                catch
                {
                    /* ignored */
                }

                throw;
            }

            await unitOfWork.CompleteAsync();
        }

        Logger.LogInformation("Completed to save external localizations.");
    }

    private async Task<SaveContext> CreateSaveContextAsync()
    {
        // LanguageProvider already caches, no need to care about performance here
        var applicationLanguages = await LanguageProvider.GetLanguagesAsync();

        var context = new SaveContext(
            applicationLanguages,
            GetStaticResourcesInApplication()
        );

        foreach (var resource in context.Resources)
        {
            var localizer = StringLocalizerFactory.Create(resource.ResourceType);
            context.Localizers[resource.ResourceName] = localizer;

            var supportedCultures = (await localizer.GetSupportedCulturesAsync()).ToArray();
            context.SupportedCultures[resource.ResourceName] = supportedCultures;
        }

        context.ResourcesHash = GetResourcesHash(context);
        var cachedResourcesHash = await GetCachedResourcesHashAsync();
        context.ResourcesShouldBeUpdated = context.ResourcesHash != cachedResourcesHash;

        return context;
    }

    private static string GetResourcesHash(SaveContext context)
    {
        var resourcesForHashing = context
            .Resources
            .Select(x =>
                new {
                    Name = x.ResourceName,
                    BaseResourceNames = x.BaseResourceNames.ToArray(),
                    DefaultCultureName = x.DefaultCultureName,
                    SupportedCultures = context.SupportedCultures[x.ResourceName]
                }
            )
            .ToArray();

        return JsonSerializer
            .Serialize(resourcesForHashing)
            .ToMd5();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpExternalLocalizationSaving";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_CommonAbpExternalLocalizationSaving";
    }

    private LocalizationResource[] GetStaticResourcesInApplication()
    {
        return LocalizationOptions.Resources.Values.OfType<LocalizationResource>().ToArray();
    }

    private async Task UpdateResourceChangesAsync(SaveContext context, LocalizationResource resource, List<LocalizationResourceRecord> resourceRecords, List<LocalizationTextRecord> textRecords)
    {
        var localizer = StringLocalizerFactory.Create(resource.ResourceType);
        var supportedCultures = (await localizer.GetSupportedCulturesAsync()).ToArray();

        if (context.ResourcesShouldBeUpdated)
        {

            await AddOrUpdateResourceAsync(
                context,
                resource,
                supportedCultures,
                resourceRecords
            );
        }

        var compatibleCultures = GetCompatibleCultures(
            context,
            resource,
            supportedCultures
        );

        foreach (var cultureName in compatibleCultures)
        {
            await UpdateResourceTextChangesAsync(
                context,
                resource,
                cultureName,
                localizer,
                textRecords
            );
        }
    }

    private Task AddOrUpdateResourceAsync(
        SaveContext context,
        LocalizationResource resource,
        string[] supportedCultures,
        List<LocalizationResourceRecord> resourceRecords)
    {
        var existingResourceRecord = resourceRecords.FirstOrDefault(x => x.Name == resource.ResourceName);

        if (existingResourceRecord == null)
        {
            context.NewResourceRecords.Add(
                new LocalizationResourceRecord(resource, supportedCultures)
            );
        }
        else if (existingResourceRecord.TryUpdate(resource, supportedCultures))
        {
            context.ChangedResourceRecords.Add(existingResourceRecord);
        }

        return Task.CompletedTask;
    }

    private async Task InvalidateResourceCacheAsync(SaveContext context)
    {
        if (context.NewResourceRecords.Any() || context.ChangedResourceRecords.Any())
        {
            /* It is also needed to invalidate the store cache also for new records
             * because ExternalLocalizationStore stores a list of all the records in the cache,
             * so it should be updated.
             */

            await ExternalLocalizationStoreCache.InvalidateAsync(
                context.ChangedResourceRecords.Select(x => x.Name)
            );
        }
    }

    private async Task InvalidateTextCacheAsync(SaveContext context)
    {
        var affectedResources = context.NewTextRecords
            .Select(x => new { x.ResourceName, x.CultureName })
            .Distinct()
            .Union(
                context.ChangedTextRecords
                    .Select(x => new { x.ResourceName, x.CultureName })
                    .Distinct()
            );

        foreach (var affectedResource in affectedResources)
        {
            await ExternalLocalizationTextCache.InvalidateAsync(affectedResource.ResourceName,
                affectedResource.CultureName);
        }
    }

    private async Task UpdateDatabaseAsync(SaveContext context)
    {
        if (context.NewResourceRecords.Any())
        {
            await LocalizationResourceRecordRepository.InsertManyAsync(context.NewResourceRecords);
        }

        if (context.ChangedResourceRecords.Any())
        {
            await LocalizationResourceRecordRepository.UpdateManyAsync(context.ChangedResourceRecords);
        }

        if (context.NewTextRecords.Any())
        {
            await LocalizationTextRecordRepository.InsertManyAsync(context.NewTextRecords);
        }

        if (context.ChangedTextRecords.Any())
        {
            await LocalizationTextRecordRepository.UpdateManyAsync(context.ChangedTextRecords);
        }
    }

    private async Task UpdateResourceTextChangesAsync(
        SaveContext context,
        LocalizationResource resource,
        string cultureName,
        IStringLocalizer localizer,
        List<LocalizationTextRecord> textRecords)
    {
        using (CultureHelper.Use(cultureName))
        {
            var localizedStrings = (await localizer.GetAllStringsAsync(
                includeParentCultures: false,
                includeBaseLocalizers: false,
                includeDynamicContributors: false
            )).ToArray();

            if (localizedStrings.Length <= 0)
            {
                return;
            }

            var calculatedHash = CalculateHash(localizedStrings);
            var hashInCache = await GetTextsHashInCacheAsync(resource.ResourceName, cultureName);

            if (calculatedHash == hashInCache)
            {
                /* No text changes after the last update */
                return;
            }

            var hasChange = false;
            Dictionary<string, string>? textsInDatabase;

            var record = textRecords.FirstOrDefault(x => x.ResourceName == resource.ResourceName && x.CultureName == cultureName);
            if (record != null)
            {
                textsInDatabase = JsonSerializer.Deserialize<Dictionary<string, string>>(record.Value) ??
                                  new Dictionary<string, string>();
            }
            else
            {
                textsInDatabase = new Dictionary<string, string>();
                hasChange = true;
            }

            foreach (var localizedString in localizedStrings)
            {
                if (textsInDatabase.TryGetValue(localizedString.Name, out var existingValue))
                {
                    if (existingValue != localizedString.Value)
                    {
                        textsInDatabase[localizedString.Name] = localizedString.Value;
                        hasChange = true;
                    }
                }
                else
                {
                    textsInDatabase.Add(localizedString.Name, localizedString.Value);
                    hasChange = true;
                }
            }

            if (hasChange)
            {
                var value = JsonSerializer.Serialize(
                    textsInDatabase,
                    new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.All)
                    });

                if (record == null)
                {
                    context.NewTextRecords.Add(
                        new LocalizationTextRecord(
                            GuidGenerator.Create(),
                            resource.ResourceName,
                            cultureName,
                            value
                        )
                    );
                }
                else
                {
                    record.Value = value;
                    context.ChangedTextRecords.Add(record);
                }
            }

            await SetTextsHashInCacheAsync(resource.ResourceName, cultureName, calculatedHash);
        }
    }

    private async Task<string?> GetTextsHashInCacheAsync(string resourceName, string cultureName)
    {
        return (await HashCache.GetAsync(GetTextsHashCacheKey(resourceName, cultureName)))?.Hash;
    }

    private async Task SetTextsHashInCacheAsync(string resourceName, string cultureName, string hash)
    {
        await HashCache.SetAsync(
            GetTextsHashCacheKey(resourceName, cultureName),
            new ResourceHashCacheItem { Hash = hash },
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30)
            },
            hideErrors: true,
            considerUow: true
        );
    }

    private async Task SetResourcesHashInCacheAsync(SaveContext context)
    {
        if (!context.ResourcesShouldBeUpdated)
        {
            return;
        }

        await HashCache.SetAsync(
            GetResourcesHashCacheKey(),
            new ResourceHashCacheItem { Hash = context.ResourcesHash },
            new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30)
            },
            hideErrors: true,
            considerUow: true
        );
    }

    private string GetTextsHashCacheKey(string resourceName, string cultureName)
    {
        return $"{ApplicationInfoAccessor.ApplicationName}_{resourceName}_{cultureName}_AbpTextsHash";
    }

    private string CalculateHash(IEnumerable<LocalizedString> localizedStrings)
    {
        var nameValueArray = localizedStrings
            .Select(x => new NameValue(x.Name, x.Value))
            .ToArray();

        return JsonSerializer
            .Serialize(nameValueArray)
            .ToMd5();
    }

    private async Task<string?> GetCachedResourcesHashAsync()
    {
        return (await HashCache.GetAsync(GetResourcesHashCacheKey()))?.Hash;
    }

    private string GetResourcesHashCacheKey()
    {
        return $"{ApplicationInfoAccessor.ApplicationName}_AbpResourcesHash";
    }

    protected virtual IEnumerable<string> GetCompatibleCultures(
        SaveContext context,
        LocalizationResource resource,
        IEnumerable<string> supportedCultures)
    {
        return supportedCultures
            .Where(cultureName => IsCompatibleCulture(cultureName, context.ApplicationLanguages))
            .Union(new[] { resource.DefaultCultureName })
            .Where(cultureName => !cultureName.IsNullOrEmpty());
    }

    protected virtual bool IsCompatibleCulture(
        string resourceCultureName,
        IReadOnlyList<LanguageInfo> applicationLanguages)
    {
        foreach (var applicationLanguage in applicationLanguages)
        {
            if (IsCompatibleCulture(resourceCultureName, applicationLanguage))
            {
                return true;
            }
        }

        return false;
    }

    protected virtual bool IsCompatibleCulture(
        string resourceCultureName,
        LanguageInfo applicationLanguage)
    {
        return CultureHelper.IsCompatibleCulture(resourceCultureName, applicationLanguage.UiCultureName);
    }

    protected class SaveContext
    {
        public List<LocalizationResourceRecord> NewResourceRecords { get; } = new();
        public List<LocalizationResourceRecord> ChangedResourceRecords { get; } = new();
        public List<LocalizationTextRecord> NewTextRecords { get; } = new();
        public List<LocalizationTextRecord> ChangedTextRecords { get; } = new();
        public IReadOnlyList<LanguageInfo> ApplicationLanguages { get; }
        public LocalizationResource[] Resources { get; }
        public Dictionary<string, string[]> SupportedCultures { get; } = new();
        public Dictionary<string, IStringLocalizer> Localizers { get; } = new();
        public bool ResourcesShouldBeUpdated { get; set; }
        public string ResourcesHash { get; set; }

        public SaveContext(
            IReadOnlyList<LanguageInfo> applicationLanguages,
            LocalizationResource[] resources)
        {
            ApplicationLanguages = applicationLanguages;
            Resources = resources;
        }
    }

    [Serializable]
    [IgnoreMultiTenancy]
    [CacheName("AbpExternalLocalizationSaving")]
    public class ResourceHashCacheItem
    {
        public string Hash { get; set; }
    }
}
