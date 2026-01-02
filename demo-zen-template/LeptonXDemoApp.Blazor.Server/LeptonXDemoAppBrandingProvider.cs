using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LeptonXDemoApp.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class LeptonXDemoAppBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "LeptonXDemoApp";
    }
}
