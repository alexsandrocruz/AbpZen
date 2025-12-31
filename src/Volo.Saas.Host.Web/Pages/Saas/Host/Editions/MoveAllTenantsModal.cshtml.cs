using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.ObjectExtending;

namespace Volo.Saas.Host.Pages.Saas.Host.Editions;

public class MoveAllTenantsModal : SaasHostPageModel
{
    [BindProperty]
    public EditionInfoModel Edition { get; set; }

    protected IEditionAppService EditionAppService { get; }

    public MoveAllTenantsModal(IEditionAppService editionAppService)
    {
        EditionAppService = editionAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var edition = await EditionAppService.GetAsync(id);
        var allEditions = await EditionAppService.GetAllListAsync();

        Edition = new EditionInfoModel()
        {
            Id = edition.Id,
            DisplayName = edition.DisplayName,
            TargetEditions = allEditions.Where(x => x.Id != edition.Id).Select(e => new KeyValuePair<Guid, string>(e.Id, e.DisplayName)).ToList()
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await EditionAppService.MoveAllTenantsAsync(Edition.Id, Edition.TargetEditionId);
        return NoContent();
    }

    public class EditionInfoModel : ExtensibleObject
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public List<KeyValuePair<Guid, string>> TargetEditions { get; set; }

        public Guid? TargetEditionId { get; set; }
    }
}
