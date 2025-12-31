using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity.Web.Pages.Identity.Users;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity.Web.Pages.Identity.OrganizationUnits;

public class MoveAllUsersModal : IdentityUserModalPageModel
{
    [BindProperty]
    public OrganizationUnitInfoModel OrganizationUnit { get; set; }

    protected IOrganizationUnitAppService OrganizationUnitAppService { get; }

    public MoveAllUsersModal(IOrganizationUnitAppService organizationUnitAppService)
    {
        OrganizationUnitAppService = organizationUnitAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var organizationUnit = await OrganizationUnitAppService.GetAsync(id);
        var allOrganizationUnits = await OrganizationUnitAppService.GetListAllAsync();

        OrganizationUnit = new OrganizationUnitInfoModel()
        {
            Id = organizationUnit.Id,
            Name = organizationUnit.DisplayName,
            TargetOrganizationUnits = allOrganizationUnits.Items.Where(x => organizationUnit.Id != x.Id).Select(x => new KeyValuePair<Guid, string>(x.Id, x.DisplayName)).ToList()
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await OrganizationUnitAppService.MoveAllUsersAsync(OrganizationUnit.Id, OrganizationUnit.TargetOrganizationUnitId);
        return NoContent();
    }

    public class OrganizationUnitInfoModel : ExtensibleObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<KeyValuePair<Guid, string>> TargetOrganizationUnits { get; set; }

        public Guid? TargetOrganizationUnitId { get; set; }
    }
}
