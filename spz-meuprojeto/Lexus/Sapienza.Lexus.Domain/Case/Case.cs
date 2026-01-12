#nullable enable
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.Case;

/// <summary>
/// Case entity
/// </summary>
public class Case : FullAuditedAggregateRoot<Guid>
{
    public string? CaseNumber { get; set; }
    public string? Title { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected Case()
    {
        // Required by EF Core
    }

    public Case(Guid id) : base(id)
    {
    }
}
