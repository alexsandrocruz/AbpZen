using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplateManagement.Localization;
using Volo.Abp.Validation.StringValues;

namespace Volo.Abp.TextTemplateManagement;

public class TextManagementFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(TextManagementFeatures.GroupName,
            L("Feature:TextManagementGroup"));

        group.AddFeature(TextManagementFeatures.Enable,
            "true",
            L("Feature:TextManagementEnable"),
            L("Feature:TextManagementEnableDescription"),
            new ToggleStringValueType());
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TextTemplateManagementResource>(name);
    }
}
