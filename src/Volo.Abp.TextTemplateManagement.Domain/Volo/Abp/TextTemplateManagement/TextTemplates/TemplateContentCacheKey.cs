using JetBrains.Annotations;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class TemplateContentCacheKey
{
    public string TemplateDefinitionName { get; protected set; }

    public string Culture { get; protected set; }

    public TemplateContentCacheKey([NotNull] string templateDefinitionName, string culture)
    {
        TemplateDefinitionName = Check.NotNullOrWhiteSpace(templateDefinitionName, nameof(templateDefinitionName));
        Culture = culture;
    }

    public override string ToString()
    {
        return $"{TemplateDefinitionName}_{Culture}";
    }
}
