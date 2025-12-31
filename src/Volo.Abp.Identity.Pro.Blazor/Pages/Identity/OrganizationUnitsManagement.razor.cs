using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

public partial class OrganizationUnitsManagement
{
    protected Modal DeleteModal;

    protected Modal MoveAllUsersModal;

    protected readonly IOrganizationUnitAppService OrganizationUnitAppService;

    protected List<BreadcrumbItem> BreadcrumbItems { get; } = new();

    protected bool HasManageOuPermission;

    protected bool HasManageUsersPermission;

    protected bool HasManageRolesPermission;

    protected int PageSize = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int TotalRolesCount;

    protected int TotalUsersCount;

    protected int SelectedOuUsersCurrentPage = 1;

    protected int SelectedOuRolesCurrentPage = 1;

    protected string OuDetailSelectedTab { get; set; } = "Members";

    protected OrganizationUnitTreeView SelectedOrganizationUnit;

    protected List<OrganizationUnitTreeView> OrganizationUnits;

    protected PagedAndSortedResultRequestDto SelectedOuRolesFilter = new();

    protected IReadOnlyList<IdentityRoleDto> SelectedOuRoles = Array.Empty<IdentityRoleDto>();

    protected GetIdentityUsersInput SelectedOuUsersFilter = new();

    protected IReadOnlyList<IdentityUserDto> SelectedOuUsers = Array.Empty<IdentityUserDto>();

    protected OrganizationUnitCreateDto CreateOuModel = new();

    protected Modal CreateOuModal;

    protected Validations CreateOuValidationsRef;

    protected OrganizationUnitUpdateDto EditOuModel = new();

    protected Modal EditOuModal;

    protected Validations EditOuValidationsRef;

    protected Modal AddMemberModal;

    protected Modal AddRoleModal;

    protected int TotalAssignableRolesCount;

    protected int TotalAssignableUsersCount;

    protected GetAvailableRolesInput OuAssignableRolesFilter = new();

    protected IReadOnlyList<IdentityRoleDto> OuAssignableRoles = Array.Empty<IdentityRoleDto>();

    protected List<IdentityRoleDto> SelectedAssignableOuRoles = new();

    protected GetAvailableUsersInput OuAssignableUsersFilter = new();

    protected IReadOnlyList<IdentityUserDto> OuAssignableUsers = Array.Empty<IdentityUserDto>();

    protected List<IdentityUserDto> SelectedAssignableOuUsers = new();

    protected Modal MoveOuModal;

    protected List<OrganizationUnitTreeView> MovableOuTree;

    protected OrganizationUnitTreeView MovingOu;

    protected OrganizationUnitTreeView NewParentOu;

    protected PageToolbar Toolbar { get; } = new();

    protected TableColumnDictionary TableColumns { get; } = new();
    protected EntityActionDictionary EntityActions { get; } = new();

    protected List<TableColumn> RoleManagementTableColumns => TableColumns.Get<RoleManagement>();
    protected List<TableColumn> UserManagementTableColumns => TableColumns.Get<UserManagement>();
    protected List<DataGridColumn<IdentityUserDto>> AddMemberTableColumns = new();
    protected List<DataGridColumn<IdentityRoleDto>> AddRoleTableColumns = new();
    protected Dictionary<Guid, Dropdown> Dropdowns { get; } = new();

    protected bool IsAddRoleModalPresended { get; set; }
    protected bool IsAddMemberModalPresended { get; set; }

    protected DeleteOuViewModel DeletingOu { get; set; }

    protected MoveAllUsersViewModel MoveAllUsersModel { get; set; }
    public OrganizationUnitsManagement(IOrganizationUnitAppService organizationUnitAppService)
    {
        OrganizationUnitAppService = organizationUnitAppService;
    }

    protected async override Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await SetEntityActionsAsync();
        await SetTableColumnsAsync();

        await GetOrganizationUnitsAsync();
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

    protected virtual async Task SetPermissionsAsync()
    {
        HasManageOuPermission =
            await AuthorizationService.IsGrantedAsync(IdentityPermissions.OrganizationUnits.ManageOU);
        HasManageUsersPermission =
            await AuthorizationService.IsGrantedAsync(IdentityPermissions.OrganizationUnits.ManageUsers);
        HasManageRolesPermission =
            await AuthorizationService.IsGrantedAsync(IdentityPermissions.OrganizationUnits.ManageRoles);
    }

    protected virtual ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.AddRange(new BreadcrumbItem[]
        {
                new(L["Menu:IdentityManagement"]),
                new(L["OrganizationUnits"])
        });

        return ValueTask.CompletedTask;
    }

    protected virtual async ValueTask SetEntityActionsAsync()
    {
        await SetRoleManagementEntityActionsAsync();
        await SetUserManagementEntityActionsAsync();
    }

    protected virtual async ValueTask SetTableColumnsAsync()
    {
        await SetRoleManagementTableColumnsAsync();
        await SetUserManagementTableColumnsAsync();
        await SetMemberAndRoleTableColumnsAsync();
    }

    protected virtual ValueTask SetUserManagementTableColumnsAsync()
    {
        UserManagementTableColumns
            .AddRange(new TableColumn[]
            {
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<UserManagement>()
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:UserName"],
                        Data = nameof(IdentityUserDto.UserName),
                        Sortable = true,
                    },
                    new TableColumn
                    {
                        Title = L["DisplayName:Email"],
                        Data = nameof(IdentityUserDto.Email),
                        Sortable = true,
                    },
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetUserManagementEntityActionsAsync()
    {
        EntityActions
            .Get<UserManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Visible = (data) => HasManageUsersPermission,
                        Clicked = async (data) => { await RemoveUserAsync(data.As<IdentityUserDto>()); },
                        Color = Color.Danger,
                        Icon = "fa-trash",
                        Text = L["Delete"]
                    }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetRoleManagementTableColumnsAsync()
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
                        Title = L["Name"],
                        Data = nameof(IdentityRoleDto.Name),
                        Sortable = true,
                    },
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetRoleManagementEntityActionsAsync()
    {
        EntityActions
            .Get<RoleManagement>()
            .AddRange(new EntityAction[]
            {
                    new EntityAction
                    {
                        Visible = (data) => HasManageRolesPermission,
                        Clicked = async (data) => { await RemoveRoleAsync(data.As<IdentityRoleDto>()); },
                        Color = Color.Danger,
                        Icon = "fa-trash",
                        Text = L["Delete"]
                    }
            });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetMemberAndRoleTableColumnsAsync()
    {
        AddMemberTableColumns.AddRange(new DataGridColumn<IdentityUserDto>[]
        {
            new DataGridColumn<IdentityUserDto>
            {
                Field = nameof(IdentityUserDto.UserName),
                Caption = L["DisplayName:UserName"].Value
            },
            new DataGridColumn<IdentityUserDto>
            {
                Field = nameof(IdentityUserDto.Email),
                Caption = L["DisplayName:Email"].Value
            }
        });

        AddRoleTableColumns.AddRange(new DataGridColumn<IdentityRoleDto>[]
        {
            new DataGridColumn<IdentityRoleDto>
            {
                Field = nameof(IdentityRoleDto.Name),
                Caption = L["DisplayName:Name"].Value
            }
        });

        return ValueTask.CompletedTask;
    }

    protected virtual ValueTask SetToolbarItemsAsync()
    {
        return ValueTask.CompletedTask;
    }

    protected virtual async Task GetOrganizationUnitsAsync()
    {
        var organizationUnitsDto = await OrganizationUnitAppService.GetListAllAsync();

        if (IsDisposed)
        {
            return;
        }

        var organizationUnits =
            ObjectMapper.Map<IReadOnlyList<OrganizationUnitWithDetailsDto>, List<OrganizationUnitTreeView>>(
                organizationUnitsDto.Items);

        var organizationUnitsDictionary = new Dictionary<Guid, List<OrganizationUnitTreeView>>();

        foreach (var organizationUnit in organizationUnits)
        {
            var parentId = organizationUnit.ParentId ?? Guid.Empty;

            if (!organizationUnitsDictionary.ContainsKey(parentId))
            {
                organizationUnitsDictionary.Add(parentId, new List<OrganizationUnitTreeView>());
            }

            organizationUnitsDictionary[parentId].Add(organizationUnit);
        }

        foreach (var organizationUnit in organizationUnits)
        {
            if (organizationUnitsDictionary.ContainsKey(organizationUnit.Id))
            {
                organizationUnit.Children = organizationUnitsDictionary[organizationUnit.Id];
            }
        }

        if (organizationUnitsDictionary.Any())
        {
            OrganizationUnits = organizationUnitsDictionary[Guid.Empty];
        }
        else
        {
            OrganizationUnits = new List<OrganizationUnitTreeView>();
        }
    }

    protected virtual async Task OnSelectedOUNodeChangedAsync([CanBeNull] OrganizationUnitTreeView node)
    {
        if (SelectedOrganizationUnit == node)
        {
            return;
        }

        SelectedOrganizationUnit = node;

        SelectedOuRolesFilter = new PagedAndSortedResultRequestDto();
        SelectedOuUsersFilter = new GetIdentityUsersInput();

        if (node != null)
        {
            foreach (var dropdown in Dropdowns)
            {
                if (dropdown.Key != SelectedOrganizationUnit.Id)
                {
                    await dropdown.Value.Hide();
                }
            }
            await GetSelectedOuRolesAsync();
            await GetSelectedOuUsersAsync();
        }
    }

    protected virtual async Task OnIdentityRoleDataGridReadAsync(DataGridReadDataEventArgs<IdentityRoleDto> e)
    {
        SelectedOuRolesFilter.Sorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");

        SelectedOuRolesCurrentPage = e.Page;
        SelectedOuRolesFilter.SkipCount = (SelectedOuRolesCurrentPage - 1) * PageSize;

        await GetSelectedOuRolesAsync();
    }

    protected virtual async Task GetSelectedOuRolesAsync()
    {
        var selectedOuRoles =
            await OrganizationUnitAppService.GetRolesAsync(SelectedOrganizationUnit.Id, SelectedOuRolesFilter);

        TotalRolesCount = (int)selectedOuRoles.TotalCount;
        SelectedOuRoles = selectedOuRoles.Items.ToList();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OnIdentityUserDataGridReadAsync(DataGridReadDataEventArgs<IdentityUserDto> e)
    {
        SelectedOuUsersFilter.Sorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");

        SelectedOuUsersCurrentPage = e.Page;
        SelectedOuUsersFilter.SkipCount = (SelectedOuUsersCurrentPage - 1) * PageSize;

        await GetSelectedOuUsersAsync();
    }

    protected virtual async Task GetSelectedOuUsersAsync()
    {
        var selectedOuUsers =
            await OrganizationUnitAppService.GetMembersAsync(SelectedOrganizationUnit.Id, SelectedOuUsersFilter);

        TotalUsersCount = (int)selectedOuUsers.TotalCount;
        SelectedOuUsers = selectedOuUsers.Items.ToList();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task SearchSelectedOuUsersAsync()
    {
        SelectedOuUsersCurrentPage = 1;
        await GetSelectedOuUsersAsync();
    }

    protected virtual async Task OpenCreateOuModalAsync([CanBeNull] OrganizationUnitTreeView node = null)
    {
        CreateOuValidationsRef?.ClearAll();

        await OnSelectedOUNodeChangedAsync(node);
        CreateOuModel = new OrganizationUnitCreateDto { ParentId = node?.Id };

        await CreateOuModal.Show();
    }

    protected virtual async Task CloseCreateOuModalAsync()
    {
        await CreateOuModal.Hide();
    }

    protected virtual Task ClosingCreateOuModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task CreateOuAsync()
    {
        try
        {
            if (!await CreateOuValidationsRef.ValidateAll())
            {
                return;
            }

            var createdOu = await OrganizationUnitAppService.CreateAsync(CreateOuModel);

            var createdOuTreeView =
                ObjectMapper.Map<OrganizationUnitWithDetailsDto, OrganizationUnitTreeView>(createdOu);

            if (createdOu.ParentId == null)
            {
                OrganizationUnits.Add(createdOuTreeView);
            }
            else
            {
                SelectedOrganizationUnit.Children ??= new List<OrganizationUnitTreeView>();

                SelectedOrganizationUnit.Children.Add(createdOuTreeView);

                SelectedOrganizationUnit.Collapsed = false;
            }

            await CloseCreateOuModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenEditOuModalAsync(OrganizationUnitTreeView node)
    {
        EditOuValidationsRef?.ClearAll();

        await OnSelectedOUNodeChangedAsync(node);

        var organizationUnit = await OrganizationUnitAppService.GetAsync(node.Id);

        EditOuModel = ObjectMapper.Map<OrganizationUnitWithDetailsDto, OrganizationUnitUpdateDto>(organizationUnit);

        await EditOuModal.Show();
    }

    protected virtual async Task CloseEditOuModalAsync()
    {
        await EditOuModal.Hide();
    }

    protected virtual Task ClosingEditOuModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task UpdateOrganizationUnitAsync()
    {
        try
        {
            if (!await EditOuValidationsRef.ValidateAll())
            {
                return;
            }

            var updatedOu = ObjectMapper.Map<OrganizationUnitWithDetailsDto, OrganizationUnitTreeView>(
                await OrganizationUnitAppService.UpdateAsync(SelectedOrganizationUnit.Id, EditOuModel));

            updatedOu.Collapsed = false;
            updatedOu.Children = SelectedOrganizationUnit.Children;
            SelectedOrganizationUnit = updatedOu;

            if (updatedOu.ParentId == null)
            {
                OrganizationUnits.ReplaceOne(x => x.Id == updatedOu.Id, updatedOu);
            }
            else
            {
                var parentNode = GetParentNode(OrganizationUnits, updatedOu);
                parentNode.Children.ReplaceOne(x => x.Id == updatedOu.Id, updatedOu);
            }

            await CloseEditOuModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual OrganizationUnitTreeView GetParentNode(List<OrganizationUnitTreeView> collection,
        OrganizationUnitTreeView childNode)
    {
        var parentOu = collection.FirstOrDefault(x => x.Id == childNode.ParentId);

        if (parentOu != null)
        {
            return parentOu;
        }

        foreach (var organizationUnitTreeView in collection)
        {
            if (organizationUnitTreeView.Children != null && organizationUnitTreeView.Children.Any())
            {
                parentOu = GetParentNode(organizationUnitTreeView.Children, childNode);

                if (parentOu != null)
                {
                    return parentOu;
                }
            }
        }

        return null;
    }

    protected virtual async Task RemoveRoleAsync(IdentityRoleDto roleDto)
    {
        if (await Message.Confirm(L["RemoveRoleFromOuWarningMessage", roleDto.Name,
            SelectedOrganizationUnit.DisplayName]))
        {
            try
            {
                await OrganizationUnitAppService.RemoveRoleAsync(SelectedOrganizationUnit.Id, roleDto.Id);
                await GetSelectedOuRolesAsync();
                await Notify.Success(L["DeletedSuccessfully"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
    }

    protected virtual async Task RemoveUserAsync(IdentityUserDto userDto)
    {
        if (await Message.Confirm(L["RemoveUserFromOuWarningMessage", userDto.UserName,
            SelectedOrganizationUnit.DisplayName]))
        {
            try
            {
                await OrganizationUnitAppService.RemoveMemberAsync(SelectedOrganizationUnit.Id, userDto.Id);
                await GetSelectedOuUsersAsync();
                await Notify.Success(L["DeletedSuccessfully"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
    }

    protected virtual async Task OpenAddMemberModalAsync()
    {
        OuAssignableUsersFilter = new GetAvailableUsersInput
        {
            Id = SelectedOrganizationUnit.Id
        };

        await GetOuAssignableUsersAsync();

        await InvokeAsync(StateHasChanged);

        await AddMemberModal.Show();
        IsAddMemberModalPresended = true;
    }

    protected virtual async Task CloseAddMemberModalAsync()
    {
        SelectedAssignableOuUsers.Clear();
        await AddMemberModal.Hide();
        IsAddMemberModalPresended = false;
    }

    protected virtual Task ClosingAddMemberModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task OnAssignableUserDataGridReadAsync(DataGridReadDataEventArgs<IdentityUserDto> e)
    {
        if (!IsAddMemberModalPresended)
        {
            return;
        }

        OuAssignableUsersFilter.Sorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        OuAssignableUsersFilter.SkipCount = (e.Page - 1) * PageSize;

        await GetOuAssignableUsersAsync();
    }

    protected virtual async Task GetOuAssignableUsersAsync()
    {
        var assignableOuUsers = await OrganizationUnitAppService.GetAvailableUsersAsync(OuAssignableUsersFilter);

        TotalAssignableUsersCount = (int)assignableOuUsers.TotalCount;
        OuAssignableUsers = assignableOuUsers.Items.ToList();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task AddSelectedMembersToOuAsync()
    {
        try
        {
            var selectedUsersIds = SelectedAssignableOuUsers.Select(x => x.Id).ToList();

            var input = new OrganizationUnitUserInput
            {
                UserIds = selectedUsersIds
            };

            await OrganizationUnitAppService.AddMembersAsync(SelectedOrganizationUnit.Id, input);

            await GetSelectedOuUsersAsync();

            await InvokeAsync(StateHasChanged);

            await CloseAddMemberModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenAddRoleModalAsync()
    {
        OuAssignableRolesFilter = new GetAvailableRolesInput
        {
            Id = SelectedOrganizationUnit.Id
        };

        await GetOuAssignableRolesAsync();

        await InvokeAsync(StateHasChanged);

        await AddRoleModal.Show();
        IsAddRoleModalPresended = true;
    }

    protected virtual async Task CloseAddRoleModalAsync()
    {
        SelectedAssignableOuRoles.Clear();
        await AddRoleModal.Hide();
        IsAddRoleModalPresended = false;
    }

    protected virtual Task ClosingAddRoleModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task OnAssignableRoleDataGridReadAsync(DataGridReadDataEventArgs<IdentityRoleDto> e)
    {
        if (!IsAddRoleModalPresended)
        {
            return;
        }

        OuAssignableRolesFilter.Sorting = e.Columns
            .Where(c => c.SortDirection != SortDirection.Default)
            .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
            .JoinAsString(",");
        OuAssignableRolesFilter.SkipCount = (e.Page - 1) * PageSize;

        await GetOuAssignableRolesAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task GetOuAssignableRolesAsync()
    {
        var assignableOuRoles = await OrganizationUnitAppService.GetAvailableRolesAsync(OuAssignableRolesFilter);

        TotalAssignableRolesCount = (int)assignableOuRoles.TotalCount;
        OuAssignableRoles = assignableOuRoles.Items.ToList();
    }

    protected virtual async Task AddSelectedRolesToOuAsync()
    {
        try
        {
            var selectedRolesIds = SelectedAssignableOuRoles.Select(x => x.Id).ToList();

            var input = new OrganizationUnitRoleInput
            {
                RoleIds = selectedRolesIds
            };

            await OrganizationUnitAppService.AddRolesAsync(SelectedOrganizationUnit.Id, input);

            await GetSelectedOuRolesAsync();

            await InvokeAsync(StateHasChanged);

            await CloseAddRoleModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenMoveOuModal(OrganizationUnitTreeView node)
    {
        MovingOu = node;

        var organizationUnitsDto = await OrganizationUnitAppService.GetListAllAsync();

        var organizationUnits =
            ObjectMapper.Map<IReadOnlyList<OrganizationUnitWithDetailsDto>, List<OrganizationUnitTreeView>>(
                organizationUnitsDto.Items);

        var organizationUnitsDictionary = new Dictionary<Guid, List<OrganizationUnitTreeView>>();

        foreach (var organizationUnit in organizationUnits)
        {
            if (organizationUnit.Code.StartsWith(MovingOu.Code))
            {
                continue;
            }

            var parentId = organizationUnit.ParentId ?? Guid.Empty;

            if (!organizationUnitsDictionary.ContainsKey(parentId))
            {
                organizationUnitsDictionary.Add(parentId, new List<OrganizationUnitTreeView>());
            }

            organizationUnitsDictionary[parentId].Add(organizationUnit);
        }

        foreach (var organizationUnit in organizationUnits)
        {
            if (organizationUnitsDictionary.ContainsKey(organizationUnit.Id))
            {
                organizationUnit.Children = organizationUnitsDictionary[organizationUnit.Id];
            }
        }

        if (organizationUnitsDictionary.TryGetValue(Guid.Empty, out var rootOrganizationUnits))
        {
            var rootNode = new OrganizationUnitTreeView
            {
                Id = Guid.Empty,
                Children = rootOrganizationUnits,
                Collapsed = false,
                DisplayName = L["OrganizationUnit:Root"]
            };

            NewParentOu = rootNode;
            MovableOuTree = new List<OrganizationUnitTreeView> { rootNode };
                
            await MoveOuModal.Show();
        }
        else
        {
            await Notify.Error(L["NoOrganizationUnits"]);
        }
    }

    protected virtual Task MovableOuSelected(OrganizationUnitTreeView node)
    {
        NewParentOu = node;
        return Task.CompletedTask;
    }

    protected virtual async Task CloseMoveOuModal()
    {
        MovableOuTree = null;
        MovingOu = null;
        NewParentOu = null;

        await MoveOuModal.Hide();
    }

    protected virtual Task ClosingMoveOuModal(ModalClosingEventArgs eventArgs)
    {
        eventArgs.Cancel = eventArgs.CloseReason == CloseReason.FocusLostClosing;
        return Task.CompletedTask;
    }

    protected virtual async Task MoveOrganizationUnitAsync()
    {
        try
        {
            Guid? newParentId = NewParentOu.Id == Guid.Empty ? null : NewParentOu.Id;

            if (MovingOu.ParentId == newParentId)
            {
                Message.Info(L["OrganizationUnitMoveSameParentMessage"]);
                return;
            }

            if (!await Message.Confirm(L["OrganizationUnitMoveConfirmMessage", MovingOu.DisplayName,
                NewParentOu.DisplayName]))
            {
                return;
            }

            var input = new OrganizationUnitMoveInput { NewParentId = newParentId };

            await OrganizationUnitAppService.MoveAsync(MovingOu.Id, input);

            await CloseMoveOuModal();
            await GetOrganizationUnitsAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task OpenDeleteModalAsync(OrganizationUnitTreeView data)
    {
        var organizationUnit = await OrganizationUnitAppService.GetAsync(data.Id);
        var allOrganizationUnits = await OrganizationUnitAppService.GetListAllAsync();
        DeletingOu = new DeleteOuViewModel()
        {
            Id = organizationUnit.Id,
            DisplayName = organizationUnit.DisplayName,
            UserCount = organizationUnit.UserCount,
            OtherOrganizationUnits = allOrganizationUnits.Items.Where(x => x.Id != organizationUnit.Id).ToList()
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

    protected virtual async Task DeleteOrganizationUnitAsync()
    {
        try
        {
            if (DeletingOu.AssignToOrganizationUnitId == Guid.Empty)
            {
                DeletingOu.AssignToOrganizationUnitId = null;
            }
            await OrganizationUnitAppService.MoveAllUsersAsync(DeletingOu.Id, DeletingOu.AssignToOrganizationUnitId);
            await OrganizationUnitAppService.DeleteAsync(DeletingOu.Id);
            await CloseDeleteModalAsync();
            await Notify.Success(L["DeletedSuccessfully"]);
            await OnSelectedOUNodeChangedAsync(null);
            await GetOrganizationUnitsAsync();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual void OnAssignCheckedChanged()
    {
        DeletingOu.AssignOrganizationUnit = !DeletingOu.AssignOrganizationUnit;
        if (!DeletingOu.AssignOrganizationUnit)
        {
            DeletingOu.AssignToOrganizationUnitId = Guid.Empty;
            DeletingOu.DisabledDeleteButton = false;
        }
        else
        {
            DeletingOu.DisabledDeleteButton = true;
        }
    }

    protected virtual void OnAssignToOrganizationUnitSelectedValueChanged(Guid? id)
    {
        DeletingOu.AssignToOrganizationUnitId = id;

        DeletingOu.DisabledDeleteButton = DeletingOu.AssignToOrganizationUnitId == Guid.Empty;
    }

    protected virtual async Task OpenMoveAllUsersModalAsync(OrganizationUnitTreeView organizationUnitTreeView)
    {
        var organizationUnit = await OrganizationUnitAppService.GetAsync(organizationUnitTreeView.Id);
        if (organizationUnit.UserCount == 0)
        {
            await Message.Warn(L["ThereIsNoUsersCurrentlyInThisOrganizationUnit"]);
            return;
        }
        var allOrganizationUnits = await OrganizationUnitAppService.GetListAllAsync();
        MoveAllUsersModel = new MoveAllUsersViewModel()
        {
            Id = organizationUnit.Id,
            Name = organizationUnit.DisplayName,
            OtherOrganizationUnits = allOrganizationUnits.Items.Where(x => x.Id != organizationUnit.Id).ToList(),
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
            if (MoveAllUsersModel.TargetOrganizationUnitId == Guid.Empty)
            {
                MoveAllUsersModel.TargetOrganizationUnitId = null;
            }
            await OrganizationUnitAppService.MoveAllUsersAsync(MoveAllUsersModel.Id, MoveAllUsersModel.TargetOrganizationUnitId);
            await GetSelectedOuUsersAsync();
            await CloseMoveAllUsersModalAsync();
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }

    }

    public class DeleteOuViewModel
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public long UserCount { get; set; }

        public List<OrganizationUnitWithDetailsDto> OtherOrganizationUnits { get; set; } = new();

        public bool AssignOrganizationUnit { get; set; }

        public Guid? AssignToOrganizationUnitId { get; set; } = Guid.Empty;
        
        public bool DisabledDeleteButton { get; set; } 
    }

    public class MoveAllUsersViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<OrganizationUnitWithDetailsDto> OtherOrganizationUnits { get; set; } = new();


        public Guid? TargetOrganizationUnitId { get; set; }
    }
}
