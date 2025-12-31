using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;
using Volo.Abp.Identity.Settings;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement.Blazor.Components;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

public partial class UserManagement: IDisposable
{
    [Inject]
    protected ISettingProvider SettingProvider { get; set; }

    [Inject]
    protected IPermissionChecker PermissionChecker { get; set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected IOptions<AbpIdentityProBlazorOptions> Options { get; set; }

    [Inject]
    protected IIdentityRoleAppService RoleAppService { get; set; }

    [Inject]
    protected UserManagementState UserManagementState { get; set; }


    protected const string PermissionProviderName = "U";

    protected const string DefaultSelectedTab = "UserInformations";

    protected PermissionManagementModal PermissionManagementModal;

    protected IReadOnlyList<IdentityRoleDto> Roles;

    protected AssignedRoleViewModel[] NewUserRoles;

    protected AssignedRoleViewModel[] EditUserRoles;

    protected string CreateModalSelectedTab = DefaultSelectedTab;

    protected string EditModalSelectedTab = DefaultSelectedTab;

    protected string ManagePermissionsPolicyName;

    protected bool HasImpersonationPermission;

    protected string ImpersonationPolicyName;

    protected Modal LockModal;

    protected UserLockViewModel LockingUser { get; set; }

    protected Modal ChangePasswordModal;

    protected bool RandomPasswordGenerated { get; set; }

    protected ChangeUserPasswordViewModel ChangePasswordModel { get; set; }

    protected Modal TwoFactorModal;

    protected Modal SessionsModal;

    protected Modal SessionDetailModal;

    protected IdentitySessionDto SessionDetail;

    protected Modal ClaimsModal;

    protected IdentityUserViewDetailsModal ViewDetailsModal;

    protected TwoFactorViewModel TwoFactorModel { get; set; }

    protected ClaimsViewModel ClaimsModel { get; set; }

    protected string SelectedClaimType { get; set; }

    protected string SelectedClaimValueText { get; set; }

    protected int SelectedClaimValueNumeric { get; set; }

    protected DateTime SelectedClaimValueDate { get; set; }

    protected bool SelectedClaimValueBool { get; set; }

    [Required]
    protected IdentityClaimValueType? SelectedClaimValueType { get; set; }

    private string _filter = null;

    protected string Filter {
        get => string.IsNullOrWhiteSpace(_filter) ? "" : _filter;
        set => _filter = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
    }

    protected bool HasManagePermissionsPermission { get; set; }

    protected List<OrganizationUnitTreeView> OrganizationUnits;

    protected OrganizationUnitTreeView SelectedOrganizationUnitNode { get => null; set { } }

    protected Dictionary<Guid, bool> SelectedOrganizationUnits;

    protected List<TableColumn> UserManagementTableColumns => TableColumns.Get<UserManagement>();
    protected List<TableColumn> UserManagementSessionsTableColumns = new List<TableColumn>();
    protected PageToolbar Toolbar { get; } = new();
    protected TextRole PasswordTextRole = TextRole.Password;
    protected bool RequireConfirmedEmail;

    private bool showPassword;
    protected bool ShowPassword {
        get => showPassword;
        set {
            showPassword = value;
            PasswordTextRole = showPassword ? TextRole.Text : TextRole.Password;
        }
    }

    protected bool ShowAdvancedFilters { get; set; }

    protected string ViewDetailsPolicyName;

    protected bool HasViewDetailsPermission { get; set; }

    protected AdvancedFilterInput AdvancedFilterInput { get; set; } = new();

    public bool IsEditCurrentUser { get; set; }

    public UserManagement()
    {
        LockingUser = new UserLockViewModel();
        ChangePasswordModel = new ChangeUserPasswordViewModel();
        TwoFactorModel = new TwoFactorViewModel();
        ClaimsModel = new ClaimsViewModel();
        NewUserRoles = Array.Empty<AssignedRoleViewModel>();
        EditUserRoles = Array.Empty<AssignedRoleViewModel>();
        SelectedOrganizationUnits = new Dictionary<Guid, bool>();

        ObjectMapperContext = typeof(AbpIdentityProBlazorModule);
        LocalizationResource = typeof(IdentityResource);

        CreatePolicyName = IdentityPermissions.Users.Create;
        UpdatePolicyName = IdentityPermissions.Users.Update;
        DeletePolicyName = IdentityPermissions.Users.Delete;
        ManagePermissionsPolicyName = IdentityPermissions.Users.ManagePermissions;
        ImpersonationPolicyName = IdentityPermissions.Users.Impersonation;
        ViewDetailsPolicyName = IdentityPermissions.Users.ViewDetails;
    }

    protected async override Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Roles = (await AppService.GetAssignableRolesAsync()).Items;
        RequireConfirmedEmail = await SettingProvider.IsTrueAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail);

        UserManagementState.OnDataGridChanged += OnDataGridChangedAsync;
        UserManagementState.OnGetFilter += GetFilter;

        UserManagementSessionsTableColumns
            .AddRange(new TableColumn[] {
                new TableColumn {
                    Title = L["Actions"], Actions = new List<EntityAction>()
                    {
                        new EntityAction {
                            Text = L["Session:Detail"],
                            Clicked = async (data) =>
                            {
                                SessionDetail = data.As<IdentitySessionDto>();
                                await SessionDetailModal.Show();
                            }
                        },
                        new EntityAction {
                            Text = L["Session:Revoke"],
                            Clicked = async (data) =>
                            {
                                if (await Message.Confirm(L["SessionRevokeConfirmationMessage"]))
                                {
                                    await IdentitySessionAppService.RevokeAsync(data.As<IdentitySessionDto>().Id);

                                    if (data.As<IdentitySessionDto>().IsCurrent)
                                    {
                                        NavigationManager.NavigateTo("/", true);
                                        return;
                                    }

                                    await GetSessionsAsync();
                                }
                            }
                        },
                    }
                },
                new TableColumn {Title = L["Device"], Data = nameof(IdentitySessionDto.Device), Component = typeof(IdentitySessionDeviceComponent)},
                new TableColumn {Title = L["DeviceInfo"], Data = nameof(IdentitySessionDto.DeviceInfo)},
                new TableColumn {Title = L["SignedIn"], Data = nameof(IdentitySessionDto.SignedIn)},
                new TableColumn {Title = L["LastAccessed"], Data = nameof(IdentitySessionDto.LastAccessed)}
            });
    }

    protected virtual async Task OnAdvancedFilterSectionClick()
    {
        ShowAdvancedFilters = !ShowAdvancedFilters;

        if (ShowAdvancedFilters)
        {
            await GetOrganizationUnitsAsync();
        }
    }

    protected int CurrentSessionPage { get; set; }
    protected Guid CurrentSessionUserId { get; set; }

    protected string CurrentSessionUserName { get; set; }
    protected virtual int SessionPageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
    protected int? SessionTotalCount;
    protected IReadOnlyList<IdentitySessionDto> SessionsEntities { get; set; } = new List<IdentitySessionDto>();
    protected virtual async Task OnSessionsDataGridReadAsync(DataGridReadDataEventArgs<IdentitySessionDto> e)
    {
        CurrentSessionPage = e.Page == 0 ? 1 : e.Page;
        await GetSessionsAsync();
    }

    protected virtual async Task GetSessionsAsync()
    {
        var sessions = await IdentitySessionAppService.GetListAsync(new GetIdentitySessionListInput
        {
            UserId = CurrentSessionUserId,
            SkipCount = (CurrentSessionPage - 1) * SessionPageSize,
            MaxResultCount = SessionPageSize
        });

        SessionsEntities = sessions.Items.ToList();
        SessionTotalCount = Convert.ToInt32(sessions.TotalCount);

        await InvokeAsync(StateHasChanged);
    }

    protected async override Task SetPermissionsAsync()
    {
        await base.SetPermissionsAsync();

        HasManagePermissionsPermission = await AuthorizationService.IsGrantedAsync(ManagePermissionsPolicyName);
        HasImpersonationPermission = await AuthorizationService.IsGrantedAsync(ImpersonationPolicyName);
        HasViewDetailsPermission = await AuthorizationService.IsGrantedAsync(ViewDetailsPolicyName);
    }

    protected async override Task OpenCreateModalAsync()
    {
        CreateModalSelectedTab = DefaultSelectedTab;

        NewUserRoles = Roles.Select(x => new AssignedRoleViewModel { Name = x.Name, IsAssigned = x.IsDefault }).ToArray();

        await GetOrganizationUnitsAsync();
        ShowPassword = false;
        await base.OpenCreateModalAsync();

        NewEntity.LockoutEnabled = true;
        NewEntity.IsActive = true;
        ShowPassword = false;
        CreateValidationsRef?.ClearAll();
    }

    protected virtual async Task GetOrganizationUnitsAsync([CanBeNull] ICollection<Guid> selectedOuIds = null)
    {
        selectedOuIds ??= new List<Guid>();

        var organizationUnitsDto = await AppService.GetAvailableOrganizationUnitsAsync();

        var organizationUnits =
            ObjectMapper.Map<IReadOnlyList<OrganizationUnitWithDetailsDto>, List<OrganizationUnitTreeView>>(
                organizationUnitsDto.Items);

        var organizationUnitsDictionary = new Dictionary<Guid, List<OrganizationUnitTreeView>>();

        SelectedOrganizationUnits = new Dictionary<Guid, bool>();

        foreach (var organizationUnit in organizationUnits)
        {
            organizationUnit.Collapsed = false;

            SelectedOrganizationUnits.Add(organizationUnit.Id, selectedOuIds.Contains(organizationUnit.Id));

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

    protected override Task CreateEntityAsync()
    {
        NewEntity.RoleNames = NewUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();
        NewEntity.OrganizationUnitIds = SelectedOrganizationUnits.Where(x => x.Value).Select(x => x.Key).ToArray();

        return base.CreateEntityAsync();
    }

    protected async override Task OpenEditModalAsync(IdentityUserDto entity)
    {
        EditModalSelectedTab = DefaultSelectedTab;
        var organizationUnits = await AppService.GetOrganizationUnitsAsync(entity.Id);
        IsEditCurrentUser = entity.Id == CurrentUser.Id;

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles))
        {
            var userRoleNames = (await AppService.GetRolesAsync(entity.Id)).Items.Select(r => r.Name).ToList();

            EditUserRoles = Roles
                .Select(x => new AssignedRoleViewModel
                {
                    Name = x.Name,
                    IsAssigned = userRoleNames.Contains(x.Name),
                    IsInheritedFromOu = organizationUnits.Any(ou => ou.Roles.Any(role => role.RoleId == x.Id))
                })
                .ToArray();
        }

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageOU))
        {
            var assignedOuIds = organizationUnits.Select(x => x.Id).ToList();
            await GetOrganizationUnitsAsync(assignedOuIds);
        }
        else
        {
            OrganizationUnits = new List<OrganizationUnitTreeView>();
        }

        ShowPassword = false;

        await base.OpenEditModalAsync(entity);
    }

    protected async Task CloseLockModalAsync()
    {
        LockingUser = new UserLockViewModel();
        await LockModal.Close(CloseReason.None);
    }

    protected async Task CloseChangePasswordModalAsync()
    {
        ChangePasswordModel = new ChangeUserPasswordViewModel();
        await ChangePasswordModal.Close(CloseReason.None);
    }

    protected async Task CloseTwoFactorModalAsync()
    {
        TwoFactorModel = new TwoFactorViewModel();
        await TwoFactorModal.Close(CloseReason.None);
    }

    protected async Task CloseSessionsModalAsync()
    {
        SessionsEntities = new List<IdentitySessionDto>();
        SessionTotalCount = 0;
        await SessionsModal.Close(CloseReason.None);
    }

    protected async Task CloseSessionDetailModalAsync()
    {
        await SessionDetailModal.Close(CloseReason.None);
    }

    protected async Task CloseClaimsModalAsync()
    {
        ClaimsModel = new ClaimsViewModel();
        SelectedClaimValueText = null;
        await ClaimsModal.Close(CloseReason.None);
    }

    protected async Task LockUserAsync()
    {
        try
        {
            await identityUserAppService.LockAsync(LockingUser.Id, LockingUser.LockoutEnd);

            await CloseLockModalAsync();
            await GetEntitiesAsync();
            await InvokeAsync(StateHasChanged);
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await Message.Error(e.Message);
        }
    }

    protected async Task ChangeTwoFactorAsync()
    {
        try
        {
            await identityUserAppService.SetTwoFactorEnabledAsync(TwoFactorModel.Id, TwoFactorModel.TwoFactorEnabled);

            await CloseTwoFactorModalAsync();
            await GetEntitiesAsync();
            await InvokeAsync(StateHasChanged);
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await Message.Error(e.Message);
        }
    }

    protected async Task ChangePasswordAsync()
    {
        try
        {
            await identityUserAppService.UpdatePasswordAsync(ChangePasswordModel.Id,
                new IdentityUserUpdatePasswordInput { NewPassword = ChangePasswordModel.NewPassword });
            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception e)
        {
            await Message.Error(e.Message);
            return;
        }

        await CloseChangePasswordModalAsync();
    }

    protected async Task GenerateRandomPassword()
    {
        var rnd = new Random();
        var specials = "!*_#/+-.";
        var lowercase = "abcdefghjkmnpqrstuvwxyz";
        var uppercase = lowercase.ToUpper();
        var numbers = "23456789";

        var all = specials + lowercase + uppercase + numbers;

        var password =
            lowercase.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(4, 6)).JoinAsString("") +
            specials.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(1, 2)).JoinAsString("") +
            uppercase.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(2, 3)).JoinAsString("") +
            numbers.ToArray().OrderBy(x => rnd.Next()).Take(rnd.Next(1, 2)).JoinAsString("");

        var requiredLength = 8;
        var requiredLengthSetting = await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequiredLength);
        if (requiredLengthSetting != null && int.TryParse(requiredLengthSetting, out var requiredLengthParsed))
        {
            requiredLength = requiredLengthParsed;
        }

        if (password.Length < requiredLength)
        {
            password += all.ToArray().OrderBy(x => rnd.Next()).Take(requiredLength - password.Length).JoinAsString("");
        }

        var requiredUniqueChars = 1;
        var requiredUniqueCharsSetting = await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequiredUniqueChars);
        if (requiredUniqueCharsSetting != null && int.TryParse(requiredUniqueCharsSetting, out var requiredUniqueCharsParsed))
        {
            requiredUniqueChars = requiredUniqueCharsParsed;
        }

        if (password.Distinct().Count() < requiredUniqueChars)
        {
            var uniqueChars = all.ToArray().OrderBy(x => rnd.Next()).Take(requiredUniqueChars - password.Distinct().Count()).JoinAsString("");
            password += uniqueChars;
        }

        ChangePasswordModel.NewPassword = password.ToArray().OrderBy(x => rnd.Next()).JoinAsString("");
        RandomPasswordGenerated = true;
        ShowPassword = true;
    }

    protected virtual async Task AddClaimAsync()
    {
        var claim = ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == SelectedClaimType);
        if (claim == null)
        {
            return;
        }

        if (claim.Required && (claim.ValueType == IdentityClaimValueType.String && string.IsNullOrWhiteSpace(SelectedClaimValueText)))
        {
            await Message.Info(L["ClaimValueCanNotBeBlank"]);
            return;
        }

        if(!SelectedClaimValueText.IsNullOrWhiteSpace() && !claim.Regex.IsNullOrWhiteSpace() && !Regex.IsMatch(SelectedClaimValueText, claim.Regex))
        {
            await Message.Info(L["ClaimValueIsInvalid", claim.Name]);
            return;
        }

        ClaimsModel.OwnedClaims.Add(new IdentityUserClaimViewModel
        {
            ClaimType = SelectedClaimType,
            ClaimValueText = SelectedClaimValueText,
            ClaimValueNumeric = SelectedClaimValueNumeric,
            ClaimValueDate = SelectedClaimValueDate,
            ClaimValueBool = SelectedClaimValueBool
        });
    }

    protected IdentityClaimValueType GetClaimValueType(string claimType)
    {
        return ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == claimType)?.ValueType ?? IdentityClaimValueType.String;
    }

    protected string GetClaimRegex(string claimType)
    {
        return ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == SelectedClaimType)?.Regex ?? string.Empty;
    }

    protected void RemoveClaim(IdentityUserClaimViewModel claim)
    {
        ClaimsModel.OwnedClaims.Remove(claim);
    }

    protected async Task SaveClaimsAsync()
    {
        var ownedClaimsToPost = new List<IdentityUserClaimDto>();

        foreach (var ownedClaim in ClaimsModel.OwnedClaims)
        {
            string value;
            var claim = ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == ownedClaim.ClaimType);

            if (claim is null)
            {
                continue;
            }

            switch (claim.ValueType)
            {
                case IdentityClaimValueType.String:
                    value = ownedClaim.ClaimValueText;
                    break;
                case IdentityClaimValueType.Int:
                    value = ownedClaim.ClaimValueNumeric.ToString();
                    break;
                case IdentityClaimValueType.Boolean:
                    value = ownedClaim.ClaimValueBool.ToString();
                    break;
                case IdentityClaimValueType.DateTime:
                    value = ownedClaim.ClaimValueDate.ToString(CultureInfo.InvariantCulture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (value.IsNullOrWhiteSpace())
            {
                continue;
            }

            if (!claim.Regex.IsNullOrWhiteSpace() && !Regex.IsMatch(value, claim.Regex))
            {
                await Message.Info(L["ClaimValueIsInvalid", claim.Name]);
                return;
            }

            ownedClaimsToPost.Add(
                new IdentityUserClaimDto
                {
                    UserId = ClaimsModel.Id,
                    ClaimType = ownedClaim.ClaimType,
                    ClaimValue = value
                }
            );
        }

        await identityUserAppService.UpdateClaimsAsync(ClaimsModel.Id, ownedClaimsToPost);

        await Notify.Success(L["SavedSuccessfully"]);
        await CloseClaimsModalAsync();
    }

    protected Task SetSelectedClaimValueType(string value)
    {
        SelectedClaimType = value;

        var claim = ClaimsModel.AllClaims.FirstOrDefault(c => c.Name == SelectedClaimType);

        SelectedClaimValueType = claim?.ValueType;

        return Task.CompletedTask;
    }

    protected override Task UpdateEntityAsync()
    {
        if (EditUserRoles != null)
        {

            EditingEntity.RoleNames = EditUserRoles.Where(x => x.IsAssigned).Select(x => x.Name).ToArray();
        }

        if (SelectedOrganizationUnits != null)
        {
            EditingEntity.OrganizationUnitIds = SelectedOrganizationUnits.Where(x => x.Value).Select(x => x.Key).ToArray();
        }

        return base.UpdateEntityAsync();
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Menu:IdentityManagement"]));
        BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem(L["Users"]));
        return ValueTask.CompletedTask;
    }

    protected override string GetDeleteConfirmationMessage(IdentityUserDto entity)
    {
        return string.Format(L["UserDeletionConfirmationMessage"], entity.UserName);
    }

    protected override async ValueTask SetTableColumnsAsync()
    {
        UserManagementTableColumns
            .AddRange(new TableColumn[] {
                new TableColumn {Title = L["Actions"], Actions = EntityActions.Get<UserManagement>()},
                new TableColumn {Title = L["UserName"], Data = nameof(IdentityUserDto.UserName),Sortable = true, Component = typeof(IdentityUserUserNameComponent) },
                new TableColumn {Title = L["Email"], Data = nameof(IdentityUserDto.Email), Sortable = true,}, new TableColumn {
                    Title = L["Roles"],
                    Data = nameof(IdentityUserDto.RoleNames),
                    ValueConverter = (data) =>
                    {
                        var roleNames = (data as IdentityUserDto)?.RoleNames;
                        return roleNames == null ? string.Empty : string.Join(", ", roleNames);
                    }
                },
                new TableColumn {Title = L["PhoneNumber"], Data = nameof(IdentityUserDto.PhoneNumber), Sortable = true,},
                new TableColumn {Title = L["Name"], Data = nameof(IdentityUserDto.Name), Sortable = true,},
                new TableColumn {Title = L["Surname"], Data = nameof(IdentityUserDto.Surname), Sortable = true,},
                new TableColumn {
                    Title = L["DisplayName:IsActive"],
                    Data = nameof(IdentityUserDto.IsActive),
                    Sortable = true,
                    Component = typeof(IdentityUserIsActiveColumnComponent),
                },
                new TableColumn {
                    Title = L["DisplayName:LockoutEnabled"],
                    Data = nameof(IdentityUserDto.LockoutEnabled),
                    Sortable = true,
                    Component = typeof(IdentityUserLockoutEnabledColumnComponent),
                },
                new TableColumn {
                    Title = L["DisplayName:EmailConfirmed"],
                    Data = nameof(IdentityUserDto.EmailConfirmed),
                    Sortable = true,
                    Component = typeof(IdentityUserEmailConfirmedColumnComponent),
                },
                new TableColumn {
                    Title = L["DisplayName:TwoFactorEnabled"],
                    Data = nameof(IdentityUserDto.TwoFactorEnabled),
                    Sortable = true,
                    Component = typeof(IdentityUserTwoFactorEnabledColumnComponent),
                },
                new TableColumn {
                    Title = L["DisplayName:AccessFailedCount"],
                    Data = nameof(IdentityUserDto.AccessFailedCount),
                    Sortable = true,
                },
                new TableColumn {
                    Title = L["CreationTime"],
                    Data = nameof(IdentityUserDto.CreationTime),
                    Sortable = true,},
                new TableColumn {
                    Title = L["LastModificationTime"], Data = nameof(IdentityUserDto.LastModificationTime), Sortable = true,
                },
            });

        UserManagementTableColumns.AddRange(await GetExtensionTableColumnsAsync(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.User));

        await base.SetTableColumnsAsync();
    }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<UserManagement>()
            .AddRange(new EntityAction[] {
                new EntityAction {
                    Text = L["ViewDetails"],
                    Visible = (data) => HasViewDetailsPermission,
                    Clicked = async (data) => { await ViewDetailsModal.OpenAsync(data.As<IdentityUserDto>().Id); }
                },
                new EntityAction {
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) => { await OpenEditModalAsync(data.As<IdentityUserDto>()); }
                },
                new EntityAction {
                    Text = L["Claims"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        var allClaims = await identityUserAppService.GetAllClaimTypesAsync();
                        ClaimsModel = new ClaimsViewModel {
                            Id = data.As<IdentityUserDto>().Id,
                            UserName = data.As<IdentityUserDto>().UserName,
                            AllClaims = allClaims,
                            OwnedClaims = (await identityUserAppService.GetClaimsAsync(data.As<IdentityUserDto>().Id))
                                .Select(c =>
                                    new IdentityUserClaimViewModel {
                                        ClaimType = c.ClaimType,
                                        ClaimValueText = c.ClaimValue,
                                        ClaimValueNumeric =
                                            allClaims.FirstOrDefault(ac => ac.Name == c.ClaimType)?.ValueType ==
                                            IdentityClaimValueType.Int
                                                ? Convert.ToInt32(c.ClaimValue)
                                                : 0,
                                        ClaimValueDate =
                                            allClaims.FirstOrDefault(ac => ac.Name == c.ClaimType)?.ValueType ==
                                            IdentityClaimValueType.DateTime
                                                ? DateTime.Parse(c.ClaimValue, CultureInfo.InvariantCulture)
                                                : DateTime.Now,
                                        ClaimValueBool = allClaims.FirstOrDefault(ac =>ac.Name == c.ClaimType)?.ValueType ==
                                            IdentityClaimValueType.Boolean
                                            ? bool.Parse(c.ClaimValue)
                                            : default(bool)
                                    }
                                ).ToList()
                        };

                        await SetSelectedClaimValueType(ClaimsModel.AllClaims.FirstOrDefault()?.Name);
                        await ClaimsModal.Show();
                    }
                },
                new EntityAction {
                    Text = L["Lock"],
                    Visible = (data) =>
                        CurrentUser.Id != data.As<IdentityUserDto>().Id &&
                        data.As<IdentityUserDto>().LockoutEnabled &&
                        HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        LockingUser = new UserLockViewModel()
                        {
                            Id = data.As<IdentityUserDto>().Id,
                            LockoutEnd = data.As<IdentityUserDto>().LockoutEnd?.LocalDateTime ?? DateTime.Now.AddDays(7).Date
                        };

                        await LockModal.Show();
                    }
                },
                new EntityAction {
                    Text = L["Unlock"],
                    Visible = (data) =>
                        CurrentUser.Id != data.As<IdentityUserDto>().Id &&
                        data.As<IdentityUserDto>().IsLockedOut &&
                        HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        await identityUserAppService.UnlockAsync(data.As<IdentityUserDto>().Id);

                        await Notify.Success(L["UserUnlocked"]);

                        await GetEntitiesAsync();
                        await InvokeAsync(StateHasChanged);
                    }
                },
                new EntityAction {
                    Text = L["Permissions"],
                    Visible = (data) => HasManagePermissionsPermission,
                    Clicked = async (data) =>
                    {
                        var user = data.As<IdentityUserDto>();
                        await PermissionManagementModal.OpenAsync(PermissionProviderName,
                            user.Id.ToString(), user.Name);
                    }
                },
                new EntityAction {
                    Text = L["SetPassword"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        RandomPasswordGenerated = false;

                        ChangePasswordModel = new ChangeUserPasswordViewModel {
                            Id = data.As<IdentityUserDto>().Id,
                            UserName = data.As<IdentityUserDto>().UserName,
                            NewPassword = string.Empty
                        };

                        await ChangePasswordModal.Show();
                        ShowPassword = false;
                    }
                },
                new EntityAction {
                    Text = L["TwoFactor"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        TwoFactorModel = new TwoFactorViewModel {
                            Id = data.As<IdentityUserDto>().Id,
                            UserName = data.As<IdentityUserDto>().UserName,
                            TwoFactorEnabled = data.As<IdentityUserDto>().TwoFactorEnabled
                        };

                        await TwoFactorModal.Show();
                    }
                },
                new EntityAction {
                    Text = L["Sessions"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        CurrentSessionUserId = data.As<IdentityUserDto>().Id;
                        CurrentSessionUserName = data.As<IdentityUserDto>().UserName;
                        await SessionsModal.Show();
                    }
                },
                new EntityAction {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission && data.As<IdentityUserDto>().Id != CurrentUser.Id,
                    Clicked = async (data) => await DeleteEntityAsync(data.As<IdentityUserDto>()),
                    ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<IdentityUserDto>())
                }
            });

        if (Options.Value.EnableUserImpersonation && CurrentUser.FindImpersonatorUserId() == null)
        {
            EntityActions.Get<UserManagement>().InsertBefore(x => x.Text == L["Delete"], new EntityAction
            {
                Text = L["LoginWithThisUser"],
                Visible = (data) => HasImpersonationPermission && data.As<IdentityUserDto>().Id != CurrentUser.Id,
                Clicked = async (data) =>
                {
                    await JSRuntime.InvokeVoidAsync("eval",
                        $"document.getElementById('ImpersonationUserId').value = '{data.As<IdentityUserDto>().Id:D}'");
                    await JSRuntime.InvokeVoidAsync("eval", "document.getElementById('ImpersonationForm').submit()");
                }
            });
        }

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddComponent<IdentityUserImportDropdownComponent>(requiredPolicyName: IdentityPermissions.Users.Import);
        Toolbar.AddComponent<IdentityUserExportDropdownComponent>(requiredPolicyName: IdentityPermissions.Users.Export);

        Toolbar.AddButton(L["NewUser"],
            OpenCreateModalAsync,
            IconName.Add,
            requiredPolicyName: CreatePolicyName);

        return base.SetToolbarItemsAsync();
    }

    protected virtual async Task OnRoleChangedAsync(Guid roleId)
    {
        AdvancedFilterInput.RoleId = roleId;
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task OnOrganizationUnitChangedAsync(Guid organizationId)
    {
        AdvancedFilterInput.OrganizationUnitId = organizationId;
        await base.SearchEntitiesAsync();
    }

    protected virtual void ChangePasswordTextRole()
    {
        ShowPassword = !ShowPassword;
    }

    protected virtual async Task OnDataGridChangedAsync()
    {
        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OnMinCreationTimeChangedAsync(DateTime? date)
    {
        GetListInput.MinCreationTime = date;
        GetListInput.MaxCreationTime = DateTime.MaxValue;

        await GetEntitiesAsync();
    }

    protected virtual async Task OnMaxCreationTimeChangedAsync(DateTime? date)
    {
        GetListInput.MinCreationTime = DateTime.MinValue;
        GetListInput.MaxCreationTime = date;

        await GetEntitiesAsync();
    }

    protected virtual async Task OnMinModificationTimeChangedAsync(DateTime? date)
    {
        GetListInput.MinModifitionTime = date;
        GetListInput.MaxModifitionTime = DateTime.MaxValue;

        await GetEntitiesAsync();
    }

    protected virtual async Task OnMaxModificationTimeChangedAsync(DateTime? date)
    {
        GetListInput.MinModifitionTime = DateTime.MinValue;
        GetListInput.MaxModifitionTime = date;

        await GetEntitiesAsync();
    }

    protected virtual async Task OnNotActiveChangedAsync(string notActive)
    {
        AdvancedFilterInput.NotActive = notActive;
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task OnEmailConfirmedChangedAsync(string emailConfirmed)
    {
        AdvancedFilterInput.EmailConfirmed = emailConfirmed;
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task OnIsLockedOutChangedAsync(string isLockedOut)
    {
        AdvancedFilterInput.IsLockedOut = isLockedOut;
        await base.SearchEntitiesAsync();
    }

    protected virtual async Task OnIsExternalChangedAsync(string isExternal)
    {
        AdvancedFilterInput.IsExternal = isExternal;
        await base.SearchEntitiesAsync();
    }

    protected override Task GetEntitiesAsync()
    {
        SetFilter();
        return base.GetEntitiesAsync();
    }

    protected virtual GetIdentityUsersInput GetFilter()
    {
        SetFilter();
        return GetListInput;
    }

    protected virtual void SetFilter()
    {
        GetListInput.OrganizationUnitId = AdvancedFilterInput.OrganizationUnitId != Guid.Empty ? AdvancedFilterInput.OrganizationUnitId : null;
        GetListInput.RoleId = AdvancedFilterInput.RoleId != Guid.Empty ? AdvancedFilterInput.RoleId : null;
        GetListInput.NotActive = AdvancedFilterInput.NotActive == AdvancedFilterInput.NullSelectionValue ? null : AdvancedFilterInput.NotActive == "True";
        GetListInput.EmailConfirmed = AdvancedFilterInput.EmailConfirmed == AdvancedFilterInput.NullSelectionValue ? null : AdvancedFilterInput.EmailConfirmed == "True";
        GetListInput.IsLockedOut = AdvancedFilterInput.IsLockedOut == AdvancedFilterInput.NullSelectionValue ? null : AdvancedFilterInput.IsLockedOut == "True";
        GetListInput.IsExternal = AdvancedFilterInput.IsExternal == AdvancedFilterInput.NullSelectionValue ? null : AdvancedFilterInput.IsExternal == "True";
    }

    public void Dispose()
    {
        UserManagementState.OnDataGridChanged -= OnDataGridChangedAsync;
        UserManagementState.OnGetFilter -= GetFilter;
    }

    protected virtual Task OnSelectedOrganizationUnitChanged(Guid id, List<IdentityRoleDto> roles, bool createModal)
    {
        SelectedOrganizationUnits[id] = !SelectedOrganizationUnits[id];

        foreach (var role in roles)
        {
            var selectedRole = createModal ? NewUserRoles.FirstOrDefault(x => x.Name == role.Name) : EditUserRoles.FirstOrDefault(x => x.Name == role.Name);
            if (selectedRole == null)
            {
                continue;
            }

            selectedRole.IsAssigned = SelectedOrganizationUnits[id];
            selectedRole.IsInheritedFromOu = SelectedOrganizationUnits[id];
        }

        return Task.CompletedTask;
    }
}

public class UserLockViewModel
{
    public Guid Id { get; set; }

    [Required] public DateTime LockoutEnd { get; set; }
}

public class ChangeUserPasswordViewModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    [DataType(DataType.Password)] public string NewPassword { get; set; }
}

public class TwoFactorViewModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public bool TwoFactorEnabled { get; set; }
}

public class AssignedRoleViewModel
{
    public string Name { get; set; }

    public bool IsAssigned { get; set; }

    public bool IsInheritedFromOu { get; set; }
}

public class ClaimsViewModel
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public List<ClaimTypeDto> AllClaims { get; set; } = new List<ClaimTypeDto>();

    public List<IdentityUserClaimViewModel> OwnedClaims { get; set; } = new List<IdentityUserClaimViewModel>();
}

public class IdentityUserClaimViewModel
{
    public string ClaimType { get; set; }

    public string ClaimValueText { get; set; }

    public int ClaimValueNumeric { get; set; }

    public DateTime ClaimValueDate { get; set; }

    public bool ClaimValueBool { get; set; }
}

public class AdvancedFilterInput
{
    public Guid OrganizationUnitId { get; set; }
    public Guid RoleId { get; set; }
    public string NotActive { get; set; } = NullSelectionValue;
    public string EmailConfirmed { get; set; } = NullSelectionValue;
    public string IsLockedOut { get; set; } = NullSelectionValue;
    public string IsExternal { get; set; } = NullSelectionValue;

    public const string NullSelectionValue = "empty_value";
}
