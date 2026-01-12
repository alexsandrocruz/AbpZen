using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeptonXDemoApp.LeadMessage.Dtos;

[Serializable]
public class CreateUpdateLeadMessageDto
{
    public string Title { get; set; }
    public DateTime? Date { get; set; }
    public string Body { get; set; }
    public bool? Success { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? MessageTemplateId { get; set; }
    public Guid? LeadId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
