using System.Threading.Tasks;
using LeptonXDemoApp.Localization;
using LeptonXDemoApp.Permissions;
using Volo.Abp.UI.Navigation;

namespace LeptonXDemoApp.Web.Menus;

public class CategoryMenuContributor : IMenuContributor
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
                LeptonXDemoAppMenus.Category,
                l["Menu:Categories"],
                url: "/Categories",
                icon: "fas fa-list",
                requiredPermissionName: LeptonXDemoAppPermissions.Category.Default
            )
        );
    }
}
