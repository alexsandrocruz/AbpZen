// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCliBairros;

/// <summary>
/// advCliBairros entity
/// </summary>
public class advCliBairros : FullAuditedAggregateRoot<Guid>
{
    public int? idBairro { get; set; }
    public string? titulo { get; set; }
    public string? cidade { get; set; }
    public string? estado { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advCliBairros()
    {
        // Required by EF Core
    }

    public advCliBairros(Guid id) : base(id)
    {
    }
}
