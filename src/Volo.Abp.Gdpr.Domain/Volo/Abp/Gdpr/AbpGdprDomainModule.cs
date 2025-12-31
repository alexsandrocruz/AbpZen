using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpGdprAbstractionsModule),
    typeof(AbpGdprDomainSharedModule),
    typeof(AbpDddDomainModule)
)]
public class AbpGdprDomainModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}