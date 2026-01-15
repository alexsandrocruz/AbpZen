using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.LeadForm.Dtos;

[Serializable]
public class CreateUpdateLeadFormDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadWorkflowId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
