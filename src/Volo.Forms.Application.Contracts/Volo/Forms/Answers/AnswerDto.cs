using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Forms.Answers;

public class AnswerDto : EntityDto<Guid>
{
    public Guid QuestionId { get; set; }
    public Guid? ChoiceId { get; set; }
    public Guid FormResponseId { get; set; }
    public DateTime AnswerDate { get; set; }
    public string Value { get; set; }
}
