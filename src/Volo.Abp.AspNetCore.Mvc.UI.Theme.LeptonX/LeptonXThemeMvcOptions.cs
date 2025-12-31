using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX;

public class LeptonXThemeMvcOptions
{
    /// <summary>
    /// Determines layout of application. Default value is <see cref="LeptonXMvcLayouts.SideMenu"/>
    /// </summary>
    public string ApplicationLayout { get; set; } = LeptonXMvcLayouts.SideMenu;

    /// <summary>
    /// A selector that defines which menu items will be displayed at mobile layout.
    /// </summary>
    public Func<IReadOnlyList<MenuItemViewModel>, IEnumerable<MenuItemViewModel>> MobileMenuSelector { get; set; } = (menuItems) => menuItems.Where(x => x.Items.IsNullOrEmpty());
}
