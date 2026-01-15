// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.LeadStageHistory;

/// <summary>
/// LeadStageHistory entity
/// </summary>
public class LeadStageHistory : Entity<Guid>
{
    public string Notes { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? LeadId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Lead.Lead? Lead { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected LeadStageHistory()
    {
        // Required by EF Core
    }

    public LeadStageHistory(Guid id) : base(id)
    {
    }
}
