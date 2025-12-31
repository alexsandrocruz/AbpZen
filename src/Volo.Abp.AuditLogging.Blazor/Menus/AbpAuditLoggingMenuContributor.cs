using System.Threading.Tasks;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Features;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.AuditLogging.Blazor.Menus;

public class AbpAuditLoggingMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    protected virtual Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<AuditLoggingResource>();
        context.Menu
            .GetAdministration()
            .AddItem(
                new ApplicationMenuItem(
                    AbpAuditLoggingMenus.GroupName,
                    l["Menu:AuditLogging"],
                    "~/audit-logs",
                    icon: "fa fa-file-alt"
                )
                .RequireFeatures(AbpAuditLoggingFeatures.Enable)
                .RequirePermissions(AbpAuditLoggingPermissions.AuditLogs.Default)
            );

        return Task.CompletedTask;
    }
}
