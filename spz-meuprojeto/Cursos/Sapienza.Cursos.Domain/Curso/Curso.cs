using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Cursos.Curso;

/// <summary>
/// Curso entity
/// </summary>
public class Curso : FullAuditedAggregateRoot<Guid>
{
    public string? Name { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Curso()
    {
        // Required by EF Core
    }

    public Curso(Guid id) : base(id)
    {
    }
}
