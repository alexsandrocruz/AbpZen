using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class FaqGroupInfoDto
{
    [Required]
    [DynamicMaxLength(typeof(FaqSectionConst), nameof(FaqSectionConst.MaxGroupNameLength))]
    public string Name { get; set; }
}
