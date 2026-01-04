using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Aluno;

/// <summary>
/// Aluno entity
/// </summary>
public class Aluno : FullAuditedEntity<Guid>
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<LeptonXDemoApp.AlunoTurma.AlunoTurma> AlunoTurmas { get; set; } = new List<LeptonXDemoApp.AlunoTurma.AlunoTurma>();

    protected Aluno()
    {
        // Required by EF Core
    }

    public Aluno(Guid id) : base(id)
    {
    }
}
