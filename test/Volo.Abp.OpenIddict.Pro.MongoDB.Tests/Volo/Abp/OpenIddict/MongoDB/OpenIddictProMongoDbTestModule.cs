using System;
using Volo.Abp.Data;
using Volo.Abp.Identity.MongoDB;
using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict.MongoDB;

[DependsOn(
    typeof(OpenIddictProApplicationTestModule),
    typeof(AbpIdentityMongoDbModule),
    typeof(AbpOpenIddictProMongoDbModule)
)]
public class OpenIddictProMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
