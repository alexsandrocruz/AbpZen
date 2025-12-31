using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.LanguageManagement.Blazor.Server;

[DependsOn(
    typeof(LanguageManagementBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule))]
public class LanguageManagementBlazorServerModule : AbpModule
{
}
