using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace Volo.Abp.AuditLogging;

public class AbpAuditLoggingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var permissionGroup = context.AddGroup(AbpAuditLoggingPermissions.GroupName, L("Permission:AuditLogging"));

        var auditLogPermission = permissionGroup
            .AddPermission(AbpAuditLoggingPermissions.AuditLogs.Default, L("Permission:AuditLogs"))
            .RequireFeatures(AbpAuditLoggingFeatures.Enable);

        auditLogPermission.AddChild(
                AbpAuditLoggingPermissions.AuditLogs.SettingManagement,
                L("Permission:SettingManagement"))
            .RequireFeatures(AbpAuditLoggingFeatures.SettingManagement);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AuditLoggingResource>(name);
    }
}
