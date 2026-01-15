// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Booking;

/// <summary>
/// Booking entity
/// </summary>
public class Booking : FullAuditedAggregateRoot<Guid>
{
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SchedulerTypeId { get; set; }
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.SchedulerType.SchedulerType? SchedulerType { get; set; }
    public virtual Dominus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Booking()
    {
        // Required by EF Core
    }

    public Booking(Guid id) : base(id)
    {
    }
}
