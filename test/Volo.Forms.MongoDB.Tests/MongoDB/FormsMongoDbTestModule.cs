using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Forms.MongoDB;

[DependsOn(
    typeof(FormsTestBaseModule),
    typeof(FormsMongoDbModule)
    )]
public class FormsMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
