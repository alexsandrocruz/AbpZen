using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Location.Dtos;

[Serializable]
public class LocationGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int? Capacity { get; set; }
    public string? ZipCode { get; set; }
    public string? Notes { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
