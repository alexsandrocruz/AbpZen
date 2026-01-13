// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.EventoZen.EventCommission;

/// <summary>
/// EventCommission entity
/// </summary>
public class EventCommission : FullAuditedAggregateRoot<Guid>
{
    public string? Description { get; set; }
    public decimal? Value { get; set; }
    public decimal? Percentage { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? EventId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.EventoZen.Event.Event? Event { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected EventCommission()
    {
        // Required by EF Core
    }

    public EventCommission(Guid id) : base(id)
    {
    }
}
