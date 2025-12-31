using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.TextTemplateManagement.Authorization;
using Volo.Abp.TextTemplateManagement.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.TextTemplateManagement.Web.Navigation;

public class TextTemplateManagementMenuContributor : IMenuContributor
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
        var l = context.GetLocalizer<TextTemplateManagementResource>();

        var textTemplateManagementMenu =
            new ApplicationMenuItem(
                TextTemplateManagementMainMenuNames.GroupName,
                l["Menu:TextTemplates"],
                "~/TextTemplates",
                icon: "fa fa-text-width"
            )
            .RequireFeatures(TextManagementFeatures.Enable)
            .RequirePermissions(TextTemplateManagementPermissions.TextTemplates.Default);

        context.Menu.GetAdministration().AddItem(textTemplateManagementMenu);
    }
}
