using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.WebAssembly;
using Volo.Abp.SettingManagement.Blazor.WebAssembly;

namespace Volo.Abp.Identity.Pro.Blazor.Server.WebAssembly;

[DependsOn(
    typeof(AbpIdentityProBlazorModule),
    typeof(AbpPermissionManagementBlazorWebAssemblyModule),
    typeof(AbpSettingManagementBlazorWebAssemblyModule),
    typeof(AbpIdentityHttpApiClientModule))]
public class AbpIdentityProBlazorWebAssemblyModule : AbpModule
{
}
