using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.Abp.LanguageManagement;

[DependsOn(
    typeof(LanguageManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
    )]
public class LanguageManagementApplicationContractsModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}
