using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users;

public class EditModalModel : IdentityUserModalPageModel
{
    [BindProperty] public UserInfoViewModel UserInfo { get; set; }

    [BindProperty] public AssignedRoleViewModel[] Roles { get; set; }

    protected IIdentityUserAppService IdentityUserAppService { get; }

    protected IPermissionChecker PermissionChecker { get; }

    public bool IsEditCurrentUser { get; set; }

    public EditModalModel(IIdentityUserAppService identityUserAppService, IPermissionChecker permissionChecker)
    {
        IdentityUserAppService = identityUserAppService;
        PermissionChecker = permissionChecker;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var user = await IdentityUserAppService.GetAsync(id);

        UserInfo = ObjectMapper.Map<IdentityUserDto, UserInfoViewModel>(user);

        IsEditCurrentUser = CurrentUser.Id == id;

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles))
        {
            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>((await IdentityUserAppService.GetAssignableRolesAsync()).Items);

            var userRoleNames = (await IdentityUserAppService.GetRolesAsync(UserInfo.Id))
                .Items
                .Select(r => HttpUtility.HtmlEncode(r.Name))
                .ToList();

            foreach (var role in Roles)
            {
                if (userRoleNames.Contains(role.Name))
                {
                    role.IsAssigned = true;
                }
            }
        }

        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageOU))
        {
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

            var userOrganizationUnitRoleIds = userOrganizationUnits.SelectMany(q => q.Roles)
                .Select(r => r.RoleId)
                .Distinct()
                .ToList();

            foreach (var role in Roles)
            {
                if (userOrganizationUnitRoleIds.Contains(role.Id))
                {
                    role.IsInheritedFromOu = true;
                }
            }
        }
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserUpdateDto>(UserInfo);
        input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();
        input.OrganizationUnitIds = OrganizationUnits.Where(ou => ou.IsAssigned).Select(ou => ou.Id).ToArray();

        await IdentityUserAppService.UpdateAsync(UserInfo.Id, input);

        return NoContent();
    }

    private void HttpDecodeRoleNames()
    {
        if (Roles != null && Roles.Any())
        {
            foreach (var role in Roles)
            {
                role.Name = HttpUtility.HtmlDecode(role.Name);
            }
        }
    }

    private async Task<string> GetUserNameOrNullAsync(Guid? userId)
    {
        if (!userId.HasValue)
        {
            return null;
        }

        var user = await IdentityUserAppService.FindByIdAsync(userId.Value);
        return user?.UserName;
    }

    public class UserInfoViewModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [HiddenInput] public Guid Id { get; set; }

        [HiddenInput] public string ConcurrencyStamp { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool ShouldChangePasswordOnNextLogin { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool IsExternal { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
    }

    public class AssignedRoleViewModel
    {
        [HiddenInput] public Guid Id { get; set; }
        [Required] [HiddenInput] public string Name { get; set; }

        public bool IsAssigned { get; set; }

        public bool IsInheritedFromOu { get; set; }

        public string GetShownName(string inheritedFrom)
        {
            if (!IsInheritedFromOu)
            {
                return HttpUtility.HtmlEncode(Name);
            }

            //Maybe return name of the OU later if required
            return $"{HttpUtility.HtmlEncode(Name)} ({inheritedFrom})";
        }
    }

}
