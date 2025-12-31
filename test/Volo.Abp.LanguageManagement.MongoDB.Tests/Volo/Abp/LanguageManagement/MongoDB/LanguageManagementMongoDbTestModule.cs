using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.LanguageManagement.MongoDB;

[DependsOn(
    typeof(LanguageManagementTestBaseModule),
    typeof(LanguageManagementMongoDbModule)
    )]
public class LanguageManagementMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
