using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.EventoZen.Availability.Dtos;

[Serializable]
public class CreateUpdateAvailabilityDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public AvailabilityType Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Notes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ArtistId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
