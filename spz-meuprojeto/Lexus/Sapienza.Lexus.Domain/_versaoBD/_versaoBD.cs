// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus._versaoBD;

/// <summary>
/// _versaoBD entity
/// </summary>
public class _versaoBD : FullAuditedAggregateRoot<Guid>
{
    public int? id { get; set; }
    public string? arquivo { get; set; }
    public DateTime? dataAplicacao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected _versaoBD()
    {
        // Required by EF Core
    }

    public _versaoBD(Guid id) : base(id)
    {
    }
}
