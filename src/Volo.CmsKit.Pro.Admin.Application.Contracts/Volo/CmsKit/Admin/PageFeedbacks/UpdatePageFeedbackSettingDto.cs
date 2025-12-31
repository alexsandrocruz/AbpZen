using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Validation;
using Volo.CmsKit.Localization;
using Volo.CmsKit.PageFeedbacks;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[Serializable]
public class UpdatePageFeedbackSettingDto : IValidatableObject
{
    public Guid Id { get; set; }
    
    [DynamicMaxLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxEntityTypeLength))]
    public string EntityType { get; set; }
    
    [DynamicMaxLength(typeof(PageFeedbackConst), nameof(PageFeedbackConst.MaxEmailAddressesLength))]
    public string EmailAddresses { get; set; }
    
    
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizer = validationContext.GetRequiredService<IStringLocalizer<CmsKitResource>>();

        if (string.IsNullOrWhiteSpace(EmailAddresses))
        {
            yield break;
        }
        
        foreach (var emailAddress in EmailAddresses.Split(PageFeedbackConst.EmailAddressesSeparator))
        {
            if (!ValidationHelper.IsValidEmailAddress(emailAddress.Trim()))
            {
                yield return new ValidationResult(
                    localizer["InvalidEmailAddress", emailAddress],
                    new[] { nameof(EmailAddresses) }
                );
            }
        }
    }
}