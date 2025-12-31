using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Navigation;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.MobileNavbar;

public class MobileNavbarViewModel
{
	public ApplicationMenu UserMenu { get; set; }

	public string ProfileImageUrl { get; set; }

    public List<MenuItemViewModel> SelectedMenuItems { get; set; } = new();

    public Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars.Toolbar Toolbar { get; set; }
}
