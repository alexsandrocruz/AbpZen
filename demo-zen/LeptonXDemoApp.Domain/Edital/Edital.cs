using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Edital;

/// <summary>
/// Edital entity
/// </summary>
public class Edital : FullAuditedAggregateRoot<Guid>
{
    public string? Objeto { get; set; }
    public DateTime? Data { get; set; }
    public decimal? Valor { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Edital()
    {
        // Required by EF Core
    }

    public Edital(Guid id) : base(id)
    {
    }
}
