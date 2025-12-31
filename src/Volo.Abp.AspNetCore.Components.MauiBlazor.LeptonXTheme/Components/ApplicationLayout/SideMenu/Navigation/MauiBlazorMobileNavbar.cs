using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout.SideMenu.Navigation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.LeptonXTheme.Components.ApplicationLayout.SideMenu.Navigation;

[ExposeServices(typeof(MobileNavbar))]
public class MauiBlazorMobileNavbar : MobileNavbar
{
    public const string LogoutMenuName = "UserMenu.SideMenu.Logout";
    protected override string LoginLink => "/account/login";

    [Inject]
    protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (CurrentUser.IsAuthenticated)
        {
            UserMenu.AddItem(new ApplicationMenuItem(
                LogoutMenuName,
                L["Logout"],
                "/account/logout",
                icon: "bi bi-power"));
        }
    }

    protected override async Task<string> GetProfilePictureURLAsync(Guid userId)
    {
        return (await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AbpAccountPublic"))?.BaseUrl.EnsureEndsWith('/')
               + $"api/account/profile-picture-file/{userId}";
    }
}
