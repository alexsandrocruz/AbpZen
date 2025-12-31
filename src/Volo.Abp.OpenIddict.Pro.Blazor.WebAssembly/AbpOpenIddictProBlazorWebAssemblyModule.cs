using Volo.Abp.AuditLogging.Blazor.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.WebAssembly;

namespace Volo.Abp.OpenIddict.Pro.Blazor.WebAssembly;

[DependsOn(
    typeof(AbpOpenIddictProBlazorModule),
    typeof(AbpOpenIddictProHttpApiClientModule),
    typeof(AbpPermissionManagementBlazorWebAssemblyModule),
    typeof(AbpAuditLoggingBlazorWebAssemblyModule)
    )]
public class AbpOpenIddictProBlazorWebAssemblyModule : AbpModule
{

}
