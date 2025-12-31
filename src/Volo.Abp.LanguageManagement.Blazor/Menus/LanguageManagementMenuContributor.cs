using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.LanguageManagement.Blazor.Menus;

public class LanguageManagementMenuContributor : IMenuContributor
{
    public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    protected virtual async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<LanguageManagementResource>();

        var languagesMenu = new ApplicationMenuItem(
            LanguageManagementMenus.GroupName,
            l["Menu:Languages"],
            icon: "fa fa-globe"
        );

        context.Menu.GetAdministration().AddItem(languagesMenu);

        languagesMenu.AddItem(new ApplicationMenuItem(LanguageManagementMenus.Languages, l["Languages"], "~/language-management/languages")
            .RequireFeatures(LanguageManagementFeatures.Enable)
            .RequirePermissions(LanguageManagementPermissions.Languages.Default));

        languagesMenu.AddItem(new ApplicationMenuItem(LanguageManagementMenus.Texts, l["LanguageTexts"], "~/language-management/texts")
            .RequireFeatures(LanguageManagementFeatures.Enable)
            .RequirePermissions(LanguageManagementPermissions.LanguageTexts.Default));
    }
}
