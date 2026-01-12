#nullable enable
using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.LawyerSpecialization.Dtos;

[Serializable]
public class LawyerSpecializationGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? LawyerId { get; set; }
    public Guid? SpecializationId { get; set; }
}
