using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles;

public class DeleteRoleModal : IdentityPageModel
{
    [BindProperty]
    public RoleInfoModel Role { get; set; }

    protected IIdentityRoleAppService IdentityRoleAppService { get; }

    public DeleteRoleModal(IIdentityRoleAppService identityRoleAppService)
    {
        IdentityRoleAppService = identityRoleAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var role = await IdentityRoleAppService.GetAsync(id);
        var allRoles = await IdentityRoleAppService.GetAllListAsync();

        Role = new RoleInfoModel()
        {
            Id = role.Id,
            Name = role.Name,
            UserCount = role.UserCount,
            OtherRoles = allRoles.Items.Where(e => e.Id != role.Id).Select(e => new KeyValuePair<Guid, string>(e.Id, e.Name)).ToList()
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await IdentityRoleAppService.MoveAllUsersAsync(Role.Id, Role.AssignToRoleId);
        await IdentityRoleAppService.DeleteAsync(Role.Id);
        return NoContent();
    }

    public class RoleInfoModel : ExtensibleObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long UserCount { get; set; }

        public List<KeyValuePair<Guid, string>> OtherRoles { get; set; }

        public Guid? AssignToRoleId { get; set; }
    }
}
