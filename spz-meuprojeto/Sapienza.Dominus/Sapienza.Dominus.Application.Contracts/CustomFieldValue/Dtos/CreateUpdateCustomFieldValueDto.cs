using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.CustomFieldValue.Dtos;

[Serializable]
public class CreateUpdateCustomFieldValueDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string EntityId { get; set; }
    public string ValueText { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? CustomFieldId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
