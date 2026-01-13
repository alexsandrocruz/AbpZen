// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advPreOrigens;

/// <summary>
/// advPreOrigens entity
/// </summary>
public class advPreOrigens : FullAuditedAggregateRoot<Guid>
{
    public int? idOrigem { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advPreOrigens()
    {
        // Required by EF Core
    }

    public advPreOrigens(Guid id) : base(id)
    {
    }
}
