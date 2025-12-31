using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class UpdateFaqSectionDto
{
    [Required]
    [DynamicMaxLength(typeof(FaqSectionConst), nameof(FaqSectionConst.MaxGroupNameLength))]
    public string GroupName { get; set; }

    [Required]
    [DynamicMaxLength(typeof(FaqSectionConst), nameof(FaqSectionConst.MaxNameLength))]
    public string Name { get; set; }

    public int Order { get; set; }
}
