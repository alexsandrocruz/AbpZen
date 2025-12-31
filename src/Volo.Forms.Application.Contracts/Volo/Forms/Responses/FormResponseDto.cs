using System;
using System.Collections.ObjectModel;
using Volo.Abp.Application.Dtos;

namespace Volo.Forms.Responses;

public class FormResponseDto : FullAuditedEntityDto<Guid>
{
    public Guid? UserId { get; set; }
    public Guid FormId { get; set; }
    public string Email { get; set; }
    public Collection<Guid> Answers { get; set; }
}
