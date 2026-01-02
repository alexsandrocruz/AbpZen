using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LeptonXDemoApp
{
    [Dependency(ReplaceServices = true)]
    public class LeptonXDemoAppBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "LeptonXDemoApp";
    }
}
