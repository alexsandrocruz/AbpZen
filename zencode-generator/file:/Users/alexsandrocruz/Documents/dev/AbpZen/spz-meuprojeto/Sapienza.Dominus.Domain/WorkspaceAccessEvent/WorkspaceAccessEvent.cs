// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

/// <summary>
/// WorkspaceAccessEvent entity
/// </summary>
public class WorkspaceAccessEvent : Entity<Guid>
{
    public string EventType { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected WorkspaceAccessEvent()
    {
        // Required by EF Core
    }

    public WorkspaceAccessEvent(Guid id) : base(id)
    {
    }
}
