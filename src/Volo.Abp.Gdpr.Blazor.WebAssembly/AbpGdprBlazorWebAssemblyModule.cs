using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr.Blazor.WebAssembly;

[DependsOn(
    typeof(AbpGdprBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(AbpGdprHttpApiClientModule)
    )]
public class AbpGdprBlazorWebAssemblyModule : AbpModule
{
    
}