using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.Toolbar.LanguageSwitch;

public class ThemeLanguageInfo
{
	public LanguageInfo CurrentLanguage { get; set; }

	public IReadOnlyList<LanguageInfo> Languages { get; set; }
}
