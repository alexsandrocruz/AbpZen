// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreStatusTipos;

/// <summary>
/// advPreStatusTipos entity
/// </summary>
public class advPreStatusTipos : FullAuditedAggregateRoot<Guid>
{
    public int? idTipo { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid advPreStatusId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advPreStatus.advPreStatus advPreStatusTiposNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPreStatusTipos()
    {
        // Required by EF Core
    }

    public advPreStatusTipos(Guid id) : base(id)
    {
    }
}
