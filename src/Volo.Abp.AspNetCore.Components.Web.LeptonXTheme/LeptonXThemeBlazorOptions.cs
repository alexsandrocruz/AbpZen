using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme;
public class LeptonXThemeBlazorOptions
{
    /// <summary>
    /// Determines layout of appliction. Default value is <see cref="LeptonXBlazorLayouts.SideMenu"/>
    /// </summary>
    public Type Layout { get; set; } = LeptonXBlazorLayouts.SideMenu;

    public Func<IReadOnlyList<MenuItemViewModel>, IEnumerable<MenuItemViewModel>> MobileMenuSelector { get; set; } = (menuItems) => menuItems.Where(x => x.Items.IsNullOrEmpty());
}
