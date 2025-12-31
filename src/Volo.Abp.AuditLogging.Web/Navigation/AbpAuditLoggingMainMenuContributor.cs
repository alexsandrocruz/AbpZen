using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Features;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.AuditLogging.Web.Navigation;

public class AbpAuditLoggingMainMenuContributor : IMenuContributor
{
    public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return;
        }

        var l = context.GetLocalizer<AuditLoggingResource>();
        context.Menu
            .GetAdministration()
            .AddItem(
                new ApplicationMenuItem(
                    AbpAuditLoggingMainMenuNames.GroupName,
                    l["Menu:AuditLogging"],
                    "~/AuditLogs",
                    icon: "fa fa-file-text"
                )
                .RequireFeatures(AbpAuditLoggingFeatures.Enable)
                .RequirePermissions(AbpAuditLoggingPermissions.AuditLogs.Default));
    }
}
