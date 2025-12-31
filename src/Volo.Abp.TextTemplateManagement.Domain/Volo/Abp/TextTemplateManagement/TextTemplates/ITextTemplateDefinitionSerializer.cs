using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.TextTemplating;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public interface ITextTemplateDefinitionSerializer
{
    Task<KeyValuePair<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>>> SerializeAsync(TemplateDefinition template);

    Task<Dictionary<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>>> SerializeAsync(IEnumerable<TemplateDefinition> templates);
}
