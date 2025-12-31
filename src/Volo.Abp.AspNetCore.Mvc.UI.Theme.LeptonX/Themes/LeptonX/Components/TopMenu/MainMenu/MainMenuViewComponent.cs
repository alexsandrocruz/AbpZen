using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.TopMenu.MainMenu;
public class MainMenuViewComponent : LeptonXViewComponentBase
{
    protected MenuViewModelProvider MenuViewModelProvider { get; }

    public MainMenuViewComponent(MenuViewModelProvider menuViewModelProvider)
    {
        MenuViewModelProvider = menuViewModelProvider;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = await MenuViewModelProvider.GetMenuViewModelAsync();

        return View(
            "~/Themes/LeptonX/Components/TopMenu/MainMenu/Default.cshtml",
            viewModel
        );
    }
}
