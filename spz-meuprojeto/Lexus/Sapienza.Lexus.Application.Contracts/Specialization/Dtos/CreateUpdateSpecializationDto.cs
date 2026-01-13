using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sapienza.Lexus.LawyerSpecialization.Dtos;

namespace Sapienza.Lexus.Specialization.Dtos;

[Serializable]
public class CreateUpdateSpecializationDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(64)]
    public string Name { get; set; } = string.Empty;
    [StringLength(512)]
    public string Description { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateLawyerSpecializationDto>? LawyerSpecializations { get; set; }
}
