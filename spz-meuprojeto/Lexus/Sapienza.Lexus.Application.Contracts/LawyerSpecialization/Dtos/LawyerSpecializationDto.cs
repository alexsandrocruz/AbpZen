#nullable enable
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.LawyerSpecialization.Dtos;

[Serializable]
public class LawyerSpecializationDto : FullAuditedEntityDto<Guid>
{

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid LawyerId { get; set; }
    public string? LawyerDisplayName { get; set; }
    public Guid SpecializationId { get; set; }
    public string? SpecializationDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
