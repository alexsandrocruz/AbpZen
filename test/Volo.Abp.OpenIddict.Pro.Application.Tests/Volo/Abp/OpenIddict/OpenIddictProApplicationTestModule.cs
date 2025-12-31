using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpOpenIddictProApplicationModule),
    typeof(OpenIddictProTestBaseModule)
    )]
public class OpenIddictProApplicationTestModule : AbpModule
{

}
