namespace Volo.Abp.AuditLogging;

public class AuditLogSettingsDto
{
    public bool IsExpiredDeleterEnabled { get; set; }
    public int ExpiredDeleterPeriod { get; set; }
}