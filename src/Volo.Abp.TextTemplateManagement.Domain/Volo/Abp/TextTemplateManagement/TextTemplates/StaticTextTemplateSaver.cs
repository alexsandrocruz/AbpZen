using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.VirtualFiles;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class StaticTextTemplateSaver : IStaticTextTemplateSaver, ITransientDependency
{
    protected IStaticTemplateDefinitionStore StaticStore { get; }
    protected ITextTemplateDefinitionRecordRepository TextTemplateRepository { get; }
    protected ITextTemplateDefinitionContentRecordRepository TextTemplateContentRepository { get; }
    protected ITextTemplateDefinitionSerializer TextTemplateSerializer { get; }
    protected IDistributedCache Cache { get; }
    protected IApplicationInfoAccessor ApplicationInfoAccessor { get; }
    protected IAbpDistributedLock DistributedLock { get; }
    protected AbpTextTemplatingOptions TemplateOptions { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }
    protected AbpDistributedCacheOptions CacheOptions { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public StaticTextTemplateSaver(
        IStaticTemplateDefinitionStore staticStore,
        ITextTemplateDefinitionRecordRepository textTemplateRepository,
        ITextTemplateDefinitionContentRecordRepository textTemplateContentRepository,
        ITextTemplateDefinitionSerializer textTemplateSerializer,
        IDistributedCache cache,
        IOptions<AbpDistributedCacheOptions> cacheOptions,
        IApplicationInfoAccessor applicationInfoAccessor,
        IAbpDistributedLock distributedLock,
        IOptions<AbpTextTemplatingOptions> templateManagementOptions,
        ICancellationTokenProvider cancellationTokenProvider,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator)
    {
        StaticStore = staticStore;
        TextTemplateRepository = textTemplateRepository;
        TextTemplateContentRepository = textTemplateContentRepository;
        TextTemplateSerializer = textTemplateSerializer;
        Cache = cache;
        ApplicationInfoAccessor = applicationInfoAccessor;
        DistributedLock = distributedLock;
        CancellationTokenProvider = cancellationTokenProvider;
        TemplateOptions = templateManagementOptions.Value;
        CacheOptions = cacheOptions.Value;
        UnitOfWorkManager = unitOfWorkManager;
        GuidGenerator = guidGenerator;
    }

    [UnitOfWork]
    public async Task SaveAsync()
    {
        await using var applicationLockHandle = await DistributedLock.TryAcquireAsync(
            GetApplicationDistributedLockKey()
        );

        if (applicationLockHandle == null)
        {
            /* Another application instance is already doing it */
            return;
        }

        var cacheKey = GetApplicationHashCacheKey();
        var cachedHash = await Cache.GetStringAsync(cacheKey, CancellationTokenProvider.Token);

        var templateRecords = await TextTemplateSerializer.SerializeAsync(await StaticStore.GetAllAsync());
        var currentHash = CalculateHash(templateRecords, TemplateOptions.DeletedTemplates);

        if (cachedHash == currentHash)
        {
            return;
        }

        await using (var commonLockHandle = await DistributedLock.TryAcquireAsync(
                         GetCommonDistributedLockKey(),
                         TimeSpan.FromMinutes(5)))
        {
            if (commonLockHandle == null)
            {
                /* It will re-try */
                throw new AbpException("Could not acquire distributed lock for saving static templates!");
            }

            using (var unitOfWork = UnitOfWorkManager.Begin(requiresNew: true, isTransactional: true))
            {
                try
                {
                    var hasChangesInTemplates = await UpdateChangedTemplatesAsync(templateRecords);

                    if (hasChangesInTemplates)
                    {
                        await Cache.SetStringAsync(
                            GetCommonStampCacheKey(),
                            Guid.NewGuid().ToString(),
                            new DistributedCacheEntryOptions {
                                SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
                            },
                            CancellationTokenProvider.Token
                        );
                    }
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
        }

        await Cache.SetStringAsync(
            cacheKey,
            currentHash,
            new DistributedCacheEntryOptions {
                SlidingExpiration = TimeSpan.FromDays(30) //TODO: Make it configurable?
            },
            CancellationTokenProvider.Token
        );
    }

    private async Task<bool> UpdateChangedTemplatesAsync(Dictionary<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>> templateRecords)
    {
        var newRecords = new List<TextTemplateDefinitionRecord>();
        var newContentRecords = new List<TextTemplateDefinitionContentRecord>();

        var changedRecords = new List<TextTemplateDefinitionRecord>();

        var templateRecordsInDatabase = (await TextTemplateRepository.GetListAsync()).ToDictionary(x => x.Name);

        foreach (var record in templateRecords)
        {
            var templateRecordInDatabase = templateRecordsInDatabase.GetOrDefault(record.Key.Name);
            if (templateRecordInDatabase == null)
            {
                /* New group */
                newRecords.Add(record.Key);
                newContentRecords.AddRange(record.Value);
                continue;
            }

            /* Always remove and add contents */
            await TextTemplateContentRepository.DeleteByDefinitionIdAsync(record.Key.Id);
            foreach (var contentRecord in record.Value)
            {
                contentRecord.DefinitionId = templateRecordInDatabase.Id;
            }
            newContentRecords.AddRange(record.Value);

            if (record.Key.HasSameData(templateRecordInDatabase))
            {
                /* Not changed */
                continue;
            }

            /* Changed */
            templateRecordInDatabase.Patch(record.Key);
            changedRecords.Add(templateRecordInDatabase);
        }

        /* Deleted */
        var deletedRecords = new List<TextTemplateDefinitionRecord>();

        if (TemplateOptions.DeletedTemplates.Any())
        {
            deletedRecords.AddRange(templateRecordsInDatabase.Values.Where(x => TemplateOptions.DeletedTemplates.Contains(x.Name)));
        }

        if (newRecords.Any())
        {
            await TextTemplateRepository.InsertManyAsync(newRecords);
            await TextTemplateContentRepository.InsertManyAsync(newContentRecords);
        }

        if (changedRecords.Any())
        {
            await TextTemplateRepository.UpdateManyAsync(changedRecords);
        }

        if (deletedRecords.Any())
        {
            await TextTemplateContentRepository.DeleteByDefinitionIdAsync(deletedRecords.Select(x => x.Id).ToArray());
            await TextTemplateRepository.DeleteManyAsync(deletedRecords);
        }

        return newRecords.Any() || changedRecords.Any() || deletedRecords.Any();
    }

    private string GetApplicationDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpTextTemplateUpdateLock";
    }

    private string GetCommonDistributedLockKey()
    {
        return $"{CacheOptions.KeyPrefix}_Common_AbpTextTemplateUpdateLock";
    }

    private string GetApplicationHashCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_{ApplicationInfoAccessor.ApplicationName}_AbpTextTemplatesHash";
    }

    private string GetCommonStampCacheKey()
    {
        return $"{CacheOptions.KeyPrefix}_AbpInMemoryTextTemplateCacheStamp";
    }

    private string CalculateHash(Dictionary<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>> templateRecords, IEnumerable<string> deletedTemplates)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new AbpIgnorePropertiesModifiers<TextTemplateDefinitionRecord, Guid>().CreateModifyAction(x => x.Id),
                    new AbpIgnorePropertiesModifiers<TextTemplateDefinitionContentRecord, Guid>().CreateModifyAction(x => x.Id)
                }
            }
        };

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("TemplateRecords:");
        stringBuilder.AppendLine(JsonSerializer.Serialize(templateRecords.Keys, jsonSerializerOptions));
        stringBuilder.AppendLine(JsonSerializer.Serialize(templateRecords.Values, jsonSerializerOptions));

        stringBuilder.Append("DeletedTemplate:");
        stringBuilder.Append(deletedTemplates.JoinAsString(","));

        return stringBuilder
            .ToString()
            .ToMd5();
    }
}
