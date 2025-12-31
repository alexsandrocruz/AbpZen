using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users;

public class ViewDetailsModalModel : IdentityUserModalPageModel
{
    public ViewDetailsModalModel(IIdentityUserAppService identityUserAppService)
    {
        IdentityUserAppService = identityUserAppService;
    }

    public UserInfoViewModel UserInfo { get; set; }
    
    protected IIdentityUserAppService IdentityUserAppService { get; }

    public virtual async Task OnGetAsync(Guid id)
    {
        var user = await IdentityUserAppService.GetAsync(id);
        
        UserInfo = ObjectMapper.Map<IdentityUserDto, UserInfoViewModel>(user);
        UserInfo.CreatedBy = await GetUserNameOrNullAsync(user.CreatorId);
        UserInfo.ModifiedBy = await GetUserNameOrNullAsync(user.LastModifierId);
        
        var userOrganizationUnits = (await IdentityUserAppService.GetOrganizationUnitsAsync(id)).ToList();

        OrganizationUnits =
            ObjectMapper.Map<IReadOnlyList<OrganizationUnitWithDetailsDto>, AssignedOrganizationUnitViewModel[]>(
                (await IdentityUserAppService.GetAvailableOrganizationUnitsAsync()).Items
            );

        var userOrganizationUnitIds = userOrganizationUnits.Select(ou => ou.Id).ToList();

        foreach (var ou in OrganizationUnits)
        {
            if (userOrganizationUnitIds.Contains(ou.Id))
            {
                ou.IsAssigned = true;
            }
        }

        OrganizationUnitTreeRootNode = CreateOrganizationTree(new OrganizationTreeNode()
        {
            Id = null,
            Children = new List<OrganizationTreeNode>(),
            Index = -1
        });
    }
    
    protected virtual async Task<string> GetUserNameOrNullAsync(Guid? userId)
    {
        if (!userId.HasValue)
        {
            return null;
        }

        var user = await IdentityUserAppService.FindByIdAsync(userId.Value);
        return user?.UserName;
    }
    
    public class UserInfoViewModel : ExtensibleObject
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public bool LockoutEnabled { get; set; }
        
        public List<string> RoleNames { get; set; }
        
        public string CreatedBy { get; set; }
        
        public DateTime? CreationTime { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public DateTime? LastModificationTime { get; set; }
        
        public DateTimeOffset? LastPasswordChangeTime { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }
        
        public int AccessFailedCount { get; set; }

        public bool IsExternal { get; set; }
	}
}