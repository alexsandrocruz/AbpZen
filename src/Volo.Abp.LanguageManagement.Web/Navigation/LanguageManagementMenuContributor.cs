using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Features;
using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.LanguageManagement.Navigation;

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
            LanguageManagementMenuNames.GroupName,
            l["Menu:Languages"],
            icon: "fa fa-globe"
        );

        context.Menu.GetAdministration().AddItem(languagesMenu);

        languagesMenu.AddItem(new ApplicationMenuItem(LanguageManagementMenuNames.Languages, l["Languages"], "~/LanguageManagement")
            .RequireFeatures(LanguageManagementFeatures.Enable)
            .RequirePermissions(LanguageManagementPermissions.Languages.Default));

        languagesMenu.AddItem(new ApplicationMenuItem(LanguageManagementMenuNames.Texts, l["LanguageTexts"], "~/LanguageManagement/Texts")
            .RequireFeatures(LanguageManagementFeatures.Enable)
            .RequirePermissions(LanguageManagementPermissions.LanguageTexts.Default));
    }
}
