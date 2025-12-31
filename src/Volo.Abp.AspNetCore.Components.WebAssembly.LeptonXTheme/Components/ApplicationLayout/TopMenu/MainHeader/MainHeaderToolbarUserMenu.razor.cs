using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Security;
using Volo.Abp.Http.Client;
using Volo.Abp.LeptonX.Shared.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme.Components.ApplicationLayout.TopMenu.MainHeader;

public partial class MainHeaderToolbarUserMenu : IDisposable
{
    [Inject]
    protected IMenuManager MenuManager { get; set; }

    [Inject]
    protected ICurrentUser CurrentUser { get; set; }

    [Inject]
    protected ICurrentTenant CurrentTenant { get; set; }

    [Inject]
    protected NavigationManager Navigation { get; set; }

    [Inject]
    protected ApplicationConfigurationChangedService ApplicationConfigurationChangedService { get; set; }

    [Inject]
    protected IStringLocalizer<LeptonXResource> L { get; set; }

    [Inject]
    protected IRemoteServiceConfigurationProvider RemoteServiceConfigurationProvider { get; set; }

    [Inject]
    protected IJSRuntime JsRuntime { get; set; }

    protected ApplicationMenu UserMenu { get; set; }

    protected Guid? UserId { get; set; }

    protected string UserName { get; set; }

    protected string TenantName { get; set; }

    protected string ProfileImageUrl { get; set; }

    protected string UserFullName { get; set; }

    protected string UserEmail { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await SetUserMenuAndProfileAsync();

        Navigation.LocationChanged += OnLocationChanged;
        ApplicationConfigurationChangedService.Changed += ApplicationConfigurationChanged;
    }

    private async void ApplicationConfigurationChanged()
    {
        await SetUserMenuAndProfileAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task SetUserMenuAndProfileAsync()
    {
        UserMenu = await MenuManager.GetAsync(StandardMenus.User);

        UserId = CurrentUser.Id;
        UserName = CurrentUser.UserName;
        UserFullName = CalculateUserFullName();
        UserEmail = CurrentUser.Email;
        TenantName = CurrentTenant.Name;

        if (UserId != null)
        {
            ProfileImageUrl = (await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("AbpAccountPublic"))?.BaseUrl.EnsureEndsWith('/') +
                             $"api/account/profile-picture-file/{UserId}";
        }
    }

    protected virtual void OnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    protected virtual void BeginSignOut()
    {
        if (AbpAspNetCoreComponentsWebOptions.Value.IsBlazorWebApp)
        {
            Navigation.NavigateTo(AuthenticationOptions.Value.LogoutUrl, forceLoad: true);
        }
        else
        {
            Navigation.NavigateToLogout(AuthenticationOptions.Value.LogoutUrl);
        }
    }

    protected virtual async Task NavigateToAsync(string uri, string target = null)
    {
        if (target == "_blank")
        {
            await JsRuntime.InvokeVoidAsync("open", uri, target);
        }
        else
        {
            Navigation.NavigateTo(uri);
        }
    }

    protected virtual string CalculateUserFullName()
    {
        //TODO: Should we move this logic to some extension method for the ICurrentUser?

        var fullName = new StringBuilder();

        if (!CurrentUser.Name.IsNullOrEmpty())
        {
            fullName.Append(CurrentUser.Name);
        }

        if (!CurrentUser.SurName.IsNullOrEmpty())
        {
            if (fullName.Length > 0)
            {
                fullName.Append(" ");
            }

            fullName.Append(CurrentUser.SurName);
        }

        if (fullName.Length == 0)
        {
            fullName.Append(CurrentUser.UserName);
        }

        return fullName.ToString();
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        ApplicationConfigurationChangedService.Changed -= ApplicationConfigurationChanged;
    }
}
