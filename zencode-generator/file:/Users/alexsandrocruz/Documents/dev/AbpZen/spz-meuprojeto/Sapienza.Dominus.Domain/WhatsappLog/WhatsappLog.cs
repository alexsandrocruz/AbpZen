// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.WhatsappLog;

/// <summary>
/// WhatsappLog entity
/// </summary>
public class WhatsappLog : Entity<Guid>
{
    public string ToPhone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected WhatsappLog()
    {
        // Required by EF Core
    }

    public WhatsappLog(Guid id) : base(id)
    {
    }
}
