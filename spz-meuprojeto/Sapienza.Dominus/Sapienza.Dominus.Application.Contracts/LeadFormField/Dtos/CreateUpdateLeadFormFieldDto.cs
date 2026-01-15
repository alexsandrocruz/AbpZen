using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.LeadFormField.Dtos;

[Serializable]
public class CreateUpdateLeadFormFieldDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Label { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadFormId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
