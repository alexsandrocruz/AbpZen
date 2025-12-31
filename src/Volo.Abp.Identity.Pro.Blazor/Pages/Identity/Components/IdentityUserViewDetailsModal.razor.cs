using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;

public partial class IdentityUserViewDetailsModal
{
    protected const string DefaultSelectedTab = "UserInformations";
    protected string DefaultDateTimeFormat { get; set; } = "MMMM dd, yyyy — HH:mm";
    protected string DefaultEmptyValue { get; set; } = "-";
    
    protected Modal ViewDetailsModal;
    
    protected string EditModalSelectedTab = DefaultSelectedTab;
    
    [Inject]
    protected IIdentityUserAppService UserAppService { get; set; }
    
    protected List<OrganizationUnitTreeView> OrganizationUnits;

    protected Dictionary<Guid, bool> SelectedOrganizationUnits;
    
    protected string ManageOuPolicyName;

    protected bool HasManageOuPermission;
    
    protected Guid UserId { get; set; }
    
    protected string CreatedBy { get; set; }
    
    protected string ModifiedBy { get; set; }
    
    protected IdentityUserDto User { get; set; }

    public IdentityUserViewDetailsModal()
    {
        ManageOuPolicyName = IdentityPermissions.Users.ManageOU;
    }

    protected async override Task OnInitializedAsync()
    {
       await SetPermissionAsync();
    }
    
    protected virtual async Task GetUserAsync()
    {
        User = await UserAppService.GetAsync(UserId);
        CreatedBy = await GetUserNameOrNullAsync(User.CreatorId);
        ModifiedBy = await GetUserNameOrNullAsync(User.LastModifierId);
        
        var organizationUnits = await UserAppService.GetOrganizationUnitsAsync(UserId);

        if (HasManageOuPermission)
        {
            var assignedOuIds = organizationUnits.Select(x => x.Id).ToList();
            await GetOrganizationUnitsAsync(assignedOuIds);
        }
        else
        {
            OrganizationUnits = new List<OrganizationUnitTreeView>();
        }
    }
    
    protected virtual async Task GetOrganizationUnitsAsync(ICollection<Guid> selectedOuIds = null)
    {
        selectedOuIds ??= new List<Guid>();

        var organizationUnitsDto = await UserAppService.GetAvailableOrganizationUnitsAsync();

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
            if (organizationUnitsDictionary.TryGetValue(organizationUnit.Id, out var value))
            {
                organizationUnit.Children = value;
            }
        }

        OrganizationUnits = organizationUnitsDictionary.Any() ? organizationUnitsDictionary[Guid.Empty] : new List<OrganizationUnitTreeView>();
    }

    protected virtual async Task SetPermissionAsync()
    {
        HasManageOuPermission = await AuthorizationService.IsGrantedAsync(ManageOuPolicyName);
    }

    public virtual async Task OpenAsync(Guid id)
    {
        UserId = id;
        await GetUserAsync();
        await InvokeAsync(ViewDetailsModal.Show);
    }
    
    protected async Task CloseViewDetailsModalAsync()
    {
        await ViewDetailsModal.Close(CloseReason.None);
    }
    
    protected virtual async Task<string> GetUserNameOrNullAsync(Guid? userId)
    {
        if (!userId.HasValue)
        {
            return null;
        }

        var user = await UserAppService.FindByIdAsync(userId.Value);
        return user?.UserName;
    }
    
    protected virtual string ConvertUserFriendlyFormat(DateTime? dateTime)
    {
        return dateTime == null ? DefaultEmptyValue : dateTime.Value.ToUniversalTime().ToString(DefaultDateTimeFormat);
    }

    protected virtual string ConvertUserFriendlyFormat(DateTimeOffset? dateTime)
    {
        return dateTime == null ? DefaultEmptyValue : dateTime.Value.UtcDateTime.ToString(DefaultDateTimeFormat);
    }

    protected virtual string ConvertUserFriendlyFormat(string value)
    {
        return value.IsNullOrWhiteSpace() ? DefaultEmptyValue : value;
    }
    
    protected virtual string ConvertUserFriendlyFormat(bool value)
    {
        return value ? L["Yes"].Value : L["No"].Value;
    }
}