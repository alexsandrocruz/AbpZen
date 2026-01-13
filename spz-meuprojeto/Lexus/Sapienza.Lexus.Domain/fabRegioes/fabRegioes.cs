// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.fabRegioes;

/// <summary>
/// fabRegioes entity
/// </summary>
public class fabRegioes : FullAuditedAggregateRoot<Guid>
{
    public int? idRegiao { get; set; }
    public string? titulo { get; set; }
    public string? estados { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected fabRegioes()
    {
        // Required by EF Core
    }

    public fabRegioes(Guid id) : base(id)
    {
    }
}
