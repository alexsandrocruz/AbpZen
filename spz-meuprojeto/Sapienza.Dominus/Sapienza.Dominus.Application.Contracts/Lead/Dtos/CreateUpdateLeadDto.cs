using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Lead.Dtos;

[Serializable]
public class CreateUpdateLeadDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
    public string Source { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadWorkflowId { get; set; }
    public Guid? LeadWorkflowStageId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
