using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class DynamicTextTemplateDefinitionStoreInMemoryCache : IDynamicTextTemplateDefinitionStoreInMemoryCache, ISingletonDependency
{
    public string CacheStamp { get; set; }

    protected IDictionary<string, TemplateDefinition> TemplateDefinitions { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicTextTemplateDefinitionStoreInMemoryCache(ILocalizableStringSerializer localizableStringSerializer)
    {
        LocalizableStringSerializer = localizableStringSerializer;
        TemplateDefinitions = new Dictionary<string, TemplateDefinition>();
    }

    public Task FillAsync(List<TextTemplateDefinitionRecord> templateRecords)
    {
        TemplateDefinitions.Clear();

        foreach (var record in templateRecords)
        {
            var templateDefinition = new TemplateDefinition(
                record.Name,
                record.LocalizationResourceName,
                record.DisplayName != null ? LocalizableStringSerializer.Deserialize(record.DisplayName) : null,
                record.IsLayout,
                record.Layout,
                record.DefaultCultureName)
            {
                IsInlineLocalized = record.IsInlineLocalized,
                RenderEngine = record.RenderEngine
            };

            foreach (var property in record.ExtraProperties)
            {
                templateDefinition[property.Key] = property.Value;
            }

            TemplateDefinitions[record.Name] = templateDefinition;
        }

        return Task.CompletedTask;
    }

    public TemplateDefinition GetTemplateOrNull(string name)
    {
        return TemplateDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<TemplateDefinition> GetTemplates()
    {
        return TemplateDefinitions.Values.ToList();
    }
}
