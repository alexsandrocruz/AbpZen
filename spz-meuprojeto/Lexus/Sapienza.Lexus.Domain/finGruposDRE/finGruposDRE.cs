// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finGruposDRE;

/// <summary>
/// finGruposDRE entity
/// </summary>
public class finGruposDRE : FullAuditedAggregateRoot<Guid>
{
    public int? idGrupoDRE { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finGruposDRE()
    {
        // Required by EF Core
    }

    public finGruposDRE(Guid id) : base(id)
    {
    }
}
