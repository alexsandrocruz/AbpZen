using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Dominus.Web
{
    [Dependency(ReplaceServices = true)]
    public class DominusBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Dominus";
    }
}
