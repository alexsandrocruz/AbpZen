// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advCliLocaisAtendido;

/// <summary>
/// advCliLocaisAtendido entity
/// </summary>
public class advCliLocaisAtendido : FullAuditedAggregateRoot<Guid>
{
    public int? idLocalAtendido { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advCliLocaisAtendido()
    {
        // Required by EF Core
    }

    public advCliLocaisAtendido(Guid id) : base(id)
    {
    }
}
