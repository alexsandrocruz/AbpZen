using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Volo.Abp.LeptonX.Shared.Localization;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme.Pages
{
    public partial class Authentication
    {
        protected WebAssemblyCachedApplicationConfigurationClient WebAssemblyCachedApplicationConfigurationClient { get; }

        public Authentication(WebAssemblyCachedApplicationConfigurationClient webAssemblyCachedApplicationConfigurationClient)
        {
            WebAssemblyCachedApplicationConfigurationClient = webAssemblyCachedApplicationConfigurationClient;
            LocalizationResource = typeof(LeptonXResource);
        }

        [Parameter]
        public string Action { get; set; }
        private async Task OnLogInSucceeded(RemoteAuthenticationState obj)
        {
            await WebAssemblyCachedApplicationConfigurationClient.InitializeAsync();
        }

        private async Task OnLogOutSucceeded(RemoteAuthenticationState obj)
        {
            await WebAssemblyCachedApplicationConfigurationClient.InitializeAsync();
        }
    }
}
