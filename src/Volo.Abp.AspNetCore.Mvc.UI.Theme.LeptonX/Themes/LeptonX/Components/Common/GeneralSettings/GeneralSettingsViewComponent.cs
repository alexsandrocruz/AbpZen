using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.Toolbar.LanguageSwitch;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.GeneralSettings;

public class GeneralSettingsViewComponent : AbpViewComponent
{
    protected ThemeLanguageInfoProvider ThemeLanguageInfoProvider { get; }

    public GeneralSettingsViewComponent(ThemeLanguageInfoProvider themeLanguageInfoProvider)
    {
        ThemeLanguageInfoProvider = themeLanguageInfoProvider;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View(
            "~/Themes/LeptonX/Components/Common/GeneralSettings/Default.cshtml",
            await ThemeLanguageInfoProvider.GetLanguageSwitchViewComponentModel());
    }
}
