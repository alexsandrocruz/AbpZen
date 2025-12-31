using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

public partial class RoleManagement
{
    protected Modal DeleteModal;

    protected Modal MoveAllUsersModal;

    protected MoveAllUsersViewModel MoveAllUsersModel { get; set; }

    protected const string PermissionProviderName = "R";

    protected string ManagePermissionsPolicyName;

    protected PermissionManagementModal PermissionManagementModal;

    protected bool HasManagePermissionsPermission { get; set; }

    protected List<TableColumn> RoleManagementTableColumns => TableColumns.Get<RoleManagement>();
    protected PageToolbar Toolbar { get; } = new();

    protected DeleteRoleViewModel DeletingRole { get; set; }

    private string _filter = null;

    protected string Filter {
        get => string.IsNullOrWhiteSpace(_filter) ? "" : _filter;
        set => _filter = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }

    public RoleManagement()
    {
        ObjectMapperContext = typeof(AbpIdentityProBlazorModule);
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.Roles.Create;
        UpdatePolicyName = IdentityPermissions.Roles.Update;
        DeletePolicyName = IdentityPermissions.Roles.Delete;
        ManagePermissionsPolicyName = IdentityPermissions.Roles.ManagePermissions;
    }

    protected virtual async Task OpenDeleteModalAsync(IdentityRoleDto data)
    {
        var role = await AppService.GetAsync(data.Id);
        var allRoles = await AppService.GetAllListAsync();
        DeletingRole = new DeleteRoleViewModel
        {
            Id = role.Id,
            Name = role.Name,
            UserCount = role.UserCount,
            OtherRoles = allRoles.Items.Where(x => x.Id != role.Id).ToList()
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

    protected virtual async Task DeleteRoleAsync()
    {
        try
        {
            await CheckDeletePolicyAsync();
            await OnDeletingEntityAsync();
            if (DeletingRole.AssignToRoleId == Guid.Empty)
            {
                DeletingRole.AssignToRoleId = null;
            }
            await AppService.MoveAllUsersAsync(DeletingRole.Id, DeletingRole.AssignToRoleId);
            await AppService.DeleteAsync(DeletingRole.Id);
            await CloseDeleteModalAsync();
            await OnDeletedEntityAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenMoveAllUsersModalAsync(IdentityRoleDto role)
    {
        if (role.UserCount == 0)
        {
            await Message.Warn(L["ThereIsNoUsersCurrentlyInThisRole"]);
            return;
        }
        
        var allRoles = await AppService.GetAllListAsync();
        MoveAllUsersModel = new MoveAllUsersViewModel()
        {
            CurrentRoleId = role.Id,
            CurrentRoleName = role.Name,
            TargetRoles =  allRoles.Items.Where(x => x.Id != role.Id).ToList()
        };

        await InvokeAsync(MoveAllUsersModal.Show);
    }

    protected virtual Task ClosingMoveAllUsersModalAsync(ModalClosingEventArgs eventArgs)
    {
        // cancel close if clicked outside of modal area
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;

        return Task.CompletedTask;
    }

    protected virtual Task CloseMoveAllUsersModalAsync()
    {
        InvokeAsync(MoveAllUsersModal.Hide);
        return Task.CompletedTask;
    }

    protected virtual async Task MoveAllUsersAsync()
    {
        try
        {
            await CheckUpdatePolicyAsync();
            if (MoveAllUsersModel.TargetRoleId == Guid.Empty)
            {
                MoveAllUsersModel.TargetRoleId = null;
            }
            await AppService.MoveAllUsersAsync(MoveAllUsersModel.CurrentRoleId, MoveAllUsersModel.TargetRoleId);
            await GetEntitiesAsync();
            await CloseMoveAllUsersModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }

    }

    protected virtual void OnAssignCheckedChanged()
    {
        DeletingRole.AssignRole = !DeletingRole.AssignRole;
        if (!DeletingRole.AssignRole)
        {
            DeletingRole.AssignToRoleId = Guid.Empty;
            DeletingRole.DisabledDeleteButton = false;
        }
        else
        {
            DeletingRole.DisabledDeleteButton = true;
        }
    }

    protected virtual void OnAssignToRoleSelectedValueChanged(Guid? id)
    {
        DeletingRole.AssignToRoleId = id;

        DeletingRole.DisabledDeleteButton = DeletingRole.AssignToRoleId == Guid.Empty;
    }

    protected override async Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(ManagePermissionsPolicyName);
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:IdentityManagement"]));
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Roles"]));
        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(IdentityRoleDto entity)
    {
        return string.Format(L["RoleDeletionConfirmationMessage"], entity.Name);
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewRole"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<RoleManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditModalAsync(data.As<IdentityRoleDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Permissions"],
                        Visible = (data) => HasManagePermissionsPermission,
                        Clicked = async (data) =>
                        {
                            var role = data.As<IdentityRoleDto>();
                            await PermissionManagementModal.OpenAsync(PermissionProviderName,
                                role.Name,role.Name);
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) =>
                        {
                            await OpenDeleteModalAsync(data.As<IdentityRoleDto>());
                        }
                    },
                    new EntityAction
                    {
                        Text = L["MoveAllUsers"],
                        Visible = (data)=> HasUpdatePermission,
                        Clicked = async (data) => await OpenMoveAllUsersModalAsync(data.As<IdentityRoleDto>()),
                    }
            });

        return base.SetEntityActionsAsync();
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        RoleManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<RoleManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["RoleName"],
                        Data = nameof(IdentityRoleDto.Name),
                        Sortable = true,
                        Component = typeof(RoleNameComponent)
                    },
                    new TableColumn
                    {
                        Title = L["UserCount"],
                        Data = nameof(IdentityRoleDto.UserCount)
                    }
            });

        RoleManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.Role));

        await base.SetTableColumnsAsync();
    }
    
    private bool IsStaticRole(string name)
    {
        return Entities.FirstOrDefault( x => x.Name == name)?.IsStatic ?? false;
    }

    public class DeleteRoleViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long UserCount { get; set; }

        public List<IdentityRoleDto> OtherRoles { get; set; } = new();

        public bool AssignRole { get; set; }

        public Guid? AssignToRoleId { get; set; } = Guid.Empty;
        
        public bool DisabledDeleteButton { get; set; } 
    }


    public class MoveAllUsersViewModel
    {
        public Guid CurrentRoleId { get; set; }

        public string CurrentRoleName { get; set; }

        public List<IdentityRoleDto> TargetRoles { get; set; }

        public Guid? TargetRoleId { get; set; }
    }

}
