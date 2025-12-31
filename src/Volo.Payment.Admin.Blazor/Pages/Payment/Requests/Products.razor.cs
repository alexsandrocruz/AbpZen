using Blazorise.DataGrid;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Localization;
using Volo.Payment.Requests;

namespace Volo.Payment.Admin.Blazor.Pages.Payment.Requests;

public partial class Products
{
    [Inject] protected IPaymentRequestAppService PaymentRequestAdminAppService { get; set; }

    [Parameter] public string Id { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    protected TableColumnDictionary TableColumns { get; } = new();

    protected List<TableColumn> PaymentRequestProductsTableColumns => TableColumns.Get<Index>();

    protected List<BreadcrumbItem> BreadcrumbItems { get; } = new();

    protected PaymentRequestWithDetailsDto PaymentRequest { get; set; }

    protected IReadOnlyList<PaymentRequestProductDto> Entities = Array.Empty<PaymentRequestProductDto>();

    public Products()
    {
        LocalizationResource = typeof(PaymentResource);
        ObjectMapperContext = typeof(AbpPaymentAdminBlazorModule);
    }

    protected override async Task OnInitializedAsync()
    {
        await SetBreadCrumbsAsync();
        await SetTableColumnsAsync();
        await InvokeAsync(StateHasChanged);
    }

    private ValueTask SetBreadCrumbsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:PaymentManagement"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:PaymentRequests"], "/payment/requests"));
        BreadcrumbItems.Add(new BreadcrumbItem(Id));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Products"]));
        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask SetTableColumnsAsync()
    {
        PaymentRequestProductsTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["DisplayName:Code"],
                        Data = nameof(PaymentRequestProductDto.Code),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:Name"],
                        Data = nameof(PaymentRequestProductDto.Name),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:Count"],
                        Data = nameof(PaymentRequestProductDto.Count),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:UnitPrice"],
                        Sortable = true,
                        Component = typeof(ProductsUnitPriceCellComponent)
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:TotalPrice"],
                        Sortable = true,
                        Component = typeof(ProductsTotalPriceCellComponent)
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:PaymentType"],
                        Data = nameof(PaymentRequestProductDto.PaymentType),
                        Sortable = true,
                    }
            });

        PaymentRequestProductsTableColumns.AddRange(
            await GetExtensionTableColumnsAsync(PaymentModuleExtensionConsts.ModuleName,
                PaymentModuleExtensionConsts.EntityNames.PaymentRequestProduct));
    }

    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            if (Guid.TryParse(Id, out Guid paymentRequestId))
            {
                Console.WriteLine("Parsed as: " + paymentRequestId);
                var PaymentRequest = await PaymentRequestAdminAppService.GetAsync(paymentRequestId);
                Entities = PaymentRequest.Products;
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<PaymentRequestProductDto> e)
    {
        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }
}
