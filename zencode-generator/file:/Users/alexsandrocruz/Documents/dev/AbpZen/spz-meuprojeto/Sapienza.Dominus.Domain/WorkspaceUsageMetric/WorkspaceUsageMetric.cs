// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.WorkspaceUsageMetric;

/// <summary>
/// WorkspaceUsageMetric entity
/// </summary>
public class WorkspaceUsageMetric : Entity<Guid>
{
    public DateTime Date { get; set; }
    public int TotalSessions { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected WorkspaceUsageMetric()
    {
        // Required by EF Core
    }

    public WorkspaceUsageMetric(Guid id) : base(id)
    {
    }
}
