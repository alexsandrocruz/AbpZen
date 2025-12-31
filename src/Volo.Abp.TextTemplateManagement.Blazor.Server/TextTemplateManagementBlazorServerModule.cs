using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.TextTemplateManagement.Blazor.Server;

[DependsOn(
    typeof(TextTemplateManagementBlazorModule),
    typeof(AbpAspNetCoreComponentsServerThemingModule))]
public class TextTemplateManagementBlazorServerModule : AbpModule
{
}
