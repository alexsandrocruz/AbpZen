// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.EventoZen.ArtistSpecialty;

/// <summary>
/// ArtistSpecialty entity
/// </summary>
public class ArtistSpecialty : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ArtistId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.EventoZen.Artist.Artist? Artist { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ArtistSpecialty()
    {
        // Required by EF Core
    }

    public ArtistSpecialty(Guid id) : base(id)
    {
    }
}
