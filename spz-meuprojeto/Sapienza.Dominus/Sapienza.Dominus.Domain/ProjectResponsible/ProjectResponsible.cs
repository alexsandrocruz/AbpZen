// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ProjectResponsible;

/// <summary>
/// ProjectResponsible entity
/// </summary>
public class ProjectResponsible : Entity<Guid>
{

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProjectId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Project.Project? Project { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ProjectResponsible()
    {
        // Required by EF Core
    }

    public ProjectResponsible(Guid id) : base(id)
    {
    }
}
