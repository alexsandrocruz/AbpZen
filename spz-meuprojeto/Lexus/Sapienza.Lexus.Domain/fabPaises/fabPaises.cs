// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabPaises;

/// <summary>
/// fabPaises entity
/// </summary>
public class fabPaises : FullAuditedAggregateRoot<Guid>
{
    public int? idPais { get; set; }
    public string? titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabPaises()
    {
        // Required by EF Core
    }

    public fabPaises(Guid id) : base(id)
    {
    }
}
