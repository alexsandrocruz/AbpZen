using System.Collections.Generic;
using Volo.Abp.BlazoriseUI;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Threading;

namespace Volo.Abp.AuditLogging.Blazor.Pages.AuditLogging;

public partial class AuditLogsManagement
{
    protected string SelectedTab = "AuditLogging";
    protected List<BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new();
    protected PageToolbar Toolbar { get; } = new();

    protected async override Task OnInitializedAsync()
    {
        await SetBreadcrumbItemsAsync();
        await SetToolbarItemsAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Task.Yield();
            await InvokeAsync(StateHasChanged);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["AuditLogs"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        return ValueTask.CompletedTask;
    }
}
