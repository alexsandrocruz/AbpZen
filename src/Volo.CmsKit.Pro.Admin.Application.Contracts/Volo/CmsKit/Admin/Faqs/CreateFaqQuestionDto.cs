using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.CmsKit.Faqs;

namespace Volo.CmsKit.Admin.Faqs;

[Serializable]
public class CreateFaqQuestionDto
{
    [Required]
    public Guid SectionId { get; set; }

    [Required]
    [DynamicMaxLength(typeof(FaqQuestionConst), nameof(FaqQuestionConst.MaxTitleLength))]
    public string Title { get; set; }

    [Required]
    [DynamicMaxLength(typeof(FaqQuestionConst), nameof(FaqQuestionConst.MaxTextLength))]
    public string Text { get; set; }

    public int Order { get; set; }
}
