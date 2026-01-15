// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.TimeEntry;

/// <summary>
/// TimeEntry entity
/// </summary>
public class TimeEntry : FullAuditedEntity<Guid>
{
    public string Description { get; set; } = string.Empty;
    public double Hours { get; set; }
    public DateTime Date { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProjectId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Project.Project? Project { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected TimeEntry()
    {
        // Required by EF Core
    }

    public TimeEntry(Guid id) : base(id)
    {
    }
}
