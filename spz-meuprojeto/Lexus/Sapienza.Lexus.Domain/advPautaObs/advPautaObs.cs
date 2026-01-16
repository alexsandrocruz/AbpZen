// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPautaObs;

/// <summary>
/// advPautaObs entity
/// </summary>
public class advPautaObs : FullAuditedAggregateRoot<Guid>
{
    public int? idPautaObs { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? id { get; set; }
    public string? idTipo { get; set; }
    public string? observacao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPautaObs()
    {
        // Required by EF Core
    }

    public advPautaObs(Guid id) : base(id)
    {
    }
}
