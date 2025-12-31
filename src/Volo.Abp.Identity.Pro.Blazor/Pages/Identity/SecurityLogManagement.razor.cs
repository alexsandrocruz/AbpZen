using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

public partial class SecurityLogManagement
{
    [Inject] protected IIdentitySecurityLogAppService AppService { get; set; }

    protected int CurrentPage = 1;
    protected string CurrentSorting;
    protected int? TotalCount;
    protected List<BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new();
    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected GetIdentitySecurityLogListInput Filter = new();
    
    public string DateFormat => CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;

    public string TimeFormat => CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;

    public bool TimeAs24hr => !CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern.Contains("tt");

    protected IReadOnlyList<IdentitySecurityLogDto> Entities = Array.Empty<IdentitySecurityLogDto>();

    protected PageToolbar Toolbar { get; } = new();
    protected TableColumnDictionary TableColumns { get; } = new();
    protected EntityActionDictionary EntityActions { get; } = new();
    protected List<TableColumn> SecurityLogManagementTableColumns => TableColumns.Get<SecurityLogManagement>();

    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;
        await GetEntitiesAsync();
    }

    protected virtual async Task GetEntitiesAsync()
    {
        Filter.Sorting = CurrentSorting;
        Filter.SkipCount = (CurrentPage - 1) * PageSize;
        Filter.MaxResultCount = PageSize;

        Filter.ApplicationName = Filter.ApplicationName?.Trim();
        Filter.Identity = Filter.Identity?.Trim();
        Filter.UserName = Filter.UserName?.Trim();
        Filter.ClientId = Filter.ClientId?.Trim();
        Filter.CorrelationId = Filter.CorrelationId?.Trim();
        Filter.ClientIpAddress = Filter.ClientIpAddress?.Trim();

        var result = await AppService.GetListAsync(Filter);
        Entities = result.Items;

        TotalCount = (int?)result.TotalCount;
    }

    protected virtual async Task ClearFilterAsync()
    {
        Filter = new GetIdentitySecurityLogListInput();
        await GetEntitiesAsync();
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<IdentitySecurityLogDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SetBreadcrumbItemsAsync();
            await SetToolbarItemsAsync();
            await InvokeAsync(StateHasChanged);
        }
    }  

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:IdentityManagement"]));
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["SecurityLogs"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetTableColumnsAsync()
    {
        SecurityLogManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["SecurityLogs:Date"],
                        Data = nameof(IdentitySecurityLogDto.CreationTime),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:Action"],
                        Data = nameof(IdentitySecurityLogDto.Action),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:IpAddress"],
                        Data = nameof(IdentitySecurityLogDto.ClientIpAddress),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:Browser"],
                        Data = nameof(IdentitySecurityLogDto.BrowserInfo),
                        Sortable = true,
                        Component = typeof(SecurityLogBrowserInfoComponent)
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:Application"],
                        Data = nameof(IdentitySecurityLogDto.ApplicationName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:Identity"],
                        Data = nameof(IdentitySecurityLogDto.Identity),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:UserName"],
                        Data = nameof(IdentitySecurityLogDto.UserName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:Client"],
                        Data = nameof(IdentitySecurityLogDto.ClientId),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["SecurityLogs:CorrelationId"],
                        Data = nameof(IdentitySecurityLogDto.CorrelationId),
                        Sortable = true,
                    },
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        return ValueTask.CompletedTask;
    }
}
