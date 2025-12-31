using Microsoft.AspNetCore.Components;
using Volo.Abp.Account.Pro.Public.MauiBlazor.OAuth;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor.Pages.Account;

public partial class RedirectToLogout
{
    [Inject]
    public IExternalAuthService ExternalAuthService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected async override Task OnInitializedAsync()
    {
        if (CurrentUser.IsAuthenticated)
        {
            await ExternalAuthService.SignOutAsync();
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
}
