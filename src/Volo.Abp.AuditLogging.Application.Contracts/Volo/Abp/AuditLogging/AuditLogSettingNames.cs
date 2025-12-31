namespace Volo.Abp.AuditLogging;

public static class AuditLogSettingNames
{
    public const string GroupName = "Abp.AuditLogging";

    public const string IsPeriodicDeleterEnabled = GroupName + ".IsPeriodicDeleterEnabled";
    public const string IsExpiredDeleterEnabled = GroupName + ".IsExpiredDeleterEnabled";
    public const string ExpiredDeleterPeriod = GroupName + ".ExpiredDeleterPeriod";
}