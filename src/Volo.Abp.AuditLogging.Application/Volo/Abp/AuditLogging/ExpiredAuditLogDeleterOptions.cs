using System;

namespace Volo.Abp.AuditLogging;

public class ExpiredAuditLogDeleterOptions
{
    /// <summary>
    /// Default: Everyday once.
    /// </summary>
    public int Period { get; set; } = (int)TimeSpan.FromDays(1).TotalMilliseconds;
}