// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finRateiosPadrao;

/// <summary>
/// finRateiosPadrao entity
/// </summary>
public class finRateiosPadrao : FullAuditedAggregateRoot<Guid>
{
    public int? idPadrao { get; set; }
    public int idUnidade { get; set; }
    public int idCentroResultado { get; set; }
    public double porcentagem { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finRateiosPadrao()
    {
        // Required by EF Core
    }

    public finRateiosPadrao(Guid id) : base(id)
    {
    }
}
