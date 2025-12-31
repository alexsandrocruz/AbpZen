using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.Toolbar.LanguageSwitch;

public class LanguageSwitchViewComponent : LeptonXViewComponentBase
{
	public virtual async Task<IViewComponentResult> InvokeAsync(ThemeLanguageInfo model)
	{
		return View("~/Themes/LeptonX/Components/SideMenu/Toolbar/LanguageSwitch/Default.cshtml", model);
	}
}
