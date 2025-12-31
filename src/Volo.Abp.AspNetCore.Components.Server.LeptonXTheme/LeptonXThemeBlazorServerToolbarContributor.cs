using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using SideMenuUserMenu = Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Components.ApplicationLayout.SideMenu.MainHeader.MainHeaderToolbarUserMenu;
using TopMenuUserMenu = Volo.Abp.AspNetCore.Components.Server.LeptonXTheme.Components.ApplicationLayout.TopMenu.MainHeader.MainHeaderToolbarUserMenu;

namespace Volo.Abp.AspNetCore.Components.Server.LeptonXTheme;

public class LeptonXThemeBlazorServerToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            // TODO: Find better way to use different components for each layout.
            var options = context.ServiceProvider.GetRequiredService<IOptions<LeptonXThemeBlazorOptions>>().Value;

            if (options.Layout == LeptonXBlazorLayouts.SideMenu)
            {
                context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(SideMenuUserMenu), order: -1));
            }

            if (options.Layout == LeptonXBlazorLayouts.TopMenu)
            {
                context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(TopMenuUserMenu), order: -1));
            }
        }

        return Task.CompletedTask;
    }
}
