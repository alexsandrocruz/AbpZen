using System.Linq;
using Volo.Abp.Features;

namespace Volo.Chat;

public class FakeChatFeatureDefinitionProvider : FeatureDefinitionProvider
{
    public override void Define(IFeatureDefinitionContext context)
    {
        var group = context.GetGroupOrNull(ChatFeatures.GroupName);
        group.Features.First(x => x.Name == ChatFeatures.Enable).DefaultValue = "true";
    }
}