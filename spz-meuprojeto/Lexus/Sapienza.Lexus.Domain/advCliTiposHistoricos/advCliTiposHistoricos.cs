// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCliTiposHistoricos;

/// <summary>
/// advCliTiposHistoricos entity
/// </summary>
public class advCliTiposHistoricos : FullAuditedAggregateRoot<Guid>
{
    public int? idTipoHistorico { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advCliTiposHistoricos()
    {
        // Required by EF Core
    }

    public advCliTiposHistoricos(Guid id) : base(id)
    {
    }
}
