// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.flwConfig;

/// <summary>
/// flwConfig entity
/// </summary>
public class flwConfig : FullAuditedAggregateRoot<Guid>
{
    public int? idConfig { get; set; }
    public string? tipoMarcacoes { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected flwConfig()
    {
        // Required by EF Core
    }

    public flwConfig(Guid id) : base(id)
    {
    }
}
