using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Admin.Newsletters;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Newsletters;

public class DetailModel : AbpPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public NewsletterRecordWithDetailsDto NewsletterRecordWithDetailsDto { get; set; }

    private readonly INewsletterRecordAdminAppService _newsletterRecordAdminAppService;

    public DetailModel(INewsletterRecordAdminAppService newsletterRecordAdminAppService)
    {
        _newsletterRecordAdminAppService = newsletterRecordAdminAppService;
    }

    public async Task OnGetAsync()
    {
        NewsletterRecordWithDetailsDto = await _newsletterRecordAdminAppService.GetAsync(Id);
    }
}
