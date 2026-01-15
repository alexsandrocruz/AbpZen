using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.CustomField.Dtos;

[Serializable]
public class CreateUpdateCustomFieldDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string EntityType { get; set; }
    [Required]
    public string Label { get; set; }
    [Required]
    public string FieldType { get; set; }
    [Required]
    public string FieldKey { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
