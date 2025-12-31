using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Navigation;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.LeptonX.Shared;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.SideMenu.MobileNavbar;

public class MobileNavbarViewComponent : LeptonXViewComponentBase
{
    protected IMenuManager MenuManager { get; }

    protected ICurrentUser CurrentUser { get; }

    protected IToolbarManager ToolbarManager { get; }

	protected MenuViewModelProvider MenuViewModelProvider { get; }
    public LeptonXThemeMvcOptions Options { get; }

    public MobileNavbarViewComponent(
        IMenuManager menuManager,
        ICurrentUser currentUser,
        IToolbarManager toolbarManager,
        MenuViewModelProvider menuViewModelProvider,
        IOptions<LeptonXThemeMvcOptions> options)
    {
        MenuManager = menuManager;
        CurrentUser = currentUser;
        ToolbarManager = toolbarManager;
        MenuViewModelProvider = menuViewModelProvider;
        Options = options.Value;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var mainMenu = await MenuViewModelProvider.GetMenuViewModelAsync();

        var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);
        var leptonxToolbar = await ToolbarManager.GetAsync(LeptonXToolbars.MainMobile);

        toolbar.Items.AddRange(leptonxToolbar.Items);

        return View("~/Themes/LeptonX/Components/SideMenu/MobileNavbar/Default.cshtml", new MobileNavbarViewModel
        {
            UserMenu = await MenuManager.GetAsync(StandardMenus.User),
            ProfileImageUrl = $"~/api/account/profile-picture-file/{CurrentUser.Id}",
            SelectedMenuItems = Options.MobileMenuSelector(mainMenu.Items.AsReadOnly()).Take(2).ToList(),
            Toolbar = toolbar
        });
    }
}
