using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Cursos.Turma;

/// <summary>
/// Turma entity
/// </summary>
public class Turma : FullAuditedAggregateRoot<Guid>
{
    public string? Nome { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Turma()
    {
        // Required by EF Core
    }

    public Turma(Guid id) : base(id)
    {
    }
}
