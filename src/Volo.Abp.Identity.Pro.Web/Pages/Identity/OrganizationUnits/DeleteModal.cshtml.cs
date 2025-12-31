using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity.Web.Pages.Identity.OrganizationUnits;

public class DeleteModal : IdentityPageModel
{
    [BindProperty]
    public OrganizationUnitInfoModel OrganizationUnit { get; set; }

    protected IOrganizationUnitAppService OrganizationUnitAppService { get; }

    public DeleteModal(IOrganizationUnitAppService organizationUnitAppService)
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
            UserCount = organizationUnit.UserCount,
            OtherOrganizationUnits = allOrganizationUnits.Items.Where(e => e.Id != organizationUnit.Id).Select(e => new KeyValuePair<Guid, string>(e.Id, e.DisplayName)).ToList()
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await OrganizationUnitAppService.MoveAllUsersAsync(OrganizationUnit.Id, OrganizationUnit.AssignToOrganizationUnitId);
        await OrganizationUnitAppService.DeleteAsync(OrganizationUnit.Id);
        return NoContent();
    }

    public class OrganizationUnitInfoModel : ExtensibleObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long UserCount { get; set; }

        public List<KeyValuePair<Guid, string>> OtherOrganizationUnits { get; set; }

        public Guid? AssignToOrganizationUnitId { get; set; }
    }
}
