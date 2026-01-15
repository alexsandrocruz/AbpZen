using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.LeadScheduledMessage.Dtos;

[Serializable]
public class LeadScheduledMessageDto : FullAuditedEntityDto<Guid>
{
    public DateTime ScheduledFor { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadAutomationId { get; set; }
    public string? LeadAutomationDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
