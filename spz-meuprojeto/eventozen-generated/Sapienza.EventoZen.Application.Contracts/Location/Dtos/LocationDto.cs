using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Location.Dtos;

[Serializable]
public class LocationDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int? Capacity { get; set; }
    public string ZipCode { get; set; }
    public string Notes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
