using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using SideMenuUserMenu = Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Components.ApplicationLayout.SideMenu.MainHeader.MainHeaderToolbarUserMenu;
using ToolbarItem = Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars.ToolbarItem;
using TopMenuUserMenu = Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Components.ApplicationLayout.TopMenu.MainHeader.MainHeaderToolbarUserMenu;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme;

public class LeptonXThemeToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            // TODO: Find better way to use different components for each layout.
            var options = context.ServiceProvider.GetRequiredService<IOptions<LeptonXThemeBlazorOptions>>().Value;

            if (options.Layout == LeptonXBlazorLayouts.SideMenu)
            {
                context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(SideMenuUserMenu)));
            }

            if (options.Layout == LeptonXBlazorLayouts.TopMenu)
            {
                context.Toolbar.Items.Insert(0, new ToolbarItem(typeof(TopMenuUserMenu)));
            }
        }
        return Task.CompletedTask;
    }
}
