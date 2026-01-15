using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Sapienza.Dominus.Web
{
    [Dependency(ReplaceServices = true)]
    public class Sapienza.DominusBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Sapienza.Dominus";
    }
}
