using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.LanguageManagement.Blazor.WebAssembly;

[DependsOn(
    typeof(LanguageManagementBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(LanguageManagementHttpApiClientModule))]
public class LanguageManagementBlazorWebAssemblyModule : AbpModule
{
}
