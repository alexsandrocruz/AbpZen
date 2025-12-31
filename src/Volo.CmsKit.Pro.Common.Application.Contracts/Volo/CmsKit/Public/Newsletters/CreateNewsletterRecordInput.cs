using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Newsletters;

namespace Volo.CmsKit.Public.Newsletters;

public class CreateNewsletterRecordInput
{
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(NewsletterRecordConst), nameof(NewsletterRecordConst.MaxEmailAddressLength))]
    public string EmailAddress { get; set; }

    [Required]
    [DynamicStringLength(typeof(NewsletterPreferenceConst), nameof(NewsletterPreferenceConst.MaxPreferenceLength))]
    public string Preference { get; set; }

    [Required]
    [DynamicStringLength(typeof(NewsletterPreferenceConst), nameof(NewsletterPreferenceConst.MaxSourceLength))]
    public string Source { get; set; }

    [Required]
    [DynamicStringLength(typeof(NewsletterPreferenceConst), nameof(NewsletterPreferenceConst.MaxSourceUrlLength))]
    public string SourceUrl { get; set; }

    public string PrivacyPolicyUrl { get; set; }

    public List<string> AdditionalPreferences { get; set; }
}
