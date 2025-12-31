using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.BlazoriseUI.Components.ObjectExtending;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Gateways;
using Volo.Payment.Localization;
using Volo.Payment.Plans;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.Payment.Admin.Blazor.Pages.Payment.Plans;

public partial class GatewayPlanManagement
{
    [Parameter] public Guid PlanId { get; set; }

    public PlanDto Plan { get; private set; }
    protected PageToolbar Toolbar { get; } = new();
    protected TableColumnDictionary TableColumns { get; } = new();
    protected EntityActionDictionary EntityActions { get; } = new();
    protected List<TableColumn> GatewayPlanManagementTableColumns => TableColumns.Get<GatewayPlanManagement>();

    protected List<BreadcrumbItem> BreadcrumbItems = new(2);

    protected bool HasCreatePermission = false;
    protected bool HasUpdatePermission = false;
    protected bool HasDeletePermission = false;

    [Inject] protected IPlanAdminAppService PlanAdminAppService { get; set; }

    [Inject] protected IGatewayAppService GatewayAppService { get; set; }

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int CurrentPage = 1;
    protected string CurrentSorting;
    protected int? TotalCount;

    protected List<GatewayDto> SubscriptionSupportedGateways = new();

    protected GatewayPlanGetListInput GetListInput = new();
    protected IReadOnlyList<GatewayPlanDto> Entities = Array.Empty<GatewayPlanDto>();

    protected GatewayPlanCreateInput NewEntity = new();
    protected string EditingEntityId;
    protected GatewayPlanUpdateInput EditingEntity = new();
    protected Modal CreateModal;
    protected Modal EditModal;
    protected Validations CreateValidationsRef;
    protected Validations EditValidationsRef;

    public GatewayPlanManagement()
    {
        LocalizationResource = typeof(PaymentResource);
        ObjectMapperContext = typeof(AbpPaymentAdminBlazorModule);
    }

    protected override async Task OnInitializedAsync()
    {
        HasCreatePermission =
            await AuthorizationService.IsGrantedAsync(PaymentAdminPermissions.Plans.GatewayPlans.Create);
        HasUpdatePermission =
            await AuthorizationService.IsGrantedAsync(PaymentAdminPermissions.Plans.GatewayPlans.Update);
        HasDeletePermission =
            await AuthorizationService.IsGrantedAsync(PaymentAdminPermissions.Plans.GatewayPlans.Delete);

        await GetPlanAsync();
        await SetSupportedGatewaysAsync();
        await SetBreadCrumbsAsync();
        await SetToolbarItemsAsync();
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<GatewayPlanManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Visible = (data) => HasUpdatePermission,
                        Text = L["Edit"],
                        Clicked = async (data) => await OpenEditModalAsync(data.As<GatewayPlanDto>())
                    },
                    new EntityAction
                    {
                        Visible = (data) => HasDeletePermission,
                        Text = L["Delete"],
                        Clicked = async (data) => await DeleteEntityAsync(data.As<GatewayPlanDto>()),
                        ConfirmationMessage = (data) => L["GatewayPlanDeletionConfirmationMessage"]
                    }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask SetTableColumnsAsync()
    {
        GatewayPlanManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<GatewayPlanManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:Gateway"],
                        Data = nameof(GatewayPlanDto.Gateway),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:ExternalId"],
                        Data = nameof(GatewayPlanDto.ExternalId),
                        Sortable = true,
                    }
            });

        GatewayPlanManagementTableColumns.AddRange(
            await GetExtensionTableColumnsAsync(PaymentModuleExtensionConsts.ModuleName,
                PaymentModuleExtensionConsts.EntityNames.GatewayPlan));
    }

    private async Task GetPlanAsync()
    {
        Plan = await PlanAdminAppService.GetAsync(PlanId);
    }

    private async Task SetBreadCrumbsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:PaymentManagement"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Plans"]));
        BreadcrumbItems.Add(new BreadcrumbItem(Plan.Name));
    }

    protected virtual async Task SetSupportedGatewaysAsync()
    {
        SubscriptionSupportedGateways = await GatewayAppService.GetSubscriptionSupportedGatewaysAsync();
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewGatewayPlan"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: PaymentAdminPermissions.Plans.GatewayPlans.Create);

        return ValueTask.CompletedTask;
    }

    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            await UpdateGetListInputAsync();
            var result = await PlanAdminAppService.GetGatewayPlansAsync(PlanId, GetListInput);
            Entities = result.Items.As<IReadOnlyList<GatewayPlanDto>>();
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

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<GatewayPlanDto> e)
    {
        CurrentSorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.Page;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task DeleteEntityAsync(GatewayPlanDto entity)
    {
        if (!HasDeletePermission)
        {
            return;
        }

        try
        {
            await PlanAdminAppService.DeleteGatewayPlanAsync(PlanId, entity.Gateway);

            await GetEntitiesAsync();
            await InvokeAsync(EditModal.Hide);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenEditModalAsync(GatewayPlanDto entity)
    {
        if (!HasUpdatePermission)
        {
            return;
        }

        try
        {
            EditValidationsRef?.ClearAll();

            EditingEntityId = entity.Gateway;
            EditingEntity = ObjectMapper.Map<GatewayPlanDto, GatewayPlanUpdateInput>(entity);

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (EditModal != null)
                {
                    await EditModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual Task ClosingEditModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual Task ClosingCreateModal(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual Task CloseEditModalAsync()
    {
        InvokeAsync(EditModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual Task CloseCreateModalAsync()
    {
        InvokeAsync(CreateModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual async Task OpenCreateModalAsync()
    {
        try
        {
            CreateValidationsRef?.ClearAll();

            NewEntity = new GatewayPlanCreateInput
            {
                Gateway = SubscriptionSupportedGateways.FirstOrDefault()?.Name
            };

            await InvokeAsync(async () =>
            {
                StateHasChanged();
                if (CreateModal != null)
                {
                    await CreateModal.Show();
                }
            });
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task CreateEntityAsync()
    {
        if (!HasCreatePermission)
        {
            return;
        }

        try
        {
            var validate = true;
            if (CreateValidationsRef != null)
            {
                validate = await CreateValidationsRef.ValidateAll();
            }
            if (validate)
            {
                await PlanAdminAppService.CreateGatewayPlanAsync(PlanId, NewEntity);

                await GetEntitiesAsync();
                await InvokeAsync(CreateModal.Hide);
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateEntityAsync()
    {
        if (!HasUpdatePermission)
        {
            return;
        }

        try
        {
            var validate = true;
            if (EditValidationsRef != null)
            {
                validate = await EditValidationsRef.ValidateAll();
            }
            if (validate)
            {
                await PlanAdminAppService.UpdateGatewayPlanAsync(PlanId, EditingEntityId, EditingEntity);

                await GetEntitiesAsync();
                await InvokeAsync(EditModal.Hide);
            }
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}
