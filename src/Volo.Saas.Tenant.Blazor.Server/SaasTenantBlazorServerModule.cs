using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.FeatureManagement.Blazor.Server;
using Volo.Abp.Modularity;

namespace Volo.Saas.Tenant.Blazor.Server;

[DependsOn(
    typeof(SaasTenantBlazorModule),
    typeof(AbpFeatureManagementBlazorServerModule)
)]
public class SaasTenantBlazorServerModule : AbpModule
{
}
