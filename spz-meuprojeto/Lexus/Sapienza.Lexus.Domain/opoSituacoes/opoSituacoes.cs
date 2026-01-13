// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.opoSituacoes;

/// <summary>
/// opoSituacoes entity
/// </summary>
public class opoSituacoes : FullAuditedAggregateRoot<Guid>
{
    public int? idSituacao { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? ordem { get; set; }
    public bool? considerarIndicador { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected opoSituacoes()
    {
        // Required by EF Core
    }

    public opoSituacoes(Guid id) : base(id)
    {
    }
}
