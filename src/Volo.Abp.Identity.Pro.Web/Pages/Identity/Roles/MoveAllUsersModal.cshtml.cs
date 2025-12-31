using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity.Web.Pages.Identity.Users;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles;

public class MoveAllUsersModal : IdentityUserModalPageModel
{
    [BindProperty]
    public RoleInfoModel Role { get; set; }

    protected IIdentityRoleAppService RoleAppService { get; }

    public MoveAllUsersModal(IIdentityRoleAppService roleAppService)
    {
        RoleAppService = roleAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var role = await RoleAppService.GetAsync(id);
        var allRoles = await RoleAppService.GetAllListAsync();

        Role = new RoleInfoModel()
        {
            CurrentRoleId = role.Id,
            CurrentRoleName = role.Name,
            TargetRoles = allRoles.Items.Where(x => x.Id != role.Id).Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name)).ToList()
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await RoleAppService.MoveAllUsersAsync(Role.CurrentRoleId, Role.TargetRoleId);
        return NoContent();
    }

    public class RoleInfoModel : ExtensibleObject
    {
        public Guid CurrentRoleId { get; set; }

        public string CurrentRoleName { get; set; }

        public List<KeyValuePair<Guid, string>> TargetRoles { get; set; }

        public Guid? TargetRoleId { get; set; }
    }
}
