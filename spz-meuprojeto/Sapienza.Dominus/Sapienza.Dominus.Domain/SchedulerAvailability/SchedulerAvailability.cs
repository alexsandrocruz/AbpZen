// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SchedulerAvailability;

/// <summary>
/// SchedulerAvailability entity
/// </summary>
public class SchedulerAvailability : Entity<Guid>
{
    public int DayOfWeek { get; set; }
    public string StartTime { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SchedulerTypeId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.SchedulerType.SchedulerType? SchedulerType { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected SchedulerAvailability()
    {
        // Required by EF Core
    }

    public SchedulerAvailability(Guid id) : base(id)
    {
    }
}
