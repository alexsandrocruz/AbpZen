// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCliPrioridades;

/// <summary>
/// advCliPrioridades entity
/// </summary>
public class advCliPrioridades : FullAuditedAggregateRoot<Guid>
{
    public int? idPrioridade { get; set; }
    public string? titulo { get; set; }
    public string? cor { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advCliPrioridades()
    {
        // Required by EF Core
    }

    public advCliPrioridades(Guid id) : base(id)
    {
    }
}
