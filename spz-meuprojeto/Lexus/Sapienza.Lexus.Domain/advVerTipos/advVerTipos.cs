// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advVerTipos;

/// <summary>
/// advVerTipos entity
/// </summary>
public class advVerTipos : FullAuditedAggregateRoot<Guid>
{
    public int? idTipo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? advVerbasId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advVerbas.advVerbas? advVerTiposNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advVerTipos()
    {
        // Required by EF Core
    }

    public advVerTipos(Guid id) : base(id)
    {
    }
}
