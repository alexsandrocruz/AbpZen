using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Volo.Abp.Account.LinkUsers;
using Volo.Abp.Account.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.Account.Pro.Public.Blazor.Shared.Pages.Account.LinkUsers;

public partial class LinkUsersModal
{
    protected Modal _modal;
    protected Modal _deleteConfirmationModal;
    protected Modal _newLinkUserConfirmationModal;

    [Inject]
    protected IOptions<AbpAccountLinkUserOptions> Options { get; set; }

    [Inject]
    protected IIdentityLinkUserAppService LinkUserAppService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IAbpAccessTokenProvider AccessTokenProvider { get; set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    protected ListResultDto<LinkUserDto> LinkUsers { get; set; }

    protected int PageSize { get; set; } = 5;

    protected string DeleteConfirmationMessage { get; set; }

    protected Guid? DeleteTenantId { get; set; }
    protected Guid DeleteUserId { get; set; }

    protected string PostAction { get; set; }
    protected string SourceLinkToken { get; set; }
    protected Guid? TargetLinkTenantId { get; set; }
    protected Guid TargetLinkUserId { get; set; }
    protected string ReturnUrl { get; set; }

    private DataGrid<LinkUserDto> _linkUsersDataGrid;
    private string _customFilterValue;

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("eval", "Array.prototype.forEach.call(document.querySelectorAll('.account_link_users_modals'), element => document.body.appendChild(element));");
        }
    }

    private Task OnCustomFilterValueChanged(string e)
    {
        _customFilterValue = e;
        return _linkUsersDataGrid.Reload();
    }

    private bool OnCustomFilter(LinkUserDto linkUser)
    {
        if (_customFilterValue.IsNullOrWhiteSpace())
        {
            return true;
        }

        return linkUser.TargetTenantName?.Contains(_customFilterValue, StringComparison.OrdinalIgnoreCase) == true ||
               linkUser.TargetUserName?.Contains(_customFilterValue, StringComparison.OrdinalIgnoreCase) == true;
    }

    public LinkUsersModal()
    {
        LocalizationResource = typeof(AccountResource);
    }

    protected virtual async Task OpenModalAsync()
    {
        LinkUsers = await LinkUserAppService.GetAllListAsync();
        await InvokeAsync(_modal.Show);
    }

    protected virtual Task CloseModalAsync(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual Task CloseDeleteConfirmationModalAsync(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual Task CLoseNewLinkUserConfirmationModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task CloseSpecifyModalAsync(Modal modal)
    {
        await modal.Hide();
    }

    protected virtual async Task OpenDeleteConfirmationModalAsync(Guid? tenantId, Guid userId, string userName)
    {
        DeleteTenantId = tenantId;
        DeleteUserId = userId;
        DeleteConfirmationMessage = L["DeleteLinkAccountConfirmationMessage", userName];
        await _deleteConfirmationModal.Show();
    }

    protected virtual async Task NewLinkAccountAsync(bool isConfirmed)
    {
        if (!isConfirmed)
        {
            await _newLinkUserConfirmationModal.Show();
            return;
        }

        var linkToken = await LinkUserAppService.GenerateLinkTokenAsync();

        var loginUrl = Options.Value.LoginUrl?.EnsureEndsWith('/') ?? "/";
        var returnUrl = NavigationManager.BaseUri.EnsureEndsWith('/') + "Account/Challenge";
        if (loginUrl == "/" || loginUrl == NavigationManager.BaseUri.EnsureEndsWith('/'))
        {
            returnUrl = NavigationManager.BaseUri.EnsureEndsWith('/');
        }


        var url =
            loginUrl +
            "Account/Login?handler=CreateLinkUser&" +
            "LinkUserId=" +
            CurrentUser.Id +
            "&LinkToken=" +
            UrlEncoder.Default.Encode(linkToken) +
            "&ReturnUrl=" + returnUrl;

        if (CurrentTenant.Id != null)
        {
            url += "&LinkTenantId=" + CurrentTenant.Id;
        }

        NavigationManager.NavigateTo(url, true);
    }

    protected virtual async Task LoginAsThisAccountAsync(Guid? tenantId, Guid userId)
    {
        PostAction = "/Account/LinkLogin";
        ReturnUrl = NavigationManager.Uri;
        if (!Options.Value.LoginUrl.IsNullOrEmpty())
        {
            var accessToken = await AccessTokenProvider.GetTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                PostAction = Options.Value.LoginUrl.EnsureEndsWith('/') + "Account/LinkLogin";
                PostAction += "?access_token=" + accessToken;
                ReturnUrl = NavigationManager.BaseUri.EnsureEndsWith('/') + "Account/Challenge";
            }
        }

        TargetLinkTenantId = tenantId;
        TargetLinkUserId = userId;
        SourceLinkToken = await LinkUserAppService.GenerateLinkLoginTokenAsync();

        await InvokeAsync(StateHasChanged);

        await JSRuntime.InvokeVoidAsync("eval", "document.getElementById('linkUserLoginForm').submit()");
    }

    protected virtual async Task DeleteUsersAsync()
    {
        await _deleteConfirmationModal.Hide();

        await LinkUserAppService.UnlinkAsync(new UnLinkUserInput
        {
            TenantId = DeleteTenantId,
            UserId = DeleteUserId
        });

        DeleteTenantId = default;
        DeleteUserId = default;

        DeleteConfirmationMessage = string.Empty;

        LinkUsers = await LinkUserAppService.GetAllListAsync();
        await InvokeAsync(StateHasChanged);
    }
}
