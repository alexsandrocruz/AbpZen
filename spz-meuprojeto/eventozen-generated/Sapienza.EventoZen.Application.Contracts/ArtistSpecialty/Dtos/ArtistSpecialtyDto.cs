using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.ArtistSpecialty.Dtos;

[Serializable]
public class ArtistSpecialtyDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ArtistId { get; set; }
    public string? ArtistDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
