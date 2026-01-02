using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace LeptonXDemoApp.Web
{
    [Dependency(ReplaceServices = true)]
    public class LeptonXDemoAppBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "LeptonXDemoApp";
    }
}
