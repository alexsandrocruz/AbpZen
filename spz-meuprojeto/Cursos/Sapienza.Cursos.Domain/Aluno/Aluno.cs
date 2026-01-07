using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Cursos.Aluno;

/// <summary>
/// Aluno entity
/// </summary>
public class Aluno : FullAuditedAggregateRoot<Guid>
{
    public string? Nome { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Aluno()
    {
        // Required by EF Core
    }

    public Aluno(Guid id) : base(id)
    {
    }
}
