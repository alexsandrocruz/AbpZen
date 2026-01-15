// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.TaskComment;

/// <summary>
/// TaskComment entity
/// </summary>
public class TaskComment : AuditedEntity<Guid>
{
    public string Content { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? TaskId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Task.Task? Task { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected TaskComment()
    {
        // Required by EF Core
    }

    public TaskComment(Guid id) : base(id)
    {
    }
}
