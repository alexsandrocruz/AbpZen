// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabPermissoesTipos;

/// <summary>
/// fabPermissoesTipos entity
/// </summary>
public class fabPermissoesTipos : FullAuditedAggregateRoot<Guid>
{
    public int? idPermissaoTipo { get; set; }
    public string? descricao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabPermissoesTipos()
    {
        // Required by EF Core
    }

    public fabPermissoesTipos(Guid id) : base(id)
    {
    }
}
