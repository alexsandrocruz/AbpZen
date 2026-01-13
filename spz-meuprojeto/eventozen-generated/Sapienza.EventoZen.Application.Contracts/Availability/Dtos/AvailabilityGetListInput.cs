using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Availability.Dtos;

[Serializable]
public class AvailabilityGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public AvailabilityType? Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ArtistId { get; set; }
}
