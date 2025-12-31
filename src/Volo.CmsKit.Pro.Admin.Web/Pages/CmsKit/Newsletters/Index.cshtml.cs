using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.CmsKit.Admin.Newsletters;
using Volo.CmsKit.Admin.Web.Pages;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Newsletters;

public class IndexModel : CmsKitAdminPageModel
{
    public List<SelectListItem> PreferencesSelectList { get; set; }

    [SelectItems(nameof(PreferencesSelectList))]
    public string Preference { get; set; }

    [DisplayName("Source")]
    public string Source { get; set; }
    
    public string EmailAddress { get; set; }

    private readonly INewsletterRecordAdminAppService _newsletterRecordAdminAppService;

    public IndexModel(INewsletterRecordAdminAppService newsletterRecordAdminAppService)
    {
        _newsletterRecordAdminAppService = newsletterRecordAdminAppService;
    }

    public async Task OnGetAsync()
    {
        var newsletterPreferences = await _newsletterRecordAdminAppService.GetNewsletterPreferencesAsync();

        PreferencesSelectList = new List<SelectListItem> { new SelectListItem("-", "") };
        foreach (var newsletterPreference in newsletterPreferences)
        {
            PreferencesSelectList.Add(new SelectListItem(L[newsletterPreference] , newsletterPreference));
        }
    }
}
