// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProMeritos;

/// <summary>
/// advProMeritos entity
/// </summary>
public class advProMeritos : FullAuditedAggregateRoot<Guid>
{
    public int? idMerito { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? beneficioINSS { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? advProcessosMeritosId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.advProcessosMeritos.advProcessosMeritos? advProMeritosNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProMeritos()
    {
        // Required by EF Core
    }

    public advProMeritos(Guid id) : base(id)
    {
    }
}
