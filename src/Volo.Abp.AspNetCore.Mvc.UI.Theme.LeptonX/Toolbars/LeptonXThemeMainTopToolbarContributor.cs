using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.GeneralSettings;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.Users;
using SideMenuUserMenu = Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.Toolbar.UserMenu.UserMenuViewComponent;
using TopMenuUserMenu = Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.TopMenu.UserMenu.UserMenuViewComponent;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Toolbars;

public class LeptonXThemeMainTopToolbarContributor : IToolbarContributor
{
	public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
	{
		if (!(context.Theme is LeptonXTheme))
		{
			return;
		}

		var options = context.ServiceProvider.GetRequiredService<IOptions<LeptonXThemeMvcOptions>>();
		var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();

		if (context.Toolbar.Name == LeptonXToolbars.Main)
		{
			context.Toolbar.Items.Add(new ToolbarItem(typeof(GeneralSettingsViewComponent)));

			if (currentUser.IsAuthenticated)
			{
				if (options.Value.ApplicationLayout == LeptonXMvcLayouts.TopMenu)
				{
					context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(TopMenuUserMenu), order: -1));
				}
				else
				{
					context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(SideMenuUserMenu), order: -1));
				}
			}
		}
	}
}
