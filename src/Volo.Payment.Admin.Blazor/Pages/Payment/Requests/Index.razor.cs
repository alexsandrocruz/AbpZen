using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Admin.Requests;
using Volo.Payment.Localization;
using Volo.Payment.Requests;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.Payment.Admin.Blazor.Pages.Payment.Requests;

public partial class Index
{
    [Inject] protected IPaymentRequestAdminAppService PaymentRequestAdminAppService { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    protected TableColumnDictionary TableColumns { get; } = new();

    protected List<TableColumn> PaymentRequestTableColumns => TableColumns.Get<Index>();

    protected List<BreadcrumbItem> BreadcrumbItems { get; } = new();

    protected EntityActionDictionary EntityActions { get; } = new();

    protected IReadOnlyList<PaymentRequestWithDetailsDto> Entities = Array.Empty<PaymentRequestWithDetailsDto>();

    protected IReadOnlyList<PaymentRequestState> PaymentRequestStates => Enum.GetValues<PaymentRequestState>();

    protected IReadOnlyList<PaymentType> PaymentTypes => Enum.GetValues<PaymentType>();

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected PaymentRequestGetListInput GetListInput { get; } = new();

    protected int CurrentPage { get; set; } = 1;
    protected string CurrentSorting { get; set; }
    protected int? TotalCount { get; set; }

    public Index()
    {
        LocalizationResource = typeof(PaymentResource);
        ObjectMapperContext = typeof(AbpPaymentAdminBlazorModule);
    }

    protected override async Task OnInitializedAsync()
    {
        await SetBreadCrumbsAsync();
        await SetTableColumnsAsync();
        await SetEntityActionsAsync();
        await InvokeAsync(StateHasChanged);
    }

    private ValueTask SetBreadCrumbsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:PaymentManagement"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:PaymentRequests"]));
        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<Index>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Products"],
                        Clicked = async (data) => NavigationManager.NavigateTo($"/payment/requests/{data.As<PaymentRequestWithDetailsDto>().Id}/products")
                    },
            });

        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask SetTableColumnsAsync()
    {
        PaymentRequestTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<Index>()
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:CreationTime"],
                        Data = nameof(PaymentRequestWithDetailsDto.CreationTime),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:TotalPrice"],
                        Sortable = true,
                        Component = typeof(IndexTotalPriceCellComponent)
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:Currency"],
                        Data = nameof(PaymentRequestWithDetailsDto.Currency),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:State"],
                        Data = nameof(PaymentRequestWithDetailsDto.State),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:Gateway"],
                        Data = nameof(PaymentRequestWithDetailsDto.Gateway),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:ExternalSubscriptionId"],
                        Data = nameof(PaymentRequestWithDetailsDto.ExternalSubscriptionId),
                        Sortable = true,
                    }
            });

        PaymentRequestTableColumns.AddRange(
            await GetExtensionTableColumnsAsync(PaymentModuleExtensionConsts.ModuleName,
                PaymentModuleExtensionConsts.EntityNames.PaymentRequest));
    }

    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            await UpdateGetListInputAsync();
            var result = await PaymentRequestAdminAppService.GetListAsync(GetListInput);
            Entities = result.Items.As<IReadOnlyList<PaymentRequestWithDetailsDto>>();
            TotalCount = (int?)result.TotalCount;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task UpdateGetListInputAsync()
    {
        GetListInput.Sorting = CurrentSorting;
        GetListInput.SkipCount = (CurrentPage - 1) * PageSize;
        GetListInput.MaxResultCount = PageSize;

        return Task.CompletedTask;
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PaymentRequestWithDetailsDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }
	
	protected virtual async Task OnCreationTimeChangedAsync(IReadOnlyList<DateTime?> dates)
	{
		GetListInput.CreationDateMin = dates.Min();
		GetListInput.CreationDateMax = dates.Max();

		if (GetListInput.CreationDateMax.HasValue)
		{
			GetListInput.CreationDateMax = GetListInput.CreationDateMax.Value.ClearTime().Add(new TimeSpan(23, 59, 59));
		}

		await GetEntitiesAsync();
	}
}
