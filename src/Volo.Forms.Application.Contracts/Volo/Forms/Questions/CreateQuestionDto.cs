using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Forms.Choices;

namespace Volo.Forms.Questions;

public class CreateQuestionDto
{
    [Required]
    public int Index { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsRequired { get; set; }

    public bool HasOtherOption { get; set; }

    [Required]
    public QuestionTypes QuestionType { get; set; }

    public List<ChoiceDto> Choices { get; set; } = new();
}
