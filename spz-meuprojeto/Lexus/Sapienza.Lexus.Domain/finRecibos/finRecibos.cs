// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finRecibos;

/// <summary>
/// finRecibos entity
/// </summary>
public class finRecibos : FullAuditedAggregateRoot<Guid>
{
    public int? idRecibo { get; set; }
    public int? idLancamento { get; set; }
    public int? numero { get; set; }
    public string? referente { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finRecibos()
    {
        // Required by EF Core
    }

    public finRecibos(Guid id) : base(id)
    {
    }
}
