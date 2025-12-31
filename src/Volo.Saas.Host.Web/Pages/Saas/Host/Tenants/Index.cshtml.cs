using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Volo.Saas.Host.Pages.Saas.Host.Tenants;

public class IndexModel : SaasHostPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid? EditionId { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? ExpirationDateMin { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? ExpirationDateMax { get; set; }

    [BindProperty(SupportsGet = true)]
    public TenantActivationState? ActivationState { get; set; }

    public List<SelectListItem> EditionsComboboxItems { get; set; } = new List<SelectListItem>
        {
            new SelectListItem("-", "", true)
        };

    protected ITenantAppService TenantAppService { get; }

    public IndexModel(ITenantAppService tenantAppService)
    {
        TenantAppService = tenantAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var editions = await TenantAppService.GetEditionLookupAsync();

        EditionsComboboxItems
            .AddRange(editions.Select(e => new SelectListItem(e.DisplayName, e.Id.ToString())).ToList());

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
