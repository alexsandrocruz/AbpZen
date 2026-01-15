// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SiteVisitDailyStat;

/// <summary>
/// SiteVisitDailyStat entity
/// </summary>
public class SiteVisitDailyStat : Entity<Guid>
{
    public DateTime Date { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SiteId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Site.Site? Site { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected SiteVisitDailyStat()
    {
        // Required by EF Core
    }

    public SiteVisitDailyStat(Guid id) : base(id)
    {
    }
}
