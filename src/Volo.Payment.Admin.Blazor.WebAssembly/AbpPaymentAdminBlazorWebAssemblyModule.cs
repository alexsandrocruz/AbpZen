using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Payment.Admin.Blazor.WebAssembly;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule),
    typeof(AbpPaymentAdminBlazorModule),
    typeof(AbpPaymentHttpApiClientModule)
    )]
public class AbpPaymentAdminBlazorWebAssemblyModule : AbpModule
{
}
