// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.SmsLog;

/// <summary>
/// SmsLog entity
/// </summary>
public class SmsLog : Entity<Guid>
{
    public string ToPhone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected SmsLog()
    {
        // Required by EF Core
    }

    public SmsLog(Guid id) : base(id)
    {
    }
}
