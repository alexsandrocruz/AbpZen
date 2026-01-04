using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.LeadMessage.Dtos;

[Serializable]
public class LeadMessageDto : FullAuditedEntityDto<Guid>
{
    public string Title { get; set; }
    public DateTime? Date { get; set; }
    public string Body { get; set; }
    public bool? Success { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? MessageTemplateId { get; set; }
    public string? MessageTemplateDisplayName { get; set; }
    public Guid? LeadId { get; set; }
    public string? LeadDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
