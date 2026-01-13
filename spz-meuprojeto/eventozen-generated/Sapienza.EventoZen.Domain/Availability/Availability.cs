// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.EventoZen.Availability;

/// <summary>
/// Availability entity
/// </summary>
public class Availability : FullAuditedAggregateRoot<Guid>
{
    public AvailabilityType Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ArtistId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.EventoZen.Artist.Artist? Artist { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Availability()
    {
        // Required by EF Core
    }

    public Availability(Guid id) : base(id)
    {
    }
}
