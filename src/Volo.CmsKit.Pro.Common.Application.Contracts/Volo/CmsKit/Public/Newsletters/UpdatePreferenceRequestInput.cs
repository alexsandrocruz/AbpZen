using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Newsletters;

namespace Volo.CmsKit.Public.Newsletters;

public class UpdatePreferenceRequestInput
{
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(NewsletterRecordConst), nameof(NewsletterRecordConst.MaxEmailAddressLength))]
    public string EmailAddress { get; set; }

    [Required]
    public List<PreferenceDetailsDto> PreferenceDetails { get; set; }

    [Required]
    [DynamicStringLength(typeof(NewsletterPreferenceConst), nameof(NewsletterPreferenceConst.MaxSourceLength))]
    public string Source { get; set; }

    [Required]
    [DynamicStringLength(typeof(NewsletterPreferenceConst), nameof(NewsletterPreferenceConst.MaxSourceUrlLength))]
    public string SourceUrl { get; set; }

    [Required]
    public string SecurityCode { get; set; }
}
