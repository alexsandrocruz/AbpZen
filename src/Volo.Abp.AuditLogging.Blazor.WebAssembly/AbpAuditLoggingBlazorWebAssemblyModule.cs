using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging.Blazor.WebAssembly;

[DependsOn(typeof(AbpAuditLoggingBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(AbpAuditLoggingHttpApiClientModule))]
public class AbpAuditLoggingBlazorWebAssemblyModule : AbpModule
{
}
