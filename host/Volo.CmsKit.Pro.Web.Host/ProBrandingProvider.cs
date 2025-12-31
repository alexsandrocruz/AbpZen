using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Pro;

[Dependency(ReplaceServices = true)]
public class ProBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Pro";
}
