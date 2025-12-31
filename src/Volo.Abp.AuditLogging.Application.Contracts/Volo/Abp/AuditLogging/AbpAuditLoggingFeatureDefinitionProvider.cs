using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.AuditLogging;

public class AbpAuditLoggingFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(AbpAuditLoggingFeatures.GroupName,
            L("Feature:AuditLoggingGroup"));

        var auditLoggingEnable = group.AddFeature(AbpAuditLoggingFeatures.Enable,
            "true",
            L("Feature:AuditLoggingEnable"),
            L("Feature:AuditLoggingEnableDescription"),
            new ToggleStringValueType());

        auditLoggingEnable.CreateChildFeature(AbpAuditLoggingFeatures.SettingManagement,
            "false",
            L("Feature:AuditLoggingSettingManagementEnable"),
            L("Feature:AuditLoggingSettingManagementEnableDescription"),
            new ToggleStringValueType());
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AuditLoggingResource>(name);
    }
}