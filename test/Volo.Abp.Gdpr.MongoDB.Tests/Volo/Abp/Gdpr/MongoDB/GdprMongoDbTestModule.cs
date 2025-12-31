using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr.MongoDB;

[DependsOn(
    typeof(GdprTestBaseModule),
    typeof(AbpGdprMongoDbModule)
)]
public class GdprMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}