using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.FeatureManagement.Blazor.WebAssembly;
using Volo.Abp.Modularity;

namespace Volo.Saas.Host.Blazor.WebAssembly;

[DependsOn(
    typeof(SaasHostBlazorModule),
    typeof(AbpFeatureManagementBlazorWebAssemblyModule),
    typeof(SaasHostHttpApiClientModule)
)]
public class SaasHostBlazorWebAssemblyModule : AbpModule
{
}
