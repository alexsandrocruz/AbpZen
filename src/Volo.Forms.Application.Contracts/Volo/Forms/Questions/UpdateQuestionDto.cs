using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.Forms.Choices;

namespace Volo.Forms.Questions;

public class UpdateQuestionDto
{
    [Required]
    public int Index { get; set; }

    [Required]
    [DynamicStringLength(typeof(FormConsts),nameof(FormConsts.MaxTitleLength))]
    public string Title { get; set; }

    [DynamicStringLength(typeof(FormConsts),nameof(FormConsts.MaxDescriptionLength))]
    public string Description { get; set; }

    public bool IsRequired { get; set; }

    public bool HasOtherOption { get; set; }

    [Required]
    public QuestionTypes QuestionType { get; set; }

    public List<ChoiceDto> Choices { get; set; } = new();
}
