using System.Threading.Tasks;
using OpenIddict.Abstractions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.OpenIddict.Localization;
using Volo.Abp.OpenIddict.Permissions;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.OpenIddict.Pro.Web.Menus;

public class OpenIddictProMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<AbpOpenIddictResource>();

        var openIddictMenuItem = new ApplicationMenuItem(OpenIddictProMenus.GroupName, l["Menu:OpenIddict"], icon: "fa fa-id-badge");

        openIddictMenuItem.AddItem(new ApplicationMenuItem(OpenIddictProMenus.Applications, l["Applications"], url: "~/openIddict/Applications").RequirePermissions(AbpOpenIddictProPermissions.Application.Default));
        openIddictMenuItem.AddItem(new ApplicationMenuItem(OpenIddictProMenus.Scopes, l["Scopes"], url: "~/openIddict/Scopes").RequirePermissions(AbpOpenIddictProPermissions.Scope.Default));
       
        context.Menu.GetAdministration().AddItem(openIddictMenuItem);
    }
}
