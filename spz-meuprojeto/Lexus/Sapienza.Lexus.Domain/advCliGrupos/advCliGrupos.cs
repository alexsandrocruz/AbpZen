// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCliGrupos;

/// <summary>
/// advCliGrupos entity
/// </summary>
public class advCliGrupos : FullAuditedAggregateRoot<Guid>
{
    public int? idGrupo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advCliGrupos()
    {
        // Required by EF Core
    }

    public advCliGrupos(Guid id) : base(id)
    {
    }
}
