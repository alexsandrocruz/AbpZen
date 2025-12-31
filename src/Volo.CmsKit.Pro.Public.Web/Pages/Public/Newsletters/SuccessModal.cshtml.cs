using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.CmsKit.Public.Newsletters;

namespace Volo.CmsKit.Pro.Public.Web.Pages.Public.Newsletters;

public class SuccessModalModel : CmsKitProPublicPageModel
{
    [Required]
    [BindProperty(SupportsGet = true)]
    public string EmailAddress { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string Preference { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string Source { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public string SourceUrl { get; set; }

    [Required]
    [BindProperty(SupportsGet = true)]
    public bool RequestAdditionalPreferences { get; set; }

    public string NormalizedSource = null;

    public List<string> AdditionalPreferences = null;

    public List<string> DisplayAdditionalPreferences = null;

    protected INewsletterRecordPublicAppService NewsletterRecordPublicAppService { get; }

    public SuccessModalModel(INewsletterRecordPublicAppService newsletterRecordPublicAppService)
    {
        NewsletterRecordPublicAppService = newsletterRecordPublicAppService;
    }

    public async Task OnGetAsync()
    {
        var newsletterEmailOptionsDto = await NewsletterRecordPublicAppService.GetOptionByPreferenceAsync(Preference);

        AdditionalPreferences = newsletterEmailOptionsDto.AdditionalPreferences;
        DisplayAdditionalPreferences = newsletterEmailOptionsDto.DisplayAdditionalPreferences;
        NormalizedSource = Source.Replace('.', '_');
    }

    public async Task OnPostAsync()
    {
        var newsletterEmailOptionsDto = await NewsletterRecordPublicAppService.GetOptionByPreferenceAsync(Preference);

        AdditionalPreferences = newsletterEmailOptionsDto.AdditionalPreferences;
        DisplayAdditionalPreferences = newsletterEmailOptionsDto.DisplayAdditionalPreferences;
        NormalizedSource = Source.Replace('.', '_');
    }
}
