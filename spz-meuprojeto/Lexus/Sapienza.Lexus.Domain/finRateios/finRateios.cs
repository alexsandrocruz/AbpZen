// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finRateios;

/// <summary>
/// finRateios entity
/// </summary>
public class finRateios : FullAuditedAggregateRoot<Guid>
{
    public int? idRateio { get; set; }
    public int idLancamento { get; set; }
    public int idCentroCusto { get; set; }
    public int? idCentroResultado { get; set; }
    public double? percentualCC { get; set; }
    public double? percentualCR { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public int? idUnidade { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finRateios()
    {
        // Required by EF Core
    }

    public finRateios(Guid id) : base(id)
    {
    }
}
