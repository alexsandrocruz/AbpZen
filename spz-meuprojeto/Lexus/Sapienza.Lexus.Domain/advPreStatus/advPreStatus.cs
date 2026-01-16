// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreStatus;

/// <summary>
/// advPreStatus entity
/// </summary>
public class advPreStatus : FullAuditedAggregateRoot<Guid>
{
    public int? idStatus { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? ordem { get; set; }
    public bool? ultimo { get; set; }
    public int? diasMaxParado { get; set; }
    public int? idTipo { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos> advPreStatuses { get; set; } = new List<Sapienza.Lexus.advPreStatusTipos.advPreStatusTipos>();

    protected advPreStatus()
    {
        // Required by EF Core
    }

    public advPreStatus(Guid id) : base(id)
    {
    }
}
