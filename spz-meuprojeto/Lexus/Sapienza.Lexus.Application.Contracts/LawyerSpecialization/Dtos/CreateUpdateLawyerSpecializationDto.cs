#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.LawyerSpecialization.Dtos;

[Serializable]
public class CreateUpdateLawyerSpecializationDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    public Guid LawyerId { get; set; }
    [Required]
    public Guid SpecializationId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
