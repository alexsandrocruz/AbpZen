using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.Lawyer;

/// <summary>
/// Lawyer entity
/// </summary>
public class Lawyer : FullAuditedAggregateRoot<Guid>
{
    public string? FullName { get; set; }
    public string? PreferredName { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Lawyer()
    {
        // Required by EF Core
    }

    public Lawyer(Guid id) : base(id)
    {
    }
}
