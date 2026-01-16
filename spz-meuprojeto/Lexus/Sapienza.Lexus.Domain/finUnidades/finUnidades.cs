// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finUnidades;

/// <summary>
/// finUnidades entity
/// </summary>
public class finUnidades : FullAuditedAggregateRoot<Guid>
{
    public int? idUnidade { get; set; }
    public string? titulo { get; set; }
    public double? percentual { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected finUnidades()
    {
        // Required by EF Core
    }

    public finUnidades(Guid id) : base(id)
    {
    }
}
