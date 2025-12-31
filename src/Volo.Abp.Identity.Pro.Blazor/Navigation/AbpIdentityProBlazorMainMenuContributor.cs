using System.Threading.Tasks;
using Volo.Abp.Identity.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Identity.Pro.Blazor.Navigation;

public class AbpIdentityProBlazorMainMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }


        var administrationMenu = context.Menu.GetAdministration();

        var l = context.GetLocalizer<IdentityResource>();

        var identityMenuItem = new ApplicationMenuItem(IdentityProMenus.GroupName, l["Menu:IdentityManagement"], icon: "fa fa-id-card");
        administrationMenu.AddItem(identityMenuItem);

        identityMenuItem.AddItem(new ApplicationMenuItem(IdentityProMenus.OrganizationUnits, l["OrganizationUnits"], url: "~/identity/organization-units").RequirePermissions(IdentityPermissions.OrganizationUnits.Default));
        identityMenuItem.AddItem(new ApplicationMenuItem(IdentityProMenus.Roles, l["Roles"], url: "~/identity/roles").RequirePermissions(IdentityPermissions.Roles.Default));
        identityMenuItem.AddItem(new ApplicationMenuItem(IdentityProMenus.Users, l["Users"], url: "~/identity/users").RequirePermissions(IdentityPermissions.Users.Default));
        identityMenuItem.AddItem(new ApplicationMenuItem(IdentityProMenus.ClaimTypes, l["ClaimTypes"], url: "~/identity/claim-types").RequirePermissions(IdentityPermissions.ClaimTypes.Default));
        identityMenuItem.AddItem(new ApplicationMenuItem(IdentityProMenus.SecurityLogs, l["SecurityLogs"], url: "~/identity/security-logs").RequirePermissions(IdentityPermissions.SecurityLogs.Default));

        return Task.CompletedTask;
    }
}
