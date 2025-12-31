using Volo.Abp.FeatureManagement.Blazor.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Saas.Host.Blazor;

namespace Volo.Saas.Tenant.Blazor.WebAssembly;

[DependsOn(
    typeof(SaasHostBlazorModule),
    typeof(AbpFeatureManagementBlazorWebAssemblyModule),
    typeof(SaasTenantHttpApiClientModule)
)]
public class SaasTenantBlazorWebAssemblyModule : AbpModule
{
}
