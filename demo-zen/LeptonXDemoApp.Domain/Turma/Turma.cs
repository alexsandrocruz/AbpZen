using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Turma;

/// <summary>
/// Turma entity
/// </summary>
public class Turma : FullAuditedEntity<Guid>
{
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public int AnoLetivo { get; set; }
    public int Semestre { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.AlunoTurma.AlunoTurma> AlunoTurmas { get; set; } = new List<LeptonXDemoApp.AlunoTurma.AlunoTurma>();

    protected Turma()
    {
        // Required by EF Core
    }

    public Turma(Guid id) : base(id)
    {
    }
}
