using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplateManagement.Blazor.WebAssembly;

[DependsOn(
    typeof(TextTemplateManagementBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(TextTemplateManagementHttpApiClientModule))]
public class TextTemplateManagementBlazorWebAssemblyModule : AbpModule
{
}
