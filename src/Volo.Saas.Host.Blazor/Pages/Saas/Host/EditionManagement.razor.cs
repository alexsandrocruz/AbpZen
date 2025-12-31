using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.FeatureManagement.Blazor.Components;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Plans;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Localization;

namespace Volo.Saas.Host.Blazor.Pages.Saas.Host;

public partial class EditionManagement
{
    protected Modal DeleteModal;

    protected const string FeatureProviderName = "E";

    protected FeatureManagementModal FeatureManagementModal;

    protected Modal MoveAllTenantsModal;
    protected MoveAllTenantsViewModel MoveAllTenantsModel { get; set; }

    protected string ManageFeaturesPolicyName;
    protected bool HasManageFeaturesPermission;

    protected PageToolbar Toolbar { get; } = new();
    protected List<TableColumn> EditionManagementTableColumns => TableColumns.Get<EditionManagement>();
    protected List<PlanDto> Plans { get; set; }

    protected DeleteEditionViewModel DeletingEdition { get; set; }

    public EditionManagement()
    {
        ObjectMapperContext = typeof(SaasHostBlazorModule);
        LocalizationResource = typeof(SaasResource);

        CreatePolicyName = SaasHostPermissions.Editions.Create;
        UpdatePolicyName = SaasHostPermissions.Editions.Update;
        DeletePolicyName = SaasHostPermissions.Editions.Delete;
        ManageFeaturesPolicyName = SaasHostPermissions.Editions.ManageFeatures;
    }

    protected virtual async Task DeleteEditionAsync()
    {
        try
        {
            await CheckDeletePolicyAsync();
            await OnDeletingEntityAsync();
            if (DeletingEdition.AssignToEditionId == Guid.Empty)
            {
                DeletingEdition.AssignToEditionId = null;
            }
            await AppService.MoveAllTenantsAsync(DeletingEdition.Id, DeletingEdition.AssignToEditionId);
            await AppService.DeleteAsync(DeletingEdition.Id);
            await CloseDeleteModalAsync();
            await OnDeletedEntityAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenDeleteModalAsync(EditionDto data)
    {
        var edition = await AppService.GetAsync(data.Id);
        var allEditions = await AppService.GetAllListAsync();
        DeletingEdition = new DeleteEditionViewModel
        {
            Id = edition.Id,
            DisplayName = edition.DisplayName,
            TenantCount = edition.TenantCount,
            OtherEditions = allEditions.Where(x => x.Id != edition.Id).ToList()
        };

        await InvokeAsync(DeleteModal.Show);
    }

    protected virtual Task ClosingDeleteModalAsync(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task CloseDeleteModalAsync()
    {
        InvokeAsync(DeleteModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual void OnAssignCheckedChanged()
    {
        DeletingEdition.AssignEdition = !DeletingEdition.AssignEdition;
        if (!DeletingEdition.AssignEdition)
        {
            DeletingEdition.AssignToEditionId = Guid.Empty;
            DeletingEdition.DisabledDeleteButton = false;
        }
        else
        {
            DeletingEdition.DisabledDeleteButton = true;
        }
    }
    
    protected virtual void OnAssignToEditionSelectedValueChanged(Guid? id)
    {
        DeletingEdition.AssignToEditionId = id;

        DeletingEdition.DisabledDeleteButton = DeletingEdition.AssignToEditionId == Guid.Empty;
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Saas"]));
        BreadcrumbItems.Add(new Abp.BlazoriseUI.BreadcrumbItem(L["Editions"]));

        return ValueTask.CompletedTask;
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();
        HasManageFeaturesPermission = await AuthorizationService.IsGrantedAsync(ManageFeaturesPolicyName);
    }

    protected override string GetDeleteConfirmationMessage(EditionDto entity)
    {
        return string.Format(L["EditionDeletionConfirmationMessage"], entity.DisplayName);
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<EditionManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data)=> HasUpdatePermission,
                        Clicked = async (data) => await OpenEditModalAsync(data.As<EditionDto>())
                    },
                    new EntityAction
                    {
                        Text = L["Features"],
                        Visible = (data)=> HasManageFeaturesPermission,
                        Clicked = async (data) => await FeatureManagementModal.OpenAsync(FeatureProviderName, data.As<EditionDto>().Id.ToString(), data.As<EditionDto>().DisplayName)
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data)=> HasDeletePermission,
                        Clicked = async (data) =>
                        {
                            await OpenDeleteModalAsync(data.As<EditionDto>());
                        }
                    },
                    new EntityAction
                    {
                        Text = L["MoveAllTenants"],
                        Visible = (data)=> HasUpdatePermission,
                        Clicked = async (data) => await OpenMoveAllTenantsModalAsync(data.As<EditionDto>()),
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        EditionManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<EditionManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["EditionName"],
                        Data = nameof(EditionDto.DisplayName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["PlanName"],
                        Data = nameof(EditionDto.PlanName)
                    },
                    new TableColumn
                    {
                        Title = L["TenantCount"],
                        Data = nameof(EditionDto.TenantCount)
                    }
            });

        EditionManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(SaasModuleExtensionConsts.ModuleName,
            SaasModuleExtensionConsts.EntityNames.Edition));

        await base.SetTableColumnsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewEdition"], OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override async Task OpenCreateModalAsync()
    {
        Plans = await AppService.GetPlanLookupAsync();

        await base.OpenCreateModalAsync();
    }

    protected override async Task OpenEditModalAsync(EditionDto entity)
    {
        Plans = await AppService.GetPlanLookupAsync();

        await base.OpenEditModalAsync(entity);
    }


    protected virtual async Task OpenMoveAllTenantsModalAsync(EditionDto data)
    {
        if (data.TenantCount == 0)
        {
            await Message.Warn(L["ThereIsNoTenantsCurrentlyInThisEdition"]);
            return;
        }

        var edition = await AppService.GetAsync(data.Id);
        var allEditions = await AppService.GetAllListAsync();
        MoveAllTenantsModel = new MoveAllTenantsViewModel()
        {
            Id = edition.Id,
            Name = edition.DisplayName,
            TargetEditions = allEditions.Where(x => x.Id != edition.Id).ToList(),
            TargetEditionId = Guid.Empty
        };

        await InvokeAsync(MoveAllTenantsModal.Show);
    }

    protected virtual Task ClosingMoveAllTenantsModalAsync(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task CloseMoveAllTenantsModalAsync()
    {
        InvokeAsync(MoveAllTenantsModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual async Task MoveAllTenantsAsync()
    {
        try
        {
            await CheckUpdatePolicyAsync();
            if (MoveAllTenantsModel.TargetEditionId == Guid.Empty)
            {
                MoveAllTenantsModel.TargetEditionId = null;
            }
            await AppService.MoveAllTenantsAsync(MoveAllTenantsModel.Id, MoveAllTenantsModel.TargetEditionId);
            await GetEntitiesAsync();
            await CloseMoveAllTenantsModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected override Task CreateEntityAsync()
    {
        if (NewEntity.PlanId == Guid.Empty)
        {
            NewEntity.PlanId = null;
        }
        return base.CreateEntityAsync();
    }

    protected override Task UpdateEntityAsync()
    {
        if (EditingEntity.PlanId == Guid.Empty)
        {
            EditingEntity.PlanId = null;
        }
        return base.UpdateEntityAsync();
    }

    public class DeleteEditionViewModel
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public long TenantCount { get; set; }

        public List<EditionDto> OtherEditions { get; set; } = new();

        public bool AssignEdition { get; set; }

        public Guid? AssignToEditionId { get; set; } = Guid.Empty;

        public bool DisabledDeleteButton { get; set; } 
    }


    public class MoveAllTenantsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<EditionDto> TargetEditions { get; set; } = new();

        public Guid? TargetEditionId { get; set; }
    }
}
