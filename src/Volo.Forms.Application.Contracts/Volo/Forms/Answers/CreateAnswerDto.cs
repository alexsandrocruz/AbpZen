using System;

namespace Volo.Forms.Answers;

public class CreateAnswerDto
{
    public Guid QuestionId { get; set; }
    public Guid? ChoiceId { get; set; }
    public string Value { get; set; }
}
