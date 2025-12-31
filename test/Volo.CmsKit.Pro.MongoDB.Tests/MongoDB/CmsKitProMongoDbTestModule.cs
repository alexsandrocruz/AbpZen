using System;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.CmsKit.MongoDB;
using Volo.Abp.Uow;

namespace Volo.CmsKit.Pro.MongoDB;

[DependsOn(
    typeof(CmsKitProTestBaseModule),
    typeof(CmsKitProMongoDbModule)
    )]
public class CmsKitProMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });

        Configure<AbpUnitOfWorkDefaultOptions>(options =>
        {
            options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
        });
    }
}
