using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class TextTemplateDefinitionSerializer: ITextTemplateDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }
    protected TemplateContentFileProvider TemplateContentFileProvider { get; }

    public TextTemplateDefinitionSerializer(IGuidGenerator guidGenerator, ILocalizableStringSerializer localizableStringSerializer, TemplateContentFileProvider templateContentFileProvider)
    {
        GuidGenerator = guidGenerator;
        LocalizableStringSerializer = localizableStringSerializer;
        TemplateContentFileProvider = templateContentFileProvider;
    }

    public virtual async Task<KeyValuePair<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>>> SerializeAsync(TemplateDefinition template)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var record = new TextTemplateDefinitionRecord(
                GuidGenerator.Create(),
                template.Name,
                LocalizableStringSerializer.Serialize(template.DisplayName),
                template.IsLayout,
                template.Layout,
                template.LocalizationResourceName,
                template.IsInlineLocalized,
                template.DefaultCultureName,
                template.RenderEngine
            );

            foreach (var property in template.Properties)
            {
                record.SetProperty(property.Key, property.Value);
            }

            var files = await TemplateContentFileProvider.GetFilesAsync(template);
            return new KeyValuePair<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>>(record,
                files.Select(file => new TextTemplateDefinitionContentRecord(
                    GuidGenerator.Create(),
                    record.Id,
                    file.FileName,
                    file.FileContent
                )).ToList());
        }
    }

    public virtual async Task<Dictionary<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>>> SerializeAsync(IEnumerable<TemplateDefinition> templates)
    {
        var list = new Dictionary<TextTemplateDefinitionRecord, List<TextTemplateDefinitionContentRecord>>();
        foreach (var template in templates)
        {
            var keyValuePair = await SerializeAsync(template);
            list.Add(keyValuePair.Key, keyValuePair.Value);
        }
        return list;
    }
}
