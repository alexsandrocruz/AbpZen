using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.Public.Newsletters;

namespace Volo.CmsKit.Admin.Newsletters;

public class UpdatePreferenceInput
{
    [Required]
    [DataType(DataType.EmailAddress)]
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
}