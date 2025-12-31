using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Volo.Payment.Admin.Blazor.Server;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerThemingModule),
    typeof(AbpPaymentAdminBlazorModule)
)]
public class AbpPaymentAdminBlazorServerModule : AbpModule
{

}
