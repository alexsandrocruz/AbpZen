using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.LeadScheduledMessage.Dtos;

[Serializable]
public class CreateUpdateLeadScheduledMessageDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public DateTime ScheduledFor { get; set; }
    [Required]
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadAutomationId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
