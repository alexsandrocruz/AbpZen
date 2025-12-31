using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Forms.Questions;

namespace Volo.Forms.Forms;

public class FormWithDetailsDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid? TenantId { get; set; }
    public bool CanEditResponse { get; set; }
    public bool IsCollectingEmail { get; set; }
    public bool HasLimitOneResponsePerUser { get; set; }
    public bool IsAcceptingResponses { get; set; }
    public bool IsQuiz { get; set; }
    public bool RequiresLogin { get; set; }
    public List<QuestionDto> Questions { get; set; }
}
