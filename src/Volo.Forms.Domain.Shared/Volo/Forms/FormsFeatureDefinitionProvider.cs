using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.Validation.StringValues;
using Volo.Forms.Localization;

namespace Volo.Forms;

public class FormsFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.AddGroup(FormsFeatures.GroupName,
            L("Feature:FormsGroup"));

        group.AddFeature(FormsFeatures.Enable,
            "true",
            L("Feature:FormsEnable"),
            L("Feature:FormsEnableDescription"),
            new ToggleStringValueType());
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FormsResource>(name);
    }
}
