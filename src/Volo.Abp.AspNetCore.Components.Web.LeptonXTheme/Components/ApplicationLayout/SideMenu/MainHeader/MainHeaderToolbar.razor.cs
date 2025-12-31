using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.LeptonX.Shared;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.SideMenu.MainHeader
{
    public partial class MainHeaderToolbar
    {
        protected static readonly string[] LeptonXPlainComponents = new string[] { "MainHeaderToolbarUserMenu", "GeneralSettings" };

        [Inject]
        private IToolbarManager ToolbarManager { get; set; }

        [Inject]
        private IAbpUtilsService AbpUtilsService { get; set; }

        private RenderFragment ToolbarRender { get; set; }

        private bool IsFullScreen { get; set; }
        
        private Toolbar Toolbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);

            var leptonxToolbar = await ToolbarManager.GetAsync(LeptonXToolbars.Main);

            Toolbar.Items.AddRange(leptonxToolbar.Items);
        }


        protected virtual bool IsPlainComponent(ToolbarItem item)
        {
            return LeptonXPlainComponents.Contains(item.ComponentType.Name);
        }

        private async Task ToogleFullScreen()
        {
            IsFullScreen = !IsFullScreen;
            if (IsFullScreen)
            {
                await AbpUtilsService.RequestFullscreenAsync();
            }
            else
            {
                await AbpUtilsService.ExitFullscreenAsync();
            }
        }
    }
}
