using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Saas.Localization;

namespace Volo.Saas.Host.Navigation;

public class SaasHostMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<SaasResource>();

        var saasMenu = new ApplicationMenuItem(SaasHostMenuNames.GroupName, l["Menu:Saas"], icon: "fa fa-globe");
        context.Menu.AddItem(saasMenu);

        saasMenu.AddItem(new ApplicationMenuItem(
            SaasHostMenuNames.Tenants, 
            l["Tenants"], 
            url: "~/Saas/Host/Tenants",
            icon: "bi bi-people-fill"
            ).RequirePermissions(SaasHostPermissions.Tenants.Default));
        saasMenu.AddItem(new ApplicationMenuItem(
            SaasHostMenuNames.Editions, 
            l["Editions"], 
            url: "~/Saas/Host/Editions",
            icon: "bi bi-ui-checks-grid"
            ).RequirePermissions(SaasHostPermissions.Editions.Default));

        return Task.CompletedTask;
    }
}
