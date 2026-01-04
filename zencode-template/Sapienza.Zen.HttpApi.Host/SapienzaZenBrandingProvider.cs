using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.Zen
{
    [Dependency(ReplaceServices = true)]
    public class SapienzaZenBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Zen";
    }
}
