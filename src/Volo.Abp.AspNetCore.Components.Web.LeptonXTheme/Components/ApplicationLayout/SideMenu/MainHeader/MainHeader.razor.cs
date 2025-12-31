using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.SideMenu.MainHeader
{
    public partial class MainHeader : IDisposable
    {
        [Inject]
        protected MainMenuProvider MainMenuProvider { get; set; }

        protected MenuViewModel Menu { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Menu = await MainMenuProvider.GetMenuAsync();
            Menu.StateChanged += RefreshMenu;
        }

        public void Dispose()
        {
            if (Menu != null)
            {
                Menu.StateChanged -= RefreshMenu;
            }
        }

        private void RefreshMenu(object sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
    }
}
