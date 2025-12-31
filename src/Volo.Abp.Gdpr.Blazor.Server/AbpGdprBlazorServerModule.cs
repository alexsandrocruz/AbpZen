using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.Gdpr.Blazor.Server;

[DependsOn(
    typeof(AbpGdprBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule)
    )]
public class AbpGdprBlazorServerModule : AbpModule
{
    
}