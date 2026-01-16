// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.opoTipos;

/// <summary>
/// opoTipos entity
/// </summary>
public class opoTipos : FullAuditedAggregateRoot<Guid>
{
    public int? idTipo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? opoOportunidadesId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.opoOportunidades.opoOportunidades? opoTiposNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected opoTipos()
    {
        // Required by EF Core
    }

    public opoTipos(Guid id) : base(id)
    {
    }
}
