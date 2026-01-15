// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SiteVisitEvent;

/// <summary>
/// SiteVisitEvent entity
/// </summary>
public class SiteVisitEvent : Entity<Guid>
{
    public string VisitorId { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SiteId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Site.Site? Site { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected SiteVisitEvent()
    {
        // Required by EF Core
    }

    public SiteVisitEvent(Guid id) : base(id)
    {
    }
}
