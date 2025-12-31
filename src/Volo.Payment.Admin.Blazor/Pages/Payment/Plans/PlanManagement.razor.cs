using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Admin.Permissions;
using Volo.Payment.Localization;
using Volo.Payment.Plans;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.Payment.Admin.Blazor.Pages.Payment.Plans;

public partial class PlanManagement
{
    protected PageToolbar Toolbar { get; } = new();
    protected List<TableColumn> PlanManagementTableColumns => TableColumns.Get<PlanManagement>();
    public PlanManagement()
    {
        LocalizationResource = typeof(PaymentResource);
        ObjectMapperContext = typeof(AbpPaymentAdminBlazorModule);

        CreatePolicyName = PaymentAdminPermissions.Plans.Create;
        UpdatePolicyName = PaymentAdminPermissions.Plans.Update;
        DeletePolicyName = PaymentAdminPermissions.Plans.Delete;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:PaymentManagement"]));
        BreadcrumbItems.Add(new BreadcrumbItem(L["Plans"]));
        return ValueTask.CompletedTask;
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewPlan"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<PlanManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["GatewayPlans"],
                        Clicked = (data) => RouteGatewayPlans(data.As<PlanDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Clicked = async (data) => await OpenEditModalAsync(data.As<PlanDto>()),
                        Visible = (data) => HasUpdatePermission
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Clicked = async (data) => await DeleteEntityAsync(data.As<PlanDto>()),
                        Visible = (data) => HasDeletePermission,
                        ConfirmationMessage = (data) => L["PlanDeletionConfirmationMessage"]
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected async override ValueTask SetTableColumnsAsync()
    {
        PlanManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<PlanManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:Name"],
                        Data = nameof(PlanDto.Name),
                        Sortable = true,
                    },
            });

        PlanManagementTableColumns.AddRange(
            await GetExtensionTableColumnsAsync(PaymentModuleExtensionConsts.ModuleName,
                PaymentModuleExtensionConsts.EntityNames.Plan));

        await base.SetTableColumnsAsync();
    }

    protected virtual Task RouteGatewayPlans(PlanDto context)
    {
        NavigationManager.NavigateTo($"/payment/plans/{context.Id}/external-plans");

        return Task.CompletedTask;
    }
}
