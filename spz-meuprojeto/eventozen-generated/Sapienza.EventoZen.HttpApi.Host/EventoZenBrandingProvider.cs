using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.EventoZen
{
    [Dependency(ReplaceServices = true)]
    public class EventoZenBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.EventoZen";
    }
}
