// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ProjectCommunication;

/// <summary>
/// ProjectCommunication entity
/// </summary>
public class ProjectCommunication : FullAuditedAggregateRoot<Guid>
{
    public string Channel { get; set; } = string.Empty;
    public string Subject { get; set; }
    public string Content { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProjectId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Project.Project? Project { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ProjectCommunication()
    {
        // Required by EF Core
    }

    public ProjectCommunication(Guid id) : base(id)
    {
    }
}
