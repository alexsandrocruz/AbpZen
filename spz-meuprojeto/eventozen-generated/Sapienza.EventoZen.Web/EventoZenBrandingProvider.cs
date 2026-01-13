using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Sapienza.EventoZen.Web
{
    [Dependency(ReplaceServices = true)]
    public class EventoZenBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.EventoZen";
    }
}
