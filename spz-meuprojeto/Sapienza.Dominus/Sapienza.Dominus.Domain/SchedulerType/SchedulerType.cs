// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SchedulerType;

/// <summary>
/// SchedulerType entity
/// </summary>
public class SchedulerType : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.Booking.Booking> Bookings { get; set; } = new List<Dominus.Booking.Booking>();
    public virtual ICollection<Dominus.SchedulerAvailability.SchedulerAvailability> SchedulerAvailabilities { get; set; } = new List<Dominus.SchedulerAvailability.SchedulerAvailability>();
    public virtual ICollection<Dominus.SchedulerException.SchedulerException> SchedulerExceptions { get; set; } = new List<Dominus.SchedulerException.SchedulerException>();

    protected SchedulerType()
    {
        // Required by EF Core
    }

    public SchedulerType(Guid id) : base(id)
    {
    }
}
