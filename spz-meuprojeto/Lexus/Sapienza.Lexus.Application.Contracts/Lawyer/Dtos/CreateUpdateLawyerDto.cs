using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sapienza.Lexus.LawyerSpecialization.Dtos;

namespace Sapienza.Lexus.Lawyer.Dtos;

[Serializable]
public class CreateUpdateLawyerDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(128)]
    public string FullName { get; set; } = string.Empty;
    [StringLength(64)]
    public string PreferredName { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdateLawyerSpecializationDto>? LawyerSpecializations { get; set; }
}
