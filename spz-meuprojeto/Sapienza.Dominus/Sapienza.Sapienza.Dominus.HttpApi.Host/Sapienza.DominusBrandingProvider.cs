using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.Sapienza.Dominus
{
    [Dependency(ReplaceServices = true)]
    public class Sapienza.DominusBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Sapienza.Dominus";
    }
}
