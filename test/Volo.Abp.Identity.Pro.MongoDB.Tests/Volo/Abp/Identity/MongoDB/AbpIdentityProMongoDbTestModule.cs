using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Volo.Abp.PermissionManagement.MongoDB;

namespace Volo.Abp.Identity.MongoDB;

[DependsOn(
    typeof(AbpIdentityTestBaseModule),
    typeof(AbpPermissionManagementMongoDbModule),
    typeof(AbpIdentityProMongoDbModule)
)]
public class AbpIdentityMongoDbTestModule : AbpModule
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
