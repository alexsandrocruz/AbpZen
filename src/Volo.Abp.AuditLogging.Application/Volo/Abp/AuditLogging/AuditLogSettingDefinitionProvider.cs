using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.AuditLogging;

public class AuditLogSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(
                AuditLogSettingNames.IsPeriodicDeleterEnabled,
                "false",
                L("DisplayName:IsPeriodicDeleterEnabled"),
                L("Description:IsPeriodicDeleterEnabled"),
                isInherited: false)
                .WithProviders(
                    GlobalSettingValueProvider.ProviderName,
                    ConfigurationSettingValueProvider.ProviderName),

            new SettingDefinition(AuditLogSettingNames.IsExpiredDeleterEnabled,
                "false",
                L("DisplayName:IsExpiredDeleterEnabled"),
                L("Description:IsExpiredDeleterEnabled"),
                isInherited: false),

            new SettingDefinition(AuditLogSettingNames.ExpiredDeleterPeriod,
                null,
                L("DisplayName:ExpiredDeleterPeriod"),
                L("Description:ExpiredDeleterPeriod"),
                isInherited: false)
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AuditLoggingResource>(name);
    }
}