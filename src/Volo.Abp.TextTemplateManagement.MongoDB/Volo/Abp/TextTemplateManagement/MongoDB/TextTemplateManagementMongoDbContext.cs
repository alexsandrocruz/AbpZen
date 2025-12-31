using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.TextTemplateManagement.TextTemplates;

namespace Volo.Abp.TextTemplateManagement.MongoDB;

[ConnectionStringName(TextTemplateManagementDbProperties.ConnectionStringName)]
public class TextTemplateManagementMongoDbContext : AbpMongoDbContext, ITextTemplateManagementMongoDbContext
{
    public IMongoCollection<TextTemplateContent> TextTemplates => Collection<TextTemplateContent>();

    public IMongoCollection<TextTemplateDefinitionRecord> TextTemplateDefinitionRecords => Collection<TextTemplateDefinitionRecord>();

    public IMongoCollection<TextTemplateDefinitionContentRecord> TextTemplateDefinitionContentRecords => Collection<TextTemplateDefinitionContentRecord>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureTextTemplateManagement();
    }

}
