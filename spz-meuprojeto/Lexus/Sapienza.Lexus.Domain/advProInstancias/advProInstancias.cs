// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProInstancias;

/// <summary>
/// advProInstancias entity
/// </summary>
public class advProInstancias : FullAuditedAggregateRoot<Guid>
{
    public int? idInstancia { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProInstancias()
    {
        // Required by EF Core
    }

    public advProInstancias(Guid id) : base(id)
    {
    }
}
