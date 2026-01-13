// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advProFases;

/// <summary>
/// advProFases entity
/// </summary>
public class advProFases : FullAuditedAggregateRoot<Guid>
{
    public int? idFase { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advProFases()
    {
        // Required by EF Core
    }

    public advProFases(Guid id) : base(id)
    {
    }
}
