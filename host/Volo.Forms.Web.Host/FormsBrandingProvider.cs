using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Volo.Forms;

[Dependency(ReplaceServices = true)]
public class FormsBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Forms";
}
