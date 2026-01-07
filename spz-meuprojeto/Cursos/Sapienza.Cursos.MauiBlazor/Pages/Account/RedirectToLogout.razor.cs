using Microsoft.AspNetCore.Components;
using Sapienza.Cursos.MauiBlazor.OAuth;

namespace Sapienza.Cursos.MauiBlazor.Pages.Account;

public partial class RedirectToLogout
{
    [Inject]
    public IExternalAuthService CodeFlowExternalAuthService { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected async override Task OnInitializedAsync()
    {
        if (CurrentUser.IsAuthenticated)
        {
            await CodeFlowExternalAuthService.SignOutAsync();
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
}
