using System.Threading.Tasks;
using LeptonXDemoApp.Localization;
using LeptonXDemoApp.Permissions;
using Volo.Abp.UI.Navigation;

namespace LeptonXDemoApp.Web.Menus;

public class ProductMenuContributor : IMenuContributor
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
        var l = context.GetLocalizer<LeptonXDemoAppResource>();

        context.Menu.AddItem(
            new ApplicationMenuItem(
                LeptonXDemoAppMenus.Product,
                l["Menu:Products"],
                url: "/Products",
                icon: "fas fa-list",
                requiredPermissionName: LeptonXDemoAppPermissions.Product.Default
            )
        );
    }
}
