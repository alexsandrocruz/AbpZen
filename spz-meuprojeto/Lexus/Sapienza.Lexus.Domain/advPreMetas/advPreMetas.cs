// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreMetas;

/// <summary>
/// advPreMetas entity
/// </summary>
public class advPreMetas : FullAuditedAggregateRoot<Guid>
{
    public int? idMeta { get; set; }
    public string? tipo { get; set; }
    public int? idResponsavel { get; set; }
    public int? idEscritorio { get; set; }
    public int? qtde { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPreMetas()
    {
        // Required by EF Core
    }

    public advPreMetas(Guid id) : base(id)
    {
    }
}
