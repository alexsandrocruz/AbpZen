using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host.Pages.Saas.Host.Editions;

public class DeleteEditionModal : SaasHostPageModel
{
    [BindProperty]
    public EditionInfoModel Edition { get; set; }

    protected IEditionAppService EditionAppService { get; }

    public DeleteEditionModal(IEditionAppService editionAppService)
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
            TenantCount = edition.TenantCount,
            OtherEditions = allEditions.Where(e => e.Id != edition.Id).Select(e => new KeyValuePair<Guid, string>(e.Id, e.DisplayName)).ToList()
        };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        await EditionAppService.MoveAllTenantsAsync(Edition.Id, Edition.AssignToEditionId);
        await EditionAppService.DeleteAsync(Edition.Id);
        return NoContent();
    }

    public class EditionInfoModel : ExtensibleObject
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public long TenantCount { get; set; }

        public List<KeyValuePair<Guid, string>> OtherEditions { get; set; }

        public Guid? AssignToEditionId { get; set; }
    }
}
