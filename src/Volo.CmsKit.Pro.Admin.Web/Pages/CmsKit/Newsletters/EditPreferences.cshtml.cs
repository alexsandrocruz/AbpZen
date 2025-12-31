using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.CmsKit.Admin.Newsletters;
using Volo.CmsKit.Public.Newsletters;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Newsletters;

public class EditPreferencesModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string EmailAddress { get; set; }

    [BindProperty]
    public List<EditPreferenceViewModel> NewsletterPreferences { get; set; }

    private readonly INewsletterRecordAdminAppService _newsletterRecordAdminAppService;

    public EditPreferencesModel(INewsletterRecordAdminAppService newsletterRecordAdminAppService)
    {
        _newsletterRecordAdminAppService = newsletterRecordAdminAppService;
    }

    public async Task OnGetAsync()
    {
        var newsletterPreferences =
            await _newsletterRecordAdminAppService.GetNewsletterPreferencesAsync(EmailAddress);
        
        NewsletterPreferences = newsletterPreferences.Select(x => new EditPreferenceViewModel {
            Preference = x.Preference,
            DisplayPreference = x.DisplayPreference,
            IsEnabled = x.IsSelectedByEmailAddress
        }).ToList();
    }
    
    public async Task OnPostAsync()
    {
        await _newsletterRecordAdminAppService.UpdatePreferencesAsync(new UpdatePreferenceInput() {
            EmailAddress = EmailAddress,
            PreferenceDetails = NewsletterPreferences.Select(x => new PreferenceDetailsDto() {
                Preference = x.Preference,
                IsEnabled = x.IsEnabled
            }).ToList(),
            Source = "Admin",
            SourceUrl = "/CmsKit/Newsletters/EditPreferences"
        });
    }

    public class EditPreferenceViewModel
    {
        public string Preference { get; set; }

        public string DisplayPreference { get; set; }

        public bool IsEnabled { get; set; }
    }
}