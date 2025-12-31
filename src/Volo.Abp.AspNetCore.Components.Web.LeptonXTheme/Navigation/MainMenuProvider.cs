using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;

public class MainMenuProvider : IScopedDependency
{
    private readonly IMenuManager _menuManager;
    private readonly IObjectMapper<AbpAspNetCoreComponentsWebLeptonXThemeModule> _objectMapper;

    public MainMenuProvider(
        IMenuManager menuManager,
        IObjectMapper<AbpAspNetCoreComponentsWebLeptonXThemeModule> objectMapper)
    {
        _menuManager = menuManager;
        _objectMapper = objectMapper;
    }

    public virtual async Task<MenuViewModel> GetMenuAsync()
    {
        var menu = await _menuManager.GetMainMenuAsync();

            //Menu = _objectMapper.Map<ApplicationMenu, MenuViewModel>(menu);
        var result = new MenuViewModel
        {
            Menu = menu,
            Items = menu.Items.Select(CreateMenuItemViewModel).ToList()
        };
        result.SetParents();
            // TODO: LeptonX - Menu Placement
            //Menu.Placement = await _leptonSettings.GetMenuPlacementAsync();
            //Menu.NavBarStatus = await _leptonSettings.GetMenuStatusAsync();

        return result;
    }

    private MenuItemViewModel CreateMenuItemViewModel(ApplicationMenuItem applicationMenuItem)
    {
        var viewModel = new MenuItemViewModel
        {
            MenuItem = applicationMenuItem,
        };

        viewModel.Items = new List<MenuItemViewModel>();

        foreach (var item in applicationMenuItem.Items)
        {
            viewModel.Items.Add(CreateMenuItemViewModel(item));
        }

        return viewModel;
    }
}
