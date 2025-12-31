using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Volo.CmsKit.Public.Newsletters;

public class NewsletterEmailOptionsDto
{
    public string PrivacyPolicyConfirmation { get; set; }

    public string WidgetViewPath { get; set; }

    public List<string> AdditionalPreferences { get; set; }

    public List<string> DisplayAdditionalPreferences { get; set; }

    public NewsletterEmailOptionsDto()
    {
        DisplayAdditionalPreferences = new List<string>();
        AdditionalPreferences = new List<string>();
    }
}
