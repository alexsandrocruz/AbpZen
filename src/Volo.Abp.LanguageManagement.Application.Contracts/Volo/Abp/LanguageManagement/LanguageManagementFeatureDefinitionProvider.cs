using Volo.Abp.Features;
using Volo.Abp.LanguageManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.LanguageManagement;

public class LanguageManagementFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(LanguageManagementFeatures.GroupName,
            L("Feature:LanguageManagementGroup"));

        group.AddFeature(LanguageManagementFeatures.Enable,
            "true",
            L("Feature:LanguageManagementEnable"),
            L("Feature:LanguageManagementEnableDescription"),
            new ToggleStringValueType());
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LanguageManagementResource>(name);
    }
}
