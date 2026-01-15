using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.Dominus.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class DominusBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Dominus";
    }
}
