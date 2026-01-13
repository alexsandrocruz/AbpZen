// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabDatasEFeriados;

/// <summary>
/// fabDatasEFeriados entity
/// </summary>
public class fabDatasEFeriados : FullAuditedAggregateRoot<Guid>
{
    public int? idData { get; set; }
    public string? titulo { get; set; }
    public string? data { get; set; }
    public bool? feriado { get; set; }
    public bool? fixo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabDatasEFeriados()
    {
        // Required by EF Core
    }

    public fabDatasEFeriados(Guid id) : base(id)
    {
    }
}
