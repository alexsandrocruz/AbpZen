using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor.WebAssembly;

namespace Volo.CmsKit.Pro.Admin.Blazor.WebAssembly;

[DependsOn(
    typeof(AbpSettingManagementBlazorWebAssemblyModule),
    typeof(CmsKitProAdminBlazorModule),
    typeof(CmsKitProAdminHttpApiClientModule)
)]
public class CmsKitProAdminBlazorWebAssemblyModule : AbpModule
{
}