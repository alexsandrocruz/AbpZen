using Volo.Abp;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.Saas.Tenant;

[DependsOn(
    typeof(SaasDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
)]
public class SaasTenantApplicationContractsModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
