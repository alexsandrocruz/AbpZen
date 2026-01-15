using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Booking.Dtos;

[Serializable]
public class BookingGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? SchedulerTypeId { get; set; }
    public Guid? ClientId { get; set; }
}
