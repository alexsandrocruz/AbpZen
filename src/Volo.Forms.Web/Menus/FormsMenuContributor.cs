using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Volo.Forms.Localization;
using Volo.Forms.Permissions;

namespace Volo.Forms.Web.Menus;

public class FormsMenuContributor : IMenuContributor
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
        if (await context.IsGrantedAsync(FormsPermissions.Forms.Default))
        {
            var l = context.GetLocalizer<FormsResource>();

            var formMenuItem = new ApplicationMenuItem(FormsMenus.GroupName, l["Menu:Forms"], icon: "fa fa-list", url: "/Forms");
            context.Menu.GetAdministration().AddItem(formMenuItem);
        }
    }
}
