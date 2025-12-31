using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.FileManagement.Blazor.WebAssembly;

[DependsOn(
    typeof(FileManagementBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(FileManagementHttpApiClientModule)
)]
public class FileManagementBlazorWebAssemblyModule : AbpModule
{

}
