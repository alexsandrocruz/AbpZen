// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.EventoZen.Location;

/// <summary>
/// Location entity
/// </summary>
public class Location : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public int? Capacity { get; set; }
    public string? ZipCode { get; set; }
    public string? Notes { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.EventoZen.Event.Event> Events { get; set; } = new List<Sapienza.EventoZen.Event.Event>();

    protected Location()
    {
        // Required by EF Core
    }

    public Location(Guid id) : base(id)
    {
    }
}
