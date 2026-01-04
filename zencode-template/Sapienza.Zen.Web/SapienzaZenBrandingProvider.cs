using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Zen.Web
{
    [Dependency(ReplaceServices = true)]
    public class SapienzaZenBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Zen";
    }
}
