// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.WorkspaceInvite;

/// <summary>
/// WorkspaceInvite entity
/// </summary>
public class WorkspaceInvite : Entity<Guid>
{
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected WorkspaceInvite()
    {
        // Required by EF Core
    }

    public WorkspaceInvite(Guid id) : base(id)
    {
    }
}
