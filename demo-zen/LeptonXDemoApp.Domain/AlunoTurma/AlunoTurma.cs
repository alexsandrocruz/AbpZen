using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.AlunoTurma;

/// <summary>
/// AlunoTurma entity
/// </summary>
public class AlunoTurma : FullAuditedEntity<Guid>
{
    public DateTime DataMatricula { get; set; }
    public string? Situacao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid AlunoId { get; set; }
    public Guid TurmaId { get; set; }

    // ========== Navigation Properties ==========
    public virtual LeptonXDemoApp.Aluno.Aluno Aluno { get; set; }
    public virtual LeptonXDemoApp.Turma.Turma Turma { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected AlunoTurma()
    {
        // Required by EF Core
    }

    public AlunoTurma(Guid id) : base(id)
    {
    }
}
