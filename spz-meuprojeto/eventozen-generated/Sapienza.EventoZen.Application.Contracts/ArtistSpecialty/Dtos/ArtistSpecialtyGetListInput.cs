using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.ArtistSpecialty.Dtos;

[Serializable]
public class ArtistSpecialtyGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ArtistId { get; set; }
}
