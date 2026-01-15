// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.ProjectFollower;

/// <summary>
/// ProjectFollower entity
/// </summary>
public class ProjectFollower : Entity<Guid>
{

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ProjectId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Project.Project? Project { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected ProjectFollower()
    {
        // Required by EF Core
    }

    public ProjectFollower(Guid id) : base(id)
    {
    }
}
