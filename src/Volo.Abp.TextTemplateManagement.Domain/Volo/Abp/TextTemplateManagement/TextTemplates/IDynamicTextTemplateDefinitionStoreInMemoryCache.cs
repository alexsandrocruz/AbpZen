using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.TextTemplating;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public interface IDynamicTextTemplateDefinitionStoreInMemoryCache
{
    string CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task FillAsync(List<TextTemplateDefinitionRecord> templateRecords);

    TemplateDefinition GetTemplateOrNull(string name);

    IReadOnlyList<TemplateDefinition> GetTemplates();
}
