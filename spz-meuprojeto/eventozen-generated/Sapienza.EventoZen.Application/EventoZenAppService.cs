using Sapienza.EventoZen.Localization;
using Volo.Abp.Application.Services;

namespace Sapienza.EventoZen
{
    /* Inherit your application services from this class.
     */
    public abstract class EventoZenAppService : ApplicationService
    {
        protected EventoZenAppService()
        {
            LocalizationResource = typeof(EventoZenResource);
        }
    }
}
