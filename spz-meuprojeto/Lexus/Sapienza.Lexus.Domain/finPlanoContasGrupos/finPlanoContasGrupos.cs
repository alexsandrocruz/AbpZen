// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finPlanoContasGrupos;

/// <summary>
/// finPlanoContasGrupos entity
/// </summary>
public class finPlanoContasGrupos : FullAuditedAggregateRoot<Guid>
{
    public int? idGrupo { get; set; }
    public string? titulo { get; set; }
    public string? tipo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idGrupoDRE { get; set; }
    public int? ordem { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finPlanoContasGrupos()
    {
        // Required by EF Core
    }

    public finPlanoContasGrupos(Guid id) : base(id)
    {
    }
}
