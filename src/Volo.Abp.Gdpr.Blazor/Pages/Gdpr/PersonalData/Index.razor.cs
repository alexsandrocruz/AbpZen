using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace Volo.Abp.Gdpr.Blazor.Pages.Gdpr.PersonalData;

public partial class Index
{
    protected IReadOnlyList<GdprRequestDto> GdprRequests = new List<GdprRequestDto>();

    protected GdprRequestInput GetListInput { get; } = new();
    protected int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    protected int CurrentPage { get; set; } = 1;
    protected string CurrentSorting { get; set; }
    protected int? TotalCount { get; set; }
    protected bool IsNewRequestAllowed { get; set; }
    
    protected List<BlazoriseUI.BreadcrumbItem> BreadcrumbItems { get; } = new();
    
    protected async override Task OnInitializedAsync()
    {
        await GetGdprRequestsAsync();
        
        IsNewRequestAllowed = await GdprRequestAppService.IsNewRequestAllowedAsync();
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected virtual async Task GetGdprRequestsAsync()
    {
        GetListInput.UserId = CurrentUser.GetId();
        GetListInput.Sorting = CurrentSorting;
        GetListInput.SkipCount = (CurrentPage - 1) * PageSize;
        GetListInput.MaxResultCount = PageSize;
        
        var result = await GdprRequestAppService.GetListAsync(GetListInput);
        GdprRequests = result.Items;
        TotalCount = (int?)result.TotalCount;
    }

    protected virtual async Task RequestGdprDataAsync()
    {
        try
        {
            await GdprRequestAppService.PrepareUserDataAsync();
            await GetGdprRequestsAsync();
            await Message.Success(L["PersonalDataPrepareRequestReceived"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task DeletePersonalDataAsync()
    {
        try
        {
            if (await Message.Confirm(L["DeletePersonalDataWarning"]))
            {
                await GdprRequestAppService.DeleteUserDataAsync();
                await Message.Success(L["PersonalDataDeleteRequestReceived"]);
                await GetGdprRequestsAsync();
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task DownloadPersonalDataAsync(Guid requestId)
    {
        try
        {
            var baseUrl = await ServerUrlProvider.GetBaseUrlAsync(GdprRemoteServiceConsts.RemoteServiceName);
            var downloadToken = await GdprRequestAppService.GetDownloadTokenAsync(requestId);
            var culture = CultureInfo.CurrentUICulture.Name ?? CultureInfo.CurrentCulture.Name;
            if(!culture.IsNullOrEmpty())
            {
                culture = "&culture=" + culture;
            }
            NavigationManager.NavigateTo($"{baseUrl.EnsureEndsWith('/')}api/gdpr/requests/data/{requestId}?token={downloadToken.Token}{culture}", forceLoad: true);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    protected ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:PersonalData"]));
        return ValueTask.CompletedTask;
    }
    
    private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<GdprRequestDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;
        
        await GetGdprRequestsAsync();
        await InvokeAsync(StateHasChanged);
    }
}