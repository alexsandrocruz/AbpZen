using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplateManagement;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class DatabaseTemplateContentContributor : ITemplateContentContributor, ITransientDependency
{
    protected readonly ITextTemplateContentRepository ContentRepository;
    protected readonly IStaticTemplateDefinitionStore StaticTemplateDefinitionStore;
    protected readonly IDynamicTemplateDefinitionStore DynamicTemplateDefinitionStore;
    protected readonly ITextTemplateDefinitionContentRecordRepository TextTemplateDefinitionContentRecordRepository;
    protected readonly IDistributedCache<string, TemplateContentCacheKey> Cache;

    protected readonly TextTemplateManagementOptions Options;

    public DatabaseTemplateContentContributor(
        ITextTemplateContentRepository contentRepository,
        StaticTemplateDefinitionStore staticTemplateDefinitionStore,
        IDynamicTemplateDefinitionStore dynamicTemplateDefinitionStore,
        ITextTemplateDefinitionContentRecordRepository textTemplateDefinitionContentRecordRepository,
        IDistributedCache<string, TemplateContentCacheKey> cache,
        IOptions<TextTemplateManagementOptions> options)
    {
        ContentRepository = contentRepository;
        StaticTemplateDefinitionStore = staticTemplateDefinitionStore;
        DynamicTemplateDefinitionStore = dynamicTemplateDefinitionStore;
        TextTemplateDefinitionContentRecordRepository = textTemplateDefinitionContentRecordRepository;
        Cache = cache;
        Options = options.Value;
    }

    public virtual async Task<string> GetOrNullAsync(TemplateContentContributorContext context)
    {
        return await Cache.GetOrAddAsync(
            new TemplateContentCacheKey(context.TemplateDefinition.Name, context.Culture),
            async () => await GetTemplateContentFromDbOrNullAsync(context),
            () => new DistributedCacheEntryOptions
            {
                SlidingExpiration = Options.MinimumCacheDuration
            }
        );
    }

    protected virtual async Task<string> GetTemplateContentFromDbOrNullAsync(TemplateContentContributorContext context)
    {
        var template = await ContentRepository.FindAsync(
            context.TemplateDefinition.Name,
            context.Culture
        );

        if (template != null)
        {
            return template.Content;
        }

        var templateDefinition = await StaticTemplateDefinitionStore.GetOrNullAsync(context.TemplateDefinition.Name);
        if (templateDefinition != null)
        {
            // Skip if the template is static.
            return null;
        }

        templateDefinition = await DynamicTemplateDefinitionStore.GetOrNullAsync(context.TemplateDefinition.Name);
        if (templateDefinition != null)
        {
            var templateDefinitionContentRecord = await TextTemplateDefinitionContentRecordRepository.FindByDefinitionNameAsync(templateDefinition.Name, context.Culture);
            if (templateDefinitionContentRecord != null)
            {
                return templateDefinitionContentRecord.FileContent;
            }

            templateDefinitionContentRecord = await TextTemplateDefinitionContentRecordRepository.FindByDefinitionNameAsync(templateDefinition.Name, templateDefinition.DefaultCultureName ?? "en");
            if (templateDefinitionContentRecord != null)
            {
                return templateDefinitionContentRecord.FileContent;
            }

            templateDefinitionContentRecord = await TextTemplateDefinitionContentRecordRepository.FindByDefinitionNameAsync(templateDefinition.Name);
            if (templateDefinitionContentRecord != null)
            {
                return templateDefinitionContentRecord.FileContent;
            }

            throw new AbpException($"Could not find any content for the dynamic template definition: {templateDefinition.Name}.");
        }

        return null;
    }
}
