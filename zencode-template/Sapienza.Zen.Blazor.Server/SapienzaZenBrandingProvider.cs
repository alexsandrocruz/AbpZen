using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.Zen.Blazor
{
    [Dependency(ReplaceServices = true)]
    public class SapienzaZenBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Zen";
    }
}
