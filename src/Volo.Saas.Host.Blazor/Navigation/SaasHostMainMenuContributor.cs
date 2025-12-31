using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Volo.Saas.Localization;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Saas.Host.Blazor.Navigation;

public class SaasHostMainMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var l = context.GetLocalizer<SaasResource>();

        var saasMenu = new ApplicationMenuItem(SaasHostMenus.GroupName, l["Menu:Saas"], icon: "fa fa-globe");
        context.Menu.AddItem(saasMenu);

        saasMenu.AddItem(new ApplicationMenuItem(
            SaasHostMenus.Tenants, 
            l["Tenants"], 
            url: "~/saas/host/tenants",
            icon: "bi bi-people-fill"
            ).RequirePermissions(SaasHostPermissions.Tenants.Default));
        saasMenu.AddItem(new ApplicationMenuItem(
            SaasHostMenus.Editions, 
            l["Editions"], 
            url: "~/saas/host/editions",
            icon: "bi bi-ui-checks-grid"
            ).RequirePermissions(SaasHostPermissions.Editions.Default));

        return Task.CompletedTask;
    }
}
