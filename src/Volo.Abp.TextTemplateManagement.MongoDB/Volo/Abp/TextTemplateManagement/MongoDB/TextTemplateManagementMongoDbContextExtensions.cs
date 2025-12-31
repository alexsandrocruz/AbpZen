using Volo.Abp.MongoDB;
using Volo.Abp.TextTemplateManagement.TextTemplates;

namespace Volo.Abp.TextTemplateManagement.MongoDB;

public static class TextTemplateManagementMongoDbContextExtensions
{
    public static void ConfigureTextTemplateManagement(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<TextTemplateContent>(b =>
        {
            b.CollectionName = TextTemplateManagementDbProperties.DbTablePrefix + "TextTemplates";
        });

        builder.Entity<TextTemplateDefinitionRecord>(b =>
        {
            b.CollectionName = TextTemplateManagementDbProperties.DbTablePrefix + "TextTemplateDefinitionRecords";
        });

        builder.Entity<TextTemplateDefinitionContentRecord>(b =>
        {
            b.CollectionName = TextTemplateManagementDbProperties.DbTablePrefix + "TextTemplateDefinitionContentRecords";
        });
    }
}
