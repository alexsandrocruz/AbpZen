using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.Toolbar.UserMenu;

public class UserMenuViewComponent : LeptonXViewComponentBase
{
	protected IMenuManager MenuManager;

	public UserMenuViewComponent(IMenuManager menuManager)
	{
		MenuManager = menuManager;
	}

	public virtual async Task<IViewComponentResult> InvokeAsync()
	{
		var menu = await MenuManager.GetAsync(StandardMenus.User);
		return View("~/Themes/LeptonX/Components/SideMenu/Toolbar/UserMenu/Default.cshtml", menu);
	}
}
