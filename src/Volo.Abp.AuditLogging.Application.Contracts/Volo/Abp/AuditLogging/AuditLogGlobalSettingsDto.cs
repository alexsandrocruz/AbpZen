namespace Volo.Abp.AuditLogging;

public class AuditLogGlobalSettingsDto : AuditLogSettingsDto
{
    public bool IsPeriodicDeleterEnabled { get; set; }
}