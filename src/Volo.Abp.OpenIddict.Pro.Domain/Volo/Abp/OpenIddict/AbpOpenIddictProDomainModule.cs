using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpOpenIddictProDomainSharedModule),
    typeof(AbpOpenIddictDomainModule)
)]
public class AbpOpenIddictProDomainModule : AbpModule
{

}
