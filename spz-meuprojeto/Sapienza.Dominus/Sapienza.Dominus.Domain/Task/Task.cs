// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Task;

/// <summary>
/// Task entity
/// </summary>
public class Task : FullAuditedEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProjectId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Project.Project? Project { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.TaskComment.TaskComment> TaskComments { get; set; } = new List<Dominus.TaskComment.TaskComment>();

    protected Task()
    {
        // Required by EF Core
    }

    public Task(Guid id) : base(id)
    {
    }
}
