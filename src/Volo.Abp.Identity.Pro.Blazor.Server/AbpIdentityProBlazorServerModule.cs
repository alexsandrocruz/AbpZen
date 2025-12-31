using Microsoft.AspNetCore.SignalR;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Blazor.Server;
using Volo.Abp.SettingManagement.Blazor.Server;

namespace Volo.Abp.Identity.Pro.Blazor.Server;

[DependsOn(
    typeof(AbpIdentityProBlazorModule),
    typeof(AbpPermissionManagementBlazorServerModule),
    typeof(AbpSettingManagementBlazorServerModule))]
public class AbpIdentityProBlazorServerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // TODO Remove this when https://github.com/dotnet/aspnetcore/issues/38842 fixed
        Configure<HubOptions>(options =>
        {
            options.DisableImplicitFromServicesParameters = true;
        });
    }
}
