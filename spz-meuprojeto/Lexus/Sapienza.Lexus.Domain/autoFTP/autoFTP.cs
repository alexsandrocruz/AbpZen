// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.autoFTP;

/// <summary>
/// autoFTP entity
/// </summary>
public class autoFTP : FullAuditedAggregateRoot<Guid>
{
    public int? id { get; set; }
    public string? arquivo { get; set; }
    public bool? processado { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected autoFTP()
    {
        // Required by EF Core
    }

    public autoFTP(Guid id) : base(id)
    {
    }
}
