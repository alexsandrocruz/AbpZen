using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.TextTemplateManagement.MongoDB;

[DependsOn(
    typeof(TextTemplateManagementTestBaseModule),
    typeof(TextTemplateManagementMongoDbModule)
    )]
public class TextTemplateManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
