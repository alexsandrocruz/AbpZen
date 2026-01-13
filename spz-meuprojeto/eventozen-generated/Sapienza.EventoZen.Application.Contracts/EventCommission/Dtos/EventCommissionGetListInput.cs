using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.EventCommission.Dtos;

[Serializable]
public class EventCommissionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Description { get; set; }
    public decimal? Value { get; set; }
    public decimal? Percentage { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? EventId { get; set; }
}
