using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Forms.Answers;
using Volo.Forms.Choices;

namespace Volo.Forms.Responses;

public class QuestionWithAnswersDto : FullAuditedEntityDto<Guid>
{
    public Guid FormId { get; set; }
    public int Index { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRequired { get; set; }
    public bool HasOtherOption { get; set; }
    public QuestionTypes QuestionType { get; set; }
    public List<ChoiceDto> Choices { get; set; }
    public List<AnswerDto> Answers { get; set; }
}
