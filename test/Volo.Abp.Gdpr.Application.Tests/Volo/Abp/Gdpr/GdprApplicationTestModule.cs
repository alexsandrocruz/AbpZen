using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr;

[DependsOn(
    typeof(AbpGdprApplicationModule),
    typeof(GdprDomainTestModule)
)]
public class GdprApplicationTestModule : AbpModule
{
    
}