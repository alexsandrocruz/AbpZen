using System;
using System.Collections.ObjectModel;
using Volo.Abp.Application.Dtos;
using Volo.Forms.Answers;

namespace Volo.Forms.Responses;

public class FormResponseDetailedDto : FullAuditedEntityDto<Guid>
{
    public Guid? UserId { get; set; }
    public Guid FormId { get; set; }
    public string Email { get; set; }
    public Collection<AnswerDto> Answers { get; set; }
}
