using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.Toolbar.LanguageSwitch;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.Common.MobileGeneralSettings;

public class MobileGeneralSettingsViewComponent : LeptonXViewComponentBase
{
    protected ThemeLanguageInfoProvider ThemeLanguageInfoProvider { get; }

    public MobileGeneralSettingsViewComponent(ThemeLanguageInfoProvider themeLanguageInfoProvider)
    {
        ThemeLanguageInfoProvider = themeLanguageInfoProvider;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        return View(
            "~/Themes/LeptonX/Components/Common/MobileGeneralSettings/Default.cshtml",
            await ThemeLanguageInfoProvider.GetLanguageSwitchViewComponentModel());
    }
}
