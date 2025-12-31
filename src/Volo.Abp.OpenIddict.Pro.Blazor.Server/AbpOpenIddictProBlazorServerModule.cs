using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.AuditLogging.Blazor.Server;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.Server;

namespace Volo.Abp.OpenIddict.Pro.Blazor.Server;

[DependsOn(
    typeof(AbpOpenIddictProBlazorModule),
    typeof(AbpPermissionManagementBlazorServerModule),
    typeof(AbpAuditLoggingBlazorServerModule))]
public class AbpOpenIddictProBlazorServerModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
    }
}