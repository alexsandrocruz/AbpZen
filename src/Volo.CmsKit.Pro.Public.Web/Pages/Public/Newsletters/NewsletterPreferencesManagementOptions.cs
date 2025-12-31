using Volo.Abp.Localization;

namespace Volo.CmsKit.Pro.Public.Web.Pages.Public.Newsletters;

public class NewsletterPreferencesManagementOptions
{
    public ILocalizableString PrivacyPolicyConfirmation { get; set; }
    
    public string Source { get; set; }
}