using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Pages.Identity.OrganizationUnits;

public class AddMemberModal : IdentityPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid OrganizationUnitId { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string OrganizationUnitName { get; set; }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
