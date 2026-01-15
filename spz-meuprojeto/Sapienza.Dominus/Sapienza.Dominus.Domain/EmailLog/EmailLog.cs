// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.EmailLog;

/// <summary>
/// EmailLog entity
/// </summary>
public class EmailLog : Entity<Guid>
{
    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected EmailLog()
    {
        // Required by EF Core
    }

    public EmailLog(Guid id) : base(id)
    {
    }
}
