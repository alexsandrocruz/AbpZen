using System.Collections.Generic;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Navigation;

public class MenuViewModel
{
    public ApplicationMenu Menu { get; set; }

    public List<MenuItemViewModel> Items { get; set; }
}
