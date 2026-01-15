// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SchedulerException;

/// <summary>
/// SchedulerException entity
/// </summary>
public class SchedulerException : Entity<Guid>
{
    public DateTime Date { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? SchedulerTypeId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.SchedulerType.SchedulerType? SchedulerType { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected SchedulerException()
    {
        // Required by EF Core
    }

    public SchedulerException(Guid id) : base(id)
    {
    }
}
